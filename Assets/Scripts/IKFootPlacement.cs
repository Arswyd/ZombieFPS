using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootPlacement : MonoBehaviour
{
    Vector3 rightFootPosition, leftFootPosition, leftFootIKPosition, rightFootIKPosition;
    Quaternion rightFootIKRotation, leftFootIKRotation, currentLeftFootIkRotation, currentRightFootIkRotation;
    float lastPelvisPositionY, lastRightFootPositionY, lastLeftFootPositionY;

    [SerializeField] bool enableFeetIK = true;
    [Range(0,2)][SerializeField] float heightFromGroundRaycast = 1.14f;
    [Range(0,2)][SerializeField] float raycastDownDistance = 1.5f;
    [SerializeField] LayerMask environmentLayer;
    [SerializeField] float pelvisOffset = 0f;
    [Range(0,1)][SerializeField] float pelvisUpAndDownSpeed = 0.28f;
    [Range(0,1)][SerializeField] float feetToIKPositionSpeed = 0.5f;

    public string leftFootAnimVariableName = "LeftFootCurve";
    public string rightFootAnimVariableName = "RightFootCurve";

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
        if(!enableFeetIK) {return;}
        if(animator == null) {return;}

        AdjustFeetTarget(ref rightFootPosition, HumanBodyBones.RightFoot);
        AdjustFeetTarget(ref leftFootPosition, HumanBodyBones.LeftFoot);

        FeetPositionSolver(rightFootPosition, ref rightFootIKPosition, ref rightFootIKRotation);
        FeetPositionSolver(leftFootPosition, ref leftFootIKPosition, ref leftFootIKRotation);
    }

    void OnAnimatorIK(int layerIndex) 
    {
        if(!enableFeetIK) {return;}
        if(animator == null) {return;}

        MovePelvisHeight();

        //right foot ik position and rotation
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat(rightFootAnimVariableName));

        MoveFeetToIKPoint(AvatarIKGoal.RightFoot, rightFootIKPosition, rightFootIKRotation, ref lastRightFootPositionY);

        //left foot ik position and rotation
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat(leftFootAnimVariableName));

        MoveFeetToIKPoint(AvatarIKGoal.LeftFoot, leftFootIKPosition, leftFootIKRotation, ref lastLeftFootPositionY);
    }

    void MoveFeetToIKPoint(AvatarIKGoal foot, Vector3 positionIKHolder, Quaternion rotationIKHolder, ref float lastFootPositionY)
    {
        Vector3 targetIKPosition = animator.GetIKPosition(foot);
        Quaternion targetIKRotation = rotationIKHolder * animator.GetIKRotation(foot);

        if(positionIKHolder != Vector3.zero)
        {
            targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
            positionIKHolder = transform.InverseTransformPoint(positionIKHolder);

            float y = Mathf.Lerp(lastFootPositionY, positionIKHolder.y, feetToIKPositionSpeed);
            targetIKPosition.y += y;

            lastFootPositionY = y;

            targetIKPosition = transform.TransformPoint(targetIKPosition);

            animator.SetIKRotation(foot, targetIKRotation);
        }

        animator.SetIKPosition(foot, targetIKPosition);
    }

    void MovePelvisHeight()
    {
        if(rightFootIKPosition == Vector3.zero || leftFootIKPosition == Vector3.zero || lastPelvisPositionY == 0)
        {
            lastPelvisPositionY = animator.bodyPosition.y;
            return;
        }

        float leftOffsetPosition = leftFootIKPosition.y - transform.position.y;
        float rightOffsetPosition = rightFootIKPosition.y - transform.position.y;

        float totalOffset = (leftOffsetPosition < rightOffsetPosition) ? leftOffsetPosition : rightOffsetPosition;

        Vector3 newPelvisPosition = animator.bodyPosition + Vector3.up * totalOffset;

        newPelvisPosition.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);

        animator.bodyPosition = newPelvisPosition;

        lastPelvisPositionY = animator.bodyPosition.y;
    }

    void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIKPositions, ref Quaternion feetIKRotations)
    {
        RaycastHit hit;

        if(showSolverDebug) 
            Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.yellow);

        if (Physics.Raycast(fromSkyPosition, Vector3.down, out hit, raycastDownDistance + heightFromGroundRaycast, environmentLayer))
        {
            Debug.DrawLine(fromSkyPosition, hit.point, Color.magenta);
            feetIKPositions = fromSkyPosition;
            feetIKPositions.y = hit.point.y + pelvisOffset;
            feetIKRotations = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;

            return;
        }

        feetIKPositions = Vector3.zero;
    }

    void AdjustFeetTarget(ref Vector3 feetPositions, HumanBodyBones foot)
    {
        feetPositions = animator.GetBoneTransform(foot).position;
        feetPositions.y = transform.position.y + heightFromGroundRaycast;
    }
}
