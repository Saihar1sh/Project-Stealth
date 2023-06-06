using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    [SerializeField]
    private Vector3 camOffset = Vector3.zero;

    [SerializeField]
    Transform lookAtTransform;

    [SerializeField, Range(1,10)]
    float speed = 1.0f;

    private void LateUpdate()
    {
        transform.LookAt(lookAtTransform);
        transform.position =Vector3.MoveTowards(transform.position, lookAtTransform.position + camOffset, Time.deltaTime * speed);
    }
}
