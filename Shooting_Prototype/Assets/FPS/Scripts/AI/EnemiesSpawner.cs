using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        private float timeBetweenWaves;
        [SerializeField]
        private float timeBetweenEnemySpawn;
        [SerializeField]
        private float StartingMaxEnemyWeight;

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
                return Enemies.FindAll(x => CurrentWave >= x.staringWave
                && CurrentEnemyWeight + x.weight <= MaxEnemyWeight);
            }
        }

        public Vector3 GetHitPosition(Vector3 spawnPosition)
        {
            if (Physics.Raycast(spawnPosition + new Vector3(0, 999999, 0), Vector3.down, out RaycastHit hit, float.MaxValue))
            {
                return hit.point;
            }

            return Vector3.zero;
        }

        public Vector3 GetSpawnPosition(Bounds area)
        {
            Vector3 hitPosition = GetHitPosition(area.center +
                new Vector3(Random.Range(-area.size.x / 2, area.size.x / 2), 0, Random.Range(-area.size.z / 2, area.size.z / 2)));

            if (hitPosition == Vector3.zero)
            {
                return GetSpawnPosition(area);
            }

            return hitPosition;
        }

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
                Vector3 spawnPoint = GetSpawnPosition(spawnArea);

                SpawnEnemy(enemy, spawnPoint);
            }
        }

        public Bounds GetRandomSpawnArea()
        {
            Vector3 hitPosition = GetHitPosition(Center.position +
                new Vector3(Random.Range(-Size.x / 2, Size.x / 2), 0, Random.Range(-Size.z / 2, Size.z / 2)));

            if (hitPosition == Vector3.zero)
            {
                return GetRandomSpawnArea();
            }

            return new Bounds(hitPosition, new Vector3(10, 0, 10));
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
            CurrentWave = wave;
            MaxEnemyWeight *= 1.25f;
        }

        float timeToEnterNewWave = 0;

        float timeToSummonEnemies = 0;

        // Start is called before the first frame update
        void Start()
        {
            CurrentWave = 0;
            CurrentEnemyWeight = 0;
            MaxEnemyWeight = StartingMaxEnemyWeight;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > timeToEnterNewWave)
            {
                timeToEnterNewWave = Time.time + timeBetweenWaves;

                EnterWave(CurrentWave + 1);
            }

            if (Time.time > timeToSummonEnemies)
            {
                timeToSummonEnemies = Time.time + timeBetweenEnemySpawn;

                SummonEnemies(GetRandomSpawnArea(), GetRandomEnemies(Random.Range(2, 4)));
            }
        }
    }
}
