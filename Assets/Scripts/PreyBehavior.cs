using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RomanTristan.Lab5
{
    public class PreyBehavior : MonoBehaviour
    {
        [SerializeField] private GameObject Predator;
        [SerializeField] private GameObject Spawn;
        [SerializeField] private GameObject AltSpawn;

        [SerializeField] private GameObject Goal;

        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float rotationSpeed = .01f;

        private Rigidbody rigidbody;

        private FieldOfView fieldOfView;

        private Ray leftR;
        private Ray rightR;

        private Vector3 startingPos;
        private Quaternion startingRot;

        private bool isWandering = false;
        private bool isWalking = false;
        private bool isHunting = false;
        private bool isRotatingLeft = false;
        private bool isRotatingRight = false;

        private Transform closestPredator;
        private Transform closestWall;

        void Start()
        {
            startingPos = transform.position;
            startingRot = transform.rotation;
            rigidbody = GetComponent<Rigidbody>();
            fieldOfView = GetComponent<FieldOfView>();
            rigidbody.freezeRotation = true;

        }
        void FixedUpdate()
        {
            rightR = new Ray(transform.position, Quaternion.AngleAxis(45f, transform.up) * transform.forward);
            leftR = new Ray(transform.position, Quaternion.AngleAxis(-45f, transform.up) * transform.forward);

            // move
            rigidbody.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);

            foreach (Transform target in fieldOfView.visibleTargets)
            {
                if (target.gameObject.tag == "Predator")
                {
                    if (!closestPredator)
                    {
                        closestPredator = target;
                    }
                    else if (Vector3.Distance(target.transform.position, this.transform.position) < Vector3.Distance(closestPredator.transform.position, this.transform.position))
                    {
                        closestPredator = target;
                    }
                }
            }

            if (fieldOfView.visibleTargets.Count == 0)
            {
                closestPredator = null;
            }

            if (closestPredator)
            {
                rigidbody.MoveRotation(rigidbody.rotation * Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, transform.position - closestPredator.transform.position, rotationSpeed, 0)));
            }

            if (Physics.Raycast(leftR, 2f, fieldOfView.obstacleMask) & Physics.Raycast(rightR, 2f, fieldOfView.obstacleMask))
            {
                //rigidbody.MovePosition(transform.position - transform.forward * moveSpeed * Time.deltaTime);
                rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(new Vector3(0, 120, 0) * Time.deltaTime));
                // Stop and rotate right if both detected close
            }
            if (Physics.Raycast(rightR, fieldOfView.viewRadius / 2, fieldOfView.obstacleMask))
            {
                rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(new Vector3(0, -120, 0) * Time.deltaTime));
                // Rotate left if right detected
            }
            if (Physics.Raycast(leftR, fieldOfView.viewRadius / 2, fieldOfView.obstacleMask))
            {
                rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(new Vector3(0, 120, 0) * Time.deltaTime));
                // rotate right if detected left
            } else
            {
                GoToGoal();
            }

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
            if (other.gameObject.tag == "Predator")
            {
                Respawn();
            }

            if (other.gameObject.tag == "Obstacles")
            {
                rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(new Vector3(0, 180, 0)));
                rigidbody.MovePosition(rigidbody.position + transform.forward * moveSpeed * Time.deltaTime);

                //rigidbody.AddForce(-transform.forward * 10000f * Time.deltaTime);

                //transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, transform.right, 1, 0));

            }
        }
        public void Respawn()
        {
            StartCoroutine("RespawnCoroutine");
        }

        public IEnumerator RespawnCoroutine()
        {
            transform.position = new Vector3(100, 100, 100);
            yield return new WaitForSeconds(2);
            transform.rotation = startingRot;
            if(Random.Range(0, 2) != 0)
            {
                transform.position = Spawn.transform.position;
            }
            else
            {
                transform.position = AltSpawn.transform.position;

            }
        }
        public void GoToGoal()
        {
            rigidbody.MoveRotation(Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, Goal.transform.position - transform.position, rotationSpeed, 0)));
        }

        private Vector3 GetRandomDirection()
        {
            return new Vector3(0, UnityEngine.Random.Range(-1f, 1f), 0).normalized;
        }
    }
}
