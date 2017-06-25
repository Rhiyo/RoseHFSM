using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoseHFSM
{
    public class Move : MonoBehaviour {

        
        void Start() {

        }

        public void MoveTowards(Transform goal)
        {
            transform.position = Vector3.MoveTowards(transform.position, goal.position, Time.deltaTime * 2);
        }
    }
}
