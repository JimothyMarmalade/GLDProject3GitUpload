using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    private Transform Target;
    private Vector3 CameraTarget;
    public float CameraSpeed = 4;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            CameraTarget = new Vector3(Target.position.x, transform.position.y, Target.position.z);
            transform.position = Vector3.Lerp(transform.position, CameraTarget, Time.deltaTime * CameraSpeed);

        }

    }
}
