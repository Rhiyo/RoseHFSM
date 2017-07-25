using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoseHFSM
{
    public class Condition : MonoBehaviour
    {
        [SerializeField]
        private GameObject owner;
        public GameObject Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        
              
        public virtual bool Check() {
            return true;
        }
    }
}