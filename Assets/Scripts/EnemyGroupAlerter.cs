using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupAlerter : MonoBehaviour
{
    public void AlertEnemyGroup()
    {
        BroadcastMessage("GetAlerted", SendMessageOptions.DontRequireReceiver);
    }
}
