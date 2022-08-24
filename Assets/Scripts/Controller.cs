using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomanTristan.Lab5
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float sensitivity = 24f;

        Rigidbody rigidbody;
        Camera viewCamera;
        Vector3 velocity;

        float mouseX, mouseY;
        float xRotation = 0f;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            viewCamera = Camera.main;
        }

        void Update()
        {
            xRotation += mouseX + mouseY;
            transform.localRotation = Quaternion.Euler(0f, xRotation, 0f);
        }

        public void Input(Vector2 mouseInput)
        {
            mouseX = mouseInput.x * sensitivity * Time.deltaTime;
            mouseY = mouseInput.y * sensitivity * Time.deltaTime;
        }
    }
}
