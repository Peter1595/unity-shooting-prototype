using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    [System.Serializable]
    public struct DamageStage
    {
        public float duration;

        public float redius;

        public float damagePerSecond;

        public DamageStage(float _duration, float _damagePerSecond, float _redius)
        {
            duration = _duration;
            damagePerSecond = _damagePerSecond;
            redius = _redius;
        }
    }

    public class RamPageParammeters : MonoBehaviour
    {
        public DamageStage[] DamageStages;

        DamageStage currentDamageStage;

        int currentStageIndex = 0;

        float lastStageSwitchTime = 0;

        public int CurrentStageIndex
        {
            get
            {
                return currentStageIndex + 1;
            }
        }

        public int MaxStage
        {
            get
            {
                return DamageStages.Length;
            }
        }
        DamageArea damageArea;

        [HideInInspector]
        public float Damage;
        [HideInInspector]
        public bool CanRamPage;

        bool AtLastStage()
        {
            return currentStageIndex >= DamageStages.Length - 1;
        }

        void SwitchStage(int stage)
        {
            lastStageSwitchTime = Time.time;

            currentDamageStage = DamageStages[stage];

            damageArea.AreaOfEffectDistance = currentDamageStage.redius;

            Damage = currentDamageStage.damagePerSecond;
        }

        // Start is called before the first frame update
        void OnEnable()
        {
            damageArea = GetComponent<DamageArea>();

            SwitchStage(currentStageIndex);
        }

        // Update is called once per frame
        void Update()
        {
            if (!CanRamPage)
            {
                currentStageIndex = 0;

                SwitchStage(currentStageIndex);

                return;
            }

            if (lastStageSwitchTime + currentDamageStage.duration < Time.time && !AtLastStage())
            {
                currentStageIndex++;

                SwitchStage(currentStageIndex);
            }
        }
    }
}
