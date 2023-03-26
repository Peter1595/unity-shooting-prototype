using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{

    public class ShopProduct : MonoBehaviour
    {
        private ShopManager m_ShopManager;

        public GameObject Product;
        public int startingPrice;

        private int price;
        public int Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;

                UpdatePrice();
            }
        }
        public ShootableButton shootableButton;
        public Text priceText;

        void Start()
        {

            Price = startingPrice;

            m_ShopManager = FindFirstObjectByType<ShopManager>();

            if (shootableButton)
            {
                shootableButton.OnClick += Buy;
            }

            UpdatePrice();
        }
        private void UpdatePrice()
        {
            int price = Price;

            priceText.text = "$" + NumberConverter.FixNumber(price);
        }

        public void Buy(GameObject Shooter)
        {

            ShopController shopController = Shooter.GetComponent<ShopController>();

            Debug.Log("Buying..." + shopController);

            m_ShopManager.Buy(shopController, this);
        }
    }
}
