using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoseHFSM
{
    public class Move : State {

        [SerializeField]
        private Transform movingObject;

        [SerializeField]
        private Transform target;

        [SerializeField]
        private float speed = 2;
        
        void Start() {

        }

        protected override State ContinuousAction()
        {
            if (movingObject == null)
            {
                Debug.LogError("No object set.");
                return null;
            }
            if (target == null)
            {
                Debug.LogError("No target set.");
                return null;
            }
            movingObject.position = Vector3.MoveTowards(movingObject.transform.position, target.position, Time.deltaTime * speed);
            return this;
        }
    }
}
