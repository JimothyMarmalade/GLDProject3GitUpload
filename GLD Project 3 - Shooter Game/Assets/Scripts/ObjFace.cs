using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjFace : MonoBehaviour
{
    public Transform ObjToFollow = null;
    public bool FollowPlayer = true;

    private void Awake()
    {
        if (!FollowPlayer)
            return;
        GameObject PlayerObj = GameObject.FindGameObjectWithTag("Player");

        if (PlayerObj != null)
        {
            ObjToFollow = PlayerObj.transform;
        }
    }

    private void Update()
    {
        if (ObjToFollow == null)
            return;

        Vector3 DirToObject = ObjToFollow.position - transform.position;

        if (DirToObject != Vector3.zero)
        {
            Quaternion thisWay = Quaternion.LookRotation(DirToObject.normalized, Vector3.up);
            thisWay.x = 0;
            thisWay.z = 0;
            transform.localRotation = thisWay;
        }


    }

}
