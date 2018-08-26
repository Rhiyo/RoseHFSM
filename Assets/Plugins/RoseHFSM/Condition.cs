using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoseHFSM
{
    public class Condition : MonoBehaviour
    {
        public GameObject Owner { get; set; }

              
        public virtual bool Check() {
            return true;
        }
    }
}