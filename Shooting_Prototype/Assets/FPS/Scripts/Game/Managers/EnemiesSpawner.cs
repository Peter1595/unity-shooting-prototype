using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.FPS.Game
{
    [System.Serializable]
    public struct SummonEnemyData
    {
        public GameObject enemy;
        public float staringWave;
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

        private List<SummonEnemyData> optionalEnemies;

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

        public Vector3 GetHitPosition(Vector3 spawnPosition)
        {
            if (Physics.Raycast(spawnPosition + new Vector3(0, 999999, 0), Vector3.down, out RaycastHit hit, float.MaxValue, -1))
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

        public GameObject SpawnEnemy(GameObject enemy, Vector3 spawnPoint)
        {
            return Instantiate(enemy, spawnPoint, Quaternion.identity);
        }

        public void SummonEnemies(Bounds spawnArea, SummonEnemyData[] enemies)
        {
            foreach (SummonEnemyData enemy in enemies)
            {
                Vector3 spawnPoint = GetSpawnPosition(spawnArea);

                GameObject spawnedEnemy = SpawnEnemy(enemy.enemy, spawnPoint);
            }
        }

        public Bounds GetSpawnArea()
        {
            Vector3 hitPosition = GetHitPosition(Center.position +
                new Vector3(Random.Range(-Size.x / 2, Size.x / 2), 0, Random.Range(-Size.z / 2, Size.z / 2)));

            if (hitPosition == Vector3.zero)
            {
                return GetSpawnArea();
            }

            return new Bounds(hitPosition, new Vector3(10, 0, 10));
        }

        public SummonEnemyData[] GetEnemies()
        {
            return optionalEnemies.FindAll(x => CurrentWave >= x.staringWave).ToArray();
        }

        public void EnterWave(int wave)
        {
        }

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
            if (Time.time > timeToSummonEnemies)
            {
                timeToSummonEnemies = Time.time + timeBetweenEnemySpawn;


            }
        }
    }
}
