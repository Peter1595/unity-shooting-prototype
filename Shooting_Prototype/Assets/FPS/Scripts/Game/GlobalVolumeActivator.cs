using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    public class GlobalVolumeActivator : MonoBehaviour
    {
        public GameObject globalVolume;

        // Start is called before the first frame update
        void Start()
        {
            globalVolume.SetActive(false);

            Debug.Log("Activating...");

            FindFirstObjectByType<DelayEventManager>().Wait(5f, () =>
            {
                globalVolume.SetActive(true);
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
