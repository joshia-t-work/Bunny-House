using BunnyHouse.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BunnyHouse.UI
{
    public class CameraScript : MonoBehaviour
    {
        private Vector3 dragOrigin;
        private Vector3 targetPosition;
        [SerializeField]
        private float boundsLeft;
        [SerializeField]
        private float boundsRight;
        private float camWidth;

        private void Awake()
        {
            targetPosition = transform.position;
            camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        }

        void Update()
        {
            if (!Singleton.isUIOverride)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }

                if (!Input.GetMouseButton(0)) return;

                Vector3 moveVec = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - dragOrigin);
                targetPosition = new Vector3(Mathf.Clamp(targetPosition.x - moveVec.x, boundsLeft + camWidth, boundsRight - camWidth), targetPosition.y, targetPosition.z);
                dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        private void FixedUpdate()
        {
            Vector3 change = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f) - transform.position;
            transform.position += change;
            dragOrigin += change;
        }
    }
}
