using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

        [SerializeField]
        private State startState;
        public State StartState
        {
            get { return startState; }
            set { startState = value; }
        }
        private State currentState;

        // Use this for initialization
        void OnEnable()
        {
            currentState = startState;
        }

        public void Execute()
        {
            if (currentState == null)
                return;

            currentState = currentState.Execute();
        }

        public void Reset()
        {
            states.Clear();
            transitions.Clear();
        }

    }
}
