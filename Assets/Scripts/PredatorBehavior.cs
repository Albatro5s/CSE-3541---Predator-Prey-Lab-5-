using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RomanTristan.Lab5
{
    public class PredatorBehavior : MonoBehaviour
    {
        [SerializeField] private GameObject Prey;

        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float rotationSpeed = .01f;

        private Rigidbody rigidbody;

        private FieldOfView fieldOfView;

        private Ray leftR;
        private Ray rightR;

        private Vector3 startingPos;
        private Vector3 roamingPos;

        private bool isWandering = false;
        private bool isWalking = false;
        private bool isHunting = false;
        private bool isRotatingLeft = false;
        private bool isRotatingRight = false;

        private Transform closestPrey;
        private Transform closestWall;

        void Start()
        {
            startingPos = transform.position;
            rigidbody = GetComponent<Rigidbody>();
            fieldOfView = GetComponent<FieldOfView>();
            rigidbody.freezeRotation = true;
        }
        void FixedUpdate()
        {
            rightR = new Ray(transform.position, Quaternion.AngleAxis(45f, transform.up) * transform.forward);
            leftR = new Ray(transform.position, Quaternion.AngleAxis(-45f, transform.up) * transform.forward);

            // move
            rigidbody.MovePosition(rigidbody.position + transform.forward * moveSpeed * Time.deltaTime);

            /*isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded)
            {
                velocityY.y = 0;
            }

            velocityY.y += gravity * Time.deltaTime;*/

            foreach (Transform target in fieldOfView.visibleTargets)
            {
                if (target.gameObject.tag == "Prey")
                {
                    if (!closestPrey)
                    {
                        closestPrey = target;
                    }
                    else if (Vector3.Distance(target.transform.position, this.transform.position) < Vector3.Distance(closestPrey.transform.position, this.transform.position))
                    {
                        closestPrey = target;
                    }
                }
            }

            if(fieldOfView.visibleTargets.Count == 0)
            {
                closestPrey = null;
            }

            if (closestPrey)
            {
                rigidbody.MoveRotation(Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, closestPrey.transform.position - transform.position, rotationSpeed, 0)));
                //transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, closestPrey.transform.position-transform.position, rotationSpeed, 0));
            }
            else
            {
                if (Physics.Raycast(leftR, 2f, fieldOfView.obstacleMask) & Physics.Raycast(rightR, 2f, fieldOfView.obstacleMask)) // Stop and rotate right if both detected close
                {
                    //rigidbody.MovePosition(transform.position - transform.forward * moveSpeed * Time.deltaTime);
                    rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(new Vector3(0, 120, 0) * Time.deltaTime));
                }
                if (Physics.Raycast(rightR, fieldOfView.viewRadius/2, fieldOfView.obstacleMask))                     // Rotate left if right detected
                {
                    rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(new Vector3(0, -120, 0) * Time.deltaTime));
                }
                if (Physics.Raycast(leftR, fieldOfView.viewRadius/2, fieldOfView.obstacleMask))                    // rotate right if detected left
                {
                    rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(new Vector3(0, 120, 0) * Time.deltaTime));
                }
            }

            /*
            ray = new Ray(transform.position + Vector3.up, transform.forward);
            Vector3 posInicial = transform.position;

            if (Physics.Raycast(ray, out hit, 55f)) // Front sensor
            {
                if (hit.collider.tag == ("Pick Up")) // If robot detects pick up, it goes towards it
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                    transform.position = Vector3.MoveTowards(transform.position, hit.point, Time.deltaTime * speed);
                }
                else
                {
                    transform.Rotate(0, -80 * Time.deltaTime, 0); // Rotate if front sensor doesn't detect pick up
                    Debug.DrawLine(ray.origin, hit.point, Color.blue);
                }
            }
            else
            {
                transform.position += transform.forward * speed * Time.deltaTime; // Go forward
            }
            */
            


            /*foreach (Transform target in fieldOfView.visibleObjects)
            {
                if (!closestWall)
                {
                    closestWall = target;
                }
                else if (Vector3.Distance(target.transform.position, this.transform.position) < Vector3.Distance(closestWall.transform.position, this.transform.position))
                {
                    closestWall = target;
                }
            }
            if (fieldOfView.visibleObjects.Count == 0)
            {
                closestWall = null;
            }

            if (closestWall)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, transform.position - closestWall.transform.position, rotationSpeed, 0));
            }*/


        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Obstacles")
            {
                rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(new Vector3(0, 180, 0)));
                rigidbody.MovePosition(rigidbody.position + transform.forward * moveSpeed * Time.deltaTime);

                //rigidbody.AddForce(-transform.forward * 10000f * Time.deltaTime);

                //transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -transform.forward, 1, 0));

            }

        }

        private Vector3 GetRandomDirection()
        {
            return new Vector3(0, UnityEngine.Random.Range(-1f, 1f), 0).normalized;
        }
    }
}

