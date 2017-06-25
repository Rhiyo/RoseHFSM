using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RoseHFSM
{
    public class DistanceCondition : Condition
    {

        [SerializeField]
        private Transform a, b;

        [SerializeField]
        private float distanceSquared;

        public override bool Check()
        {
            if (Mathf.Approximately(Vector3.SqrMagnitude(b.position - a.position), 0))
                return true;


            if (Vector3.SqrMagnitude(b.position - a.position) <= distanceSquared)
                return true;

            
            return false;
        }
     
    }

}