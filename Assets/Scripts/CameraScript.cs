using BunnyHouse.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BunnyHouse.UI
{
    /// <summary>
    /// Handles Camera movement
    /// </summary>
    public class CameraScript : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer initialSR;
        private static CameraScript I;
        private Vector3 dragOrigin;
        private Vector3 targetPosition;
        private float bounds;
        private Camera cam;

        private void Awake()
        {
            I = this;
            targetPosition = transform.position;
            cam = GetComponent<Camera>();
            SetBackground(initialSR);
        }

        void Update()
        {
            if (!Singleton.isUIOverride())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
                }

                if (!Input.GetMouseButton(0)) return;

                Vector3 moveVec = (cam.ScreenToWorldPoint(Input.mousePosition) - dragOrigin);
                targetPosition = GetBoundedPosition(new Vector3(targetPosition.x - moveVec.x, targetPosition.y, targetPosition.z));
                dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        private void FixedUpdate()
        {
            Vector3 change = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f) - transform.position;
            transform.position += change;
            dragOrigin += change;
        }

        /// <summary>
        /// Sets the background target which will lock camera movement to the sprite size.
        /// </summary>
        public void SetBackground(SpriteRenderer sr)
        {
            bounds = (sr.sprite.rect.width / sr.sprite.pixelsPerUnit) / 2f;
            cam.orthographicSize = (sr.sprite.rect.height / sr.sprite.pixelsPerUnit) / 2f;
            transform.position = new Vector3(sr.transform.position.x, sr.transform.position.y, -10);
            targetPosition = new Vector3(sr.transform.position.x, sr.transform.position.y, -10);
        }

        /// <summary>
        /// Moves focus to the transform
        /// </summary>
        /// <remarks>Will not try to move outside background bounds</remarks>
        public static void FocusOn(Transform t)
        {
            I.targetPosition = I.GetBoundedPosition(new Vector3(t.position.x, I.targetPosition.y, -10));
        }

        /// <summary>
        /// Internally calculates bounds of the background
        /// </summary>
        private Vector3 GetBoundedPosition(Vector3 position)
        {
            float camWidth = cam.orthographicSize * cam.aspect;
            return new Vector3(Mathf.Clamp(position.x, camWidth - bounds, bounds - camWidth), position.y, position.z); ;
        }
    }
}
