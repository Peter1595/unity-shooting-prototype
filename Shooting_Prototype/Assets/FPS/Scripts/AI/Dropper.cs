using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.AI
{
    [System.Serializable]
    public struct DropData
    {
        public GameObject Prefab;

        public float Weight;
    }

    [RequireComponent(typeof(EnemyController))]
    public class Dropper : MonoBehaviour
    {
        private EnemyController enemyController;


        public List<DropData> Drops;

        public List<DropData> optionalDrops
        {
            get
            {
                return Drops.FindAll(x => currentWeight + x.Weight <= MaxWeight);
            }
        }


        public float startingMaxWeight;
        private float maxWeight;
        public float MaxWeight
        {
            get
            {
                return maxWeight;
            }
            set
            {
                maxWeight = value;
            }
        }

        private float currentWeight;

        public float CurrentWeight
        {
            get
            {
                return currentWeight;
            }
            set
            {
                currentWeight = value;
            }
        }



        public DropData PickRandomDrop()
        {
            if (optionalDrops.Count <= 0)
            {
                return default;
            }

            return optionalDrops[Random.Range(0, optionalDrops.Count)];
        }

        public void PlaceDrops()
        {
            while (optionalDrops.Count > 0)
            {
                DropData dropData = PickRandomDrop();

                Instantiate(dropData.Prefab, enemyController.transform.position, Quaternion.identity);

                CurrentWeight += dropData.Weight;
            }
        }

        public void OnDie()
        {
            PlaceDrops();

        }
        // Start is called before the first frame update
        void Start()
        {
            MaxWeight = startingMaxWeight;


            enemyController = GetComponent<EnemyController>();


            enemyController.onDied += OnDie;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
