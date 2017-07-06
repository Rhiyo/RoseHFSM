using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoseHFSM
{
    /// <summary>
    /// This transitions to praent states only.
    /// Allows for more options to interface with parent states.
    /// </summary>
    public class ParentTransition : Transition
    {
        public enum ToStateType { CurrentState, DefaultStartState }

        [SerializeField]
        private ToStateType entryState = ToStateType.CurrentState;
        public ToStateType EntryState
        {
            get { return EntryState; }
            set { entryState = value; }
        }

        protected override State GetToState()
        {
            if (!(toState is ParentState))
            {
                Debug.LogError("ParentState required");
            }
            else if (entryState == ToStateType.DefaultStartState)
            {
                ParentState parentState = (ParentState)toState;

                parentState.StateHFSM.ResetToStartState();
            }

            return toState;

        }
    }
}
