using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    [System.Serializable]
    public struct ChestDropData
    {
        public Chest chest;
        public int staringWave;
        public float weight;
    }

    public class ChestSpawningManager : MonoBehaviour
    {
        [SerializeField]
        private Transform Center;
        [SerializeField]
        private Vector3 Size;
        [SerializeField]
        private List<ChestDropData> Chests;
        [SerializeField]
        private float timeBetweenChestDrop;
        [SerializeField]
        private float StartingMaxChestWeight;
        [SerializeField]
        private float ChestWeightMultiplyPerWave;
        [SerializeField]
        private MinMaxFloat SpawnRate;

        private WaveManager waveManager;

        private float maxChestWeight;

        public float MaxChestWeight
        {
            get
            {
                return maxChestWeight;
            }
            set
            {
                maxChestWeight = value;
            }
        }

        private float currentChestWeight;

        public float CurrentChestWeight
        {
            get
            {
                return currentChestWeight;
            }
            set
            {
                currentChestWeight = value;
            }
        }

        public List<ChestDropData> opionalChests
        {
            get
            {
                return Chests.FindAll(x => waveManager.CurrentWave >= x.staringWave
                && CurrentChestWeight + x.weight <= MaxChestWeight);
            }
        }

        private Transform PlayerTransform;

        void ChestOpened(ChestDropData chest)
        {
            CurrentChestWeight -= chest.weight;
        }
        public Chest DropChest(ChestDropData chest, Vector3 spawnPoint)
        {
            CurrentChestWeight += chest.weight;

            Chest newChest = Instantiate(chest.chest, spawnPoint, Quaternion.identity);

            newChest.onOpened += () =>
            {
                ChestOpened(chest);
            };

            return newChest;
        }

        public void DropChests(Bounds spawnArea, ChestDropData[] chests)
        {
            foreach (ChestDropData chest in chests)
            {
                Vector3 spawnPoint = SpawnAreaGenerator.GetSpawnPosition(spawnArea);

                DropChest(chest, spawnPoint);
            }
        }

        public ChestDropData GetRandomEnemy()
        {
            ChestDropData chestDropData = opionalChests[Random.Range(0, opionalChests.Count)];

            return chestDropData;
        }

        public ChestDropData[] GetRandomChests(int maxChests)
        {
            List<ChestDropData> chests = new List<ChestDropData>();

            for (int i = 0; i < maxChests; i++)
            {
                if (opionalChests.Count <= 0)
                {
                    continue;
                }

                chests.Add(GetRandomEnemy());
            }

            return chests.ToArray();
        }

        public void EnterWave(int wave)
        {
            MaxChestWeight = StartingMaxChestWeight * (wave * (1 + ChestWeightMultiplyPerWave));
        }

        float timeToEnterNewWave = 0;

        float timeToDropChests = 0;

        // Start is called before the first frame update
        void Start()
        {
            CurrentChestWeight = 0;
            MaxChestWeight = StartingMaxChestWeight;

            PlayerTransformEventReturnData playerTransformEventReturnData =
            ReturnableEventManager.Broadcast<PlayerTransformEventReturnData>
            (Events.PlayerTransformEvent);

            EventManager.AddListener((EnteredNewWaveEvent enteredNewWaveEvent) =>
            {
                EnterWave(enteredNewWaveEvent.Wave);
            });

            waveManager = FindObjectOfType<WaveManager>();

            PlayerTransform = playerTransformEventReturnData.Transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > timeToDropChests)
            {
                timeToDropChests = Time.time + timeBetweenChestDrop;

                DropChests(SpawnAreaGenerator.GetRandomSpawnArea(PlayerTransform.position, Size),
                GetRandomChests(Random.Range((int)SpawnRate.Min, (int)SpawnRate.Max)));
            }
        }
    }
}