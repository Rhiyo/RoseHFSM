using System;
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

        /// <summary>
        /// The game object that is the owner of this behaviour.
        /// </summary>
        public Behaviour Behaviour { get; set; }

        protected GameObject Owner
        {
            get {
                if (Behaviour == null) return null;
                return Behaviour.Owner;
            }
        }

        #region depreciated

        [Obsolete("owner is now decpreciated. Use Owner directly")]
        protected GameObject owner;

        #endregion

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
            return this;
        }

        /// <summary>
        /// Called on state entry.
        /// </summary>
        public virtual void EntryAction()
        {

        }

        /// <summary>
        /// Called on state exit.
        /// </summary>
        protected virtual void ExitAction()
        {

        }
    }
}