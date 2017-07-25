using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace RoseHFSM
{
    public class State : MonoBehaviour
    {

#if (UNITY_EDITOR)
        public Vector2 nodeEditorLoc;
#endif

        [SerializeField]
        protected string stateName = "New State";
        public virtual string StateName
        {
            get { return stateName; }
            set { stateName = value; }
        }

        protected GameObject owner;
        public GameObject Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        
        [SerializeField]
        protected UnityEvent continuousAction;

        [SerializeField]
        protected UnityEvent entryAction;

        [SerializeField]
        protected UnityEvent exitAction;

        [SerializeField]
        private List<Transition> transitions = new List<Transition>();
        public List<Transition> Transitions
        {
            get { return transitions; }
        }

        public State Execute()
        {
            if (transitions != null && transitions.Count != 0)
                foreach (Transition t in transitions)
                {
                    State toState = t.CheckConditions();
                    if (toState != null)
                    {
                        ExitAction();
                        t.TransitionAction();
                        toState.EntryAction();
                        return toState;       
                    }
                }

            return ContinuousAction();
        }

        /// <summary>
        /// Called while state is running.
        /// </summary>
        protected virtual State ContinuousAction()
        {
            continuousAction.Invoke();

            return this;
        }

        /// <summary>
        /// Called on state entry.
        /// </summary>
        public virtual void EntryAction()
        {
            entryAction.Invoke();
        }

        /// <summary>
        /// Called on state exit.
        /// </summary>
        protected virtual void ExitAction()
        {
            exitAction.Invoke();
        }
    }
}