using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.FPS.Game;

namespace Unity.FPS.AI
{
    [System.Serializable]
    public struct SummonEnemyData
    {
        public EnemyController enemy;
        public int staringWave;
        public float weight;
    }

    public class EnemiesSpawner : MonoBehaviour
    {


        [SerializeField]
        private Transform Center;
        [SerializeField]
        private Vector3 Size;
        [SerializeField]
        private List<SummonEnemyData> Enemies;
        //percentage
        [SerializeField]
        private float timeBetweenEnemySpawn;
        [SerializeField]
        private float StartingMaxEnemyWeight;
        [SerializeField]
        private float EnemyWeightMultiplyPerWave;
        [SerializeField]
        private MinMaxFloat SpawnRate;

        private WaveManager waveManager;

        private float maxEnemyWeight;

        public float MaxEnemyWeight
        {
            get
            {
                return maxEnemyWeight;
            }
            set
            {
                maxEnemyWeight = value;
            }
        }

        private float currentEnemyWeight;

        public float CurrentEnemyWeight
        {
            get
            {
                return currentEnemyWeight;
            }
            set
            {
                currentEnemyWeight = value;
            }
        }

        public List<SummonEnemyData> optionalEnemies
        {
            get
            {
                return Enemies.FindAll(x => waveManager.CurrentWave >= x.staringWave
                && CurrentEnemyWeight + x.weight <= MaxEnemyWeight);
            }
        }

        private Transform PlayerTransform;

        void EnemyDied(SummonEnemyData enemy)
        {
            CurrentEnemyWeight -= enemy.weight;
        }
        public EnemyController SpawnEnemy(SummonEnemyData enemy, Vector3 spawnPoint)
        {
            CurrentEnemyWeight += enemy.weight;

            EnemyController newEnemy = Instantiate(enemy.enemy, spawnPoint, Quaternion.identity);

            newEnemy.onDied += () =>
            {
                EnemyDied(enemy);
            };

            return newEnemy;
        }

        public void SummonEnemies(Bounds spawnArea, SummonEnemyData[] enemies)
        {
            foreach (SummonEnemyData enemy in enemies)
            {
                Vector3 spawnPoint = SpawnAreaGenerator.GetSpawnPosition(spawnArea);

                SpawnEnemy(enemy, spawnPoint);
            }
        }

        public SummonEnemyData GetRandomEnemy()
        {
            SummonEnemyData summonEnemyData = optionalEnemies[Random.Range(0, optionalEnemies.Count)];

            return summonEnemyData;
        }

        public SummonEnemyData[] GetRandomEnemies(int maxEnemies)
        {
            List<SummonEnemyData> enemies = new List<SummonEnemyData>();

            for (int i = 0; i < maxEnemies; i++)
            {
                if (optionalEnemies.Count <= 0)
                {
                    continue;
                }

                enemies.Add(GetRandomEnemy());
            }

            return enemies.ToArray();
        }

        public void EnterWave(int wave)
        {
            MaxEnemyWeight = StartingMaxEnemyWeight * (wave * (1 + EnemyWeightMultiplyPerWave));
        }

        float timeToSummonEnemies = 0;

        // Start is called before the first frame update
        void Start()
        {
            CurrentEnemyWeight = 0;
            MaxEnemyWeight = StartingMaxEnemyWeight;

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
            if (Time.time > timeToSummonEnemies)
            {
                timeToSummonEnemies = Time.time + timeBetweenEnemySpawn;

                SummonEnemies(SpawnAreaGenerator.GetRandomSpawnArea(PlayerTransform.position, Size)
                , GetRandomEnemies(Random.Range((int)SpawnRate.Min, (int)SpawnRate.Max)));
            }
        }
    }
}
