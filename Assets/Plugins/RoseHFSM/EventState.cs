using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoseHFSM {
    public class EventState : State
    {

        [SerializeField]
        protected UnityEvent continuousAction;

        [SerializeField]
        protected UnityEvent entryAction;

        [SerializeField]
        protected UnityEvent exitAction;

        public override void EntryAction()
        {
            entryAction.Invoke();
        }

        protected override State ContinuousAction()
        {
            continuousAction.Invoke();
            return this;
        }

        protected override void ExitAction()
        {
            exitAction.Invoke();
        }
    }
}

