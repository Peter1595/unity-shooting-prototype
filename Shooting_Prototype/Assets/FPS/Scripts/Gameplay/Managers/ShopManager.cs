using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{

    public class ShopManager : MonoBehaviour
    {
        private int priceMultiply;
        public int PriceMultiply
        {
            get
            {
                return priceMultiply;
            }
            set
            {
                priceMultiply = value;
            }

        }

        void Start()
        {
            EventManager.AddListener((EnteredNewWaveEvent evt) =>
            {
                MultiplyProductsPrice(evt.Wave);
            });
        }

        private bool Give(PlayerCharacterController playerCharacterController, Pickup pickup)
        {
            pickup.Pick(playerCharacterController);

            return true;
        }

        public bool Buy(ShopController shopController, ShopProduct Product)
        {
            if (!(shopController && Product))
            {
                return false;
            }

            int price = Product.Price;

            bool canBuy = shopController.CanBuy(price);

            if (!canBuy)
            {
                return false;
            }

            Give(shopController.GetComponent<PlayerCharacterController>()
            , Product.Product.GetComponent<Pickup>());

            bool bought = shopController.SpendMoney(price);

            if (!bought)
            {
                return false;
            }

            return true;
        }

        private void MultiplyProductsPrice(int multiply)
        {

            PriceMultiply = multiply;

            ShopProduct[] products = FindObjectsOfType<ShopProduct>();

            foreach (ShopProduct product in products)
            {
                product.Price = product.startingPrice * PriceMultiply;
            }
        }
    }
}
