using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoseHFSM
{
    public class Condition : ScriptableObject
    {
              
        public virtual bool Check() {
            return true;
        }
    }
}