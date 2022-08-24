using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomanTristan.Lab5
{
    public class MovingWallController : MonoBehaviour
    {
        public int moveSpeed;

        private Vector3 startingPos;

        private bool directionUp = true;

        private void Start()
        {
            startingPos = transform.position;
        }

        void Update()
        {
            if(transform.position.z >= startingPos.z + 11f)
            {
                directionUp = false;
            }
            if (transform.position.z <= startingPos.z - 11f)
            {
                directionUp = true;
            }

            if (directionUp)
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
            else
            {
                transform.position -= transform.forward * moveSpeed * Time.deltaTime;
            }
        }
    }
}
