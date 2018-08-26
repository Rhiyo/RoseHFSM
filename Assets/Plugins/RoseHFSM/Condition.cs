using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoseHFSM
{
    public class Condition : MonoBehaviour
    {        
        public Behaviour Behaviour { get; set; }

              
        public virtual bool Check() {
            return true;
        }
    }
}