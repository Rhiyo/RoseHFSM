using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoseHFSM
{
    public class NotCondition : Condition
    {
        [SerializeField]
        public Condition notThis;      
        public override bool Check() {
            return !notThis.Check();
        }

        void OnDestroy()
        {
            if (notThis)
                Destroy(notThis);
        } 
    }
}