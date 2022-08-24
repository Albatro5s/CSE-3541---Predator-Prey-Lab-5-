using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomanTristan.Lab5
{
    public class FieldOfView : MonoBehaviour
    {
        public float viewRadius;
        [Range(0,360)]
        public float viewAngle;

        public LayerMask targetMask;
        public LayerMask obstacleMask;

        public List<Transform> visibleTargets = new List<Transform>();
        public List<Transform> visibleObjects = new List<Transform>();

        private void Start()
        {
            StartCoroutine("FindTargetsWithDelay", .2f);
            StartCoroutine("FindObjectsWithDelay", .2f);
        }

        IEnumerator FindTargetsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }

        IEnumerator FindObjectsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleObjects();
            }
        }

        void FindVisibleTargets()
        {
            visibleTargets.Clear();
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            for(int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.position);
                    
                    if(!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask) & dstToTarget > 1.5f)
                    {
                        visibleTargets.Add(target);
                    }
                }
            }
        }

        void FindVisibleObjects()
        {
            visibleObjects.Clear();
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, obstacleMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform obstacle = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (obstacle.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, obstacle.position);

                    if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask) & dstToTarget > 1.5f)
                    {
                        visibleObjects.Add(obstacle);
                    }
                }
            }
        }
            public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

    }
}
