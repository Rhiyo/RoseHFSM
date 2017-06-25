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

        protected override void RunState()
        {
            if (movingObject == null)
            {
                Debug.LogError("No object set.");
                return;
            }
            if (target == null)
            {
                Debug.LogError("No target set.");
                return;
            }

            movingObject.position = Vector3.MoveTowards(movingObject.transform.position, target.position, Time.deltaTime * speed);
        }
    }
}
