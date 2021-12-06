using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCameraFollow : MonoBehaviour
{

    [SerializeField]
    float smoothTime;
    [SerializeField]
    Vector3 velocity = Vector3.zero;

    public Transform target;

    void FollowTarget()
    {
        if(target)
        {
            transform.position = Vector3.SmoothDamp(transform.position,
                new Vector3(target.position.x, target.position.y, transform.position.z),
                ref velocity, smoothTime);
        }
    }

    private void Update()
    {
        FollowTarget();
    }
}
