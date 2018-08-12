using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoseHFSM
{
    public class Condition : MonoBehaviour
    {
        [SerializeField]
        protected GameObject owner;

        protected void Start()
        {
            owner = GetComponent<Behaviour>().Owner;
        }
              
        public virtual bool Check() {
            return true;
        }
    }
}