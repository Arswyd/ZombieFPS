using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHandPlacement : MonoBehaviour
{
    Vector3 rightHandPosition, leftHandPosition, leftHandIKPosition, rightHandIKPosition;
    Quaternion rightHandIKRotation, leftHandIKRotation, currentLeftHandIkRotation, currentRightHandIkRotation;
    float lastRightHandPositionY, lastLeftHandPositionY;

    [SerializeField] bool enableHandIK = true;
    [Range(0,2)][SerializeField] float heightFromGroundRaycast = 1.14f;
    [Range(0,2)][SerializeField] float raycastDownDistance = 1.5f;
    [SerializeField] LayerMask environmentLayer;
    [Range(0,1)][SerializeField] float HandToIKPositionSpeed = 0.5f;

    public string leftHandAnimVariableName = "LeftHandCurve";
    public string rightHandAnimVariableName = "RightHandCurve";

    public bool showSolverDebug = false;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!enableHandIK) {return;}
        if(animator == null) {return;}

        AdjustHandTarget(ref rightHandPosition, HumanBodyBones.RightHand);
        AdjustHandTarget(ref leftHandPosition, HumanBodyBones.LeftHand);

        HandPositionSolver(rightHandPosition, ref rightHandIKPosition, ref rightHandIKRotation);
        HandPositionSolver(leftHandPosition, ref leftHandIKPosition, ref leftHandIKRotation);
    }

    void OnAnimatorIK(int layerIndex) 
    {
        if(!enableHandIK) {return;}
        if(animator == null) {return;}

        //right Hand ik position and rotation
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, animator.GetFloat(rightHandAnimVariableName));

        MoveHandToIKPoint(AvatarIKGoal.RightHand, rightHandIKPosition, rightHandIKRotation, ref lastRightHandPositionY);

        //left Hand ik position and rotation
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        //animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, animator.GetFloat(leftHandAnimVariableName));

        MoveHandToIKPoint(AvatarIKGoal.LeftHand, leftHandIKPosition, leftHandIKRotation, ref lastLeftHandPositionY);
    }

    void MoveHandToIKPoint(AvatarIKGoal hand, Vector3 positionIKHolder, Quaternion rotationIKHolder, ref float lastHandPositionY)
    {
        Vector3 targetIKPosition = animator.GetIKPosition(hand);
        Quaternion targetIKRotation = rotationIKHolder * animator.GetIKRotation(hand);

        if(positionIKHolder != Vector3.zero)
        {
            targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
            positionIKHolder = transform.InverseTransformPoint(positionIKHolder);

            float y = Mathf.Lerp(lastHandPositionY, positionIKHolder.y, HandToIKPositionSpeed);
            targetIKPosition.y += y;

            lastHandPositionY = y;

            targetIKPosition = transform.TransformPoint(targetIKPosition);

            animator.SetIKRotation(hand, targetIKRotation);
        }

        animator.SetIKPosition(hand, targetIKPosition);
    }

    void HandPositionSolver(Vector3 fromSkyPosition, ref Vector3 handIKPositions, ref Quaternion handIKRotations)
    {
        RaycastHit hit;

        if(showSolverDebug) 
            Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.yellow);

        if (Physics.Raycast(fromSkyPosition, Vector3.down, out hit, raycastDownDistance + heightFromGroundRaycast, environmentLayer))
        {
            Debug.DrawLine(fromSkyPosition, hit.point, Color.blue);
            handIKPositions = fromSkyPosition;
            handIKPositions.y = hit.point.y;
            handIKRotations = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;

            return;
        }

        handIKPositions = Vector3.zero;
    }

    void AdjustHandTarget(ref Vector3 handPositions, HumanBodyBones hand)
    {
        handPositions = animator.GetBoneTransform(hand).position;
        handPositions.y = transform.position.y + heightFromGroundRaycast;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(rightHandIKPosition, 0.05f);
    }
}
