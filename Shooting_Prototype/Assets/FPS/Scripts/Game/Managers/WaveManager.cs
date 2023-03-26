using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    public class WaveManager : MonoBehaviour
    {

        [SerializeField]
        private float timeBetweenWaves;

        private int currentWave = 0;

        public int CurrentWave
        {
            get
            {
                return currentWave;
            }
            set
            {
                currentWave = value;
            }
        }

        public void EnterWave(int wave)
        {
            CurrentWave = wave;

            EnteredNewWaveEvent evt = Events.EnteredNewWaveEvent;
            evt.Wave = wave;

            EventManager.Broadcast(evt);
        }

        float timeToEnterNewWave = 0;


        // Start is called before the first frame update
        void Start()
        {

            CurrentWave = 0;


        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > timeToEnterNewWave)
            {
                timeToEnterNewWave = Time.time + timeBetweenWaves;

                EnterWave(CurrentWave + 1);
            }

        }
    }

}