using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RoseHFSM
{
    public class Transition : ScriptableObject
    {
        [SerializeField]
        private State toState;
        public State ToState
        {
            get { return toState; }
            set { toState = value;}
        }

        [SerializeField]
        private List<Condition> conditions = new List<Condition>();
        public List<Condition> Conditions
        {
            get { return conditions; }
            set { conditions = value; }
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public State CheckConditions()
        {
            if(conditions != null)
                foreach (Condition c in conditions)
                {
                    if (c.Check() == false)
                        return null;
                }
            return toState;
        }
    }
}