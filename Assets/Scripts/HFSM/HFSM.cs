using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 /// <summary>
 /// If transitions directly from this HFSM to another,
 /// resets currentState to start.
 /// </summary>
namespace RoseHFSM {
    [System.Serializable]
    public class HFSM : ScriptableObject
    {
        public string hfsmName = "New HFSM";

        [SerializeField]
        private List<State> states = new List<State>();
        public List<State> States{
               get { return states; }
        }

        [SerializeField]
        private List<Transition> transitions = new List<Transition>();
        public List<Transition> Transitions
        {
            get { return transitions; }
        }

        /*// <summary>
        /// Keeping track of transitionsd that come in from outside.
        /// </summary>
        [SerializeField]
        private Dictionary<Transition, State> outsideTransitions = new Dictionary<Transition, State>();
        public Dictionary<Transition, State> OutsideTransitions
        {
            get { return outsideTransitions; }
        }*/

        [SerializeField]
        private State startState;
        public State StartState
        {
            get { return startState; }
            set { startState = value; }
        }
        private State currentState;
        public State CurrentState
        {
            get { return currentState; }
        }

        // Use this for initialization
        void OnEnable()
        {
            currentState = startState;
        }

        public State Execute()
        {
            if (currentState == null)
                return null;

            State nextState = currentState.Execute();
            if (states.Contains(nextState))
                currentState = nextState;

            return nextState;
        }

        /// <summary>
        /// Clears the HFSM. Use this if there are any errors.
        /// </summary>
        public void Clear()
        {
            states.Clear();
            transitions.Clear();
        }

        public void ResetToStartState()
        {
            currentState = startState;
        }

    }
}
