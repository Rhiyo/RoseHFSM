using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace RoseHFSM
{
    public class Transition : ScriptableObject
    {
        [SerializeField]
        protected State toState;
        public State ToState
        {
            get { return toState; }
            set { toState = value;}
        }    

        [SerializeField]
        protected UnityEvent transitionAction;

        [SerializeField]
        private List<Condition> conditions = new List<Condition>();
        public List<Condition> Conditions
        {
            get { return conditions; }
            set { conditions = value; }
        }

        /// <summary>
        /// Called when a transition is successful.
        /// </summary>
        public virtual void TransitionAction()
        {
            transitionAction.Invoke();
        }

        /// <summary>
        /// Check if conditions are successful.
        /// </summary>
        /// <returns>The state to transition to or null if unsuccesful</returns>
        public State CheckConditions()
        {
            if(conditions != null)
                foreach (Condition c in conditions)
                {
                    if (c.Check() == false)
                        return null;
                }
            return GetToState();
        }

        protected virtual State GetToState()
        {
            return toState;
        }
    }
}