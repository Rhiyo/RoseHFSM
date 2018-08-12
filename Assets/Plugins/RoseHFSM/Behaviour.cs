using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RoseHFSM
{
    public class Behaviour : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private HFSM topHFSM;
        public HFSM TopHFSM
        {
            get { return topHFSM; }
            set { topHFSM = value; }
        }

        [SerializeField]
        private GameObject owner;
        public GameObject Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public Dictionary<string, int> intFlags = new Dictionary<string, int>();

        public Dictionary<string, Vector3> vec3Flags = new Dictionary<string, Vector3>();

        public HashSet<string> flags = new HashSet<string>();

        private float timer;
        public float Timer
        {
            get { return timer; }
        }

        private int randomSelection;
        public int RandomSelection
        {
            get { return randomSelection; }
        }

        private bool paused;
        public bool Paused
        {
            set { paused = value; }
            get { return paused; }
        }

        private float timerMin = -1;

        #region[Unity Events]
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

            if(timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                    timer = 0;
            }

            topHFSM.Execute();
        }
        #endregion


        #region[Flag Methods]
        /// <summary>
        /// Set timer.
        /// </summary>
        /// <param name="timer"></param>
        public void SetTimer(float timer)
        {
            if (timerMin == -1)
                this.timer = timer;
            else
            {
                this.timer = Random.Range(timerMin, timer);
                print(this.timer + "TIME");
            }
        }

        /// <summary>
        /// Set timer between a random range.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void SetTimer(float from, float to)
        {
            timer = Random.Range(from, to);
        }

        public void SetTimerMin(float min)
        {
            timerMin = min;
        }

        /// <summary>
        /// Sets the random selection between 0 and upto but not including range.
        /// </summary>
        /// <param name="range"></param>
        public void GenerateRandomSelection(int range)
        {
            randomSelection = Random.Range(0, range);
            print(randomSelection + " as random selection.");
        }

#endregion

        public void ResetToStart()
        {
            topHFSM.ResetToStartState();
        }
    }
}