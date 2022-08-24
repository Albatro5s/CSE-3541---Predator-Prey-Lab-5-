using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace RomanTristan.Lab5
{
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 6;
        [SerializeField] private float sensitivity = 8f;

        Rigidbody rigidbody;
        Camera viewCamera;
        float velocity;

        float mouseX, mouseY;
        float xRotation = 0f;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            viewCamera = Camera.main;
        }

        private void Update()
        {
            xRotation += mouseX;
            //xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);

            transform.localRotation = Quaternion.Euler(0f, xRotation, 0f);
            //viewCamera.Rotate(Vector3.up * mouseX);
        }
        public void Input(Vector2 mouseInput)
        {
            mouseX = mouseInput.x * sensitivity * Time.deltaTime;
            mouseY = mouseInput.y * sensitivity * Time.deltaTime;
        }
    }
}

