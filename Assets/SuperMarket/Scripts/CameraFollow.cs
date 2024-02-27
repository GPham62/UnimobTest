using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float smoothTime;
        Vector3 offset;
        Vector3 currentVelocity;

        private void Start()
        {
            offset = transform.position - target.position;
        }

        private void LateUpdate()
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        }
    }
}