using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RoseHFSM
{
    public class Behaviour : MonoBehaviour
    {

        private HFSM topHFSM;
        public HFSM TopHFSM
        {
            get { return topHFSM; }
            set { topHFSM = value; }
        }
        // Use this for initialization
        void Start()
        {
            if (topHFSM == null)
            {
                Debug.LogError("No starting HFSM.");
                return;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (topHFSM == null)
                return;

            topHFSM.Execute();
        }

    }
}