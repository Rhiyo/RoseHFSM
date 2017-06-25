using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace RoseHFSM
{
    [System.Serializable]
    public class State : ScriptableObject
    {

#if (UNITY_EDITOR)
        public Vector2 nodeEditorLoc;
#endif

        
        [NodeField]
        [SerializeField]
        protected string stateName = "New State";
        public virtual string StateName
        {
            get { return stateName; }
            set { stateName = value; }
        }

        [NodeField]
        [SerializeField]
        public UnityEvent task;

        [SerializeField]
        private List<Transition> transitions = new List<Transition>();
        public List<Transition> Transitions
        {
            get { return transitions; }
        }

        public State Execute()
        {
            if(transitions != null)
                foreach (Transition t in transitions)
                {
                    State toState = t.CheckConditions();
                    if (toState != null)
                    {
                        return toState;
                    }
                }

            RunState();

            return this;
        }

        protected virtual void RunState()
        {
            task.Invoke();
        }
    }
}