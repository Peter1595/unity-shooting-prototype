using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    public class Chest : MonoBehaviour
    {
        [Tooltip("Objects will destroy when the chest is open")]
        public GameObject UpperChest;
        public GameObject HealthPivot;

        [Tooltip("The Rarity level of the loot would spawn from the Chest")]
        public int Level;

        Health Health;

        LootManager lootManager;

        bool isOpenned = false;

        public UnityAction onOpened;

        // Start is called before the first frame update
        void Start()
        {
            Health = GetComponent<Health>();

            lootManager = FindFirstObjectByType<LootManager>();

            if (!Health)
            {
                return;
            }

            Health.OnDie += onOpen;
        }


        void onOpen()
        {
            Debug.Log(isOpenned);

            if (isOpenned)
            {
                return;
            }

            isOpenned = true;

            Destroy(UpperChest);
            Destroy(HealthPivot);

            LootLevel lootLevel = lootManager.ChooseLevel(Level);

            GameObject buff = lootManager.ChooseBuff(lootLevel);

            Debug.Log(buff.name + " - " + lootLevel.levelName);

            if (buff == null)
            {
                return;
            }

            GameObject newBuff = Instantiate(buff, gameObject.transform.position, Quaternion.identity);

            newBuff.name = "chest buff";

            Debug.Log("buff - " + newBuff.name);

            onOpened?.Invoke();
        }
    }
}