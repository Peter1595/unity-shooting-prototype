using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Gameplay
{

    public class ShopController : MonoBehaviour
    {

        private CoinsStat Coins;
        private int Money
        {
            get
            {
                return Coins.get();
            }
            set
            {
                Coins.set(value);
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            Coins = GetComponent<CoinsStat>();
            if (!Coins)
            {
                Coins = GetComponentInChildren<CoinsStat>();
            }
            if (!Coins)
            {
                Coins = GetComponentInParent<CoinsStat>();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        public bool CanBuy(int money)
        {

            if (Money - money <= 0)
            {
                return false;
            }


            return true;
        }
        public bool SpendMoney(int money)
        {
            if (Money - money <= 0)
            {
                return false;
            }

            Money -= money;



            return true;
        }
    }
}
