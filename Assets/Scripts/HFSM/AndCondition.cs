using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoseHFSM
{
    public class AndCondition : Condition
    {
        [SerializeField]
        public Condition a, b;    
        public override bool Check() {
            return a.Check() && b.Check();
        }

        void OnDestroy()
        {
            if (a)
                DestroyImmediate(a);

            if (b)
                DestroyImmediate(b);    
        }
    }
}