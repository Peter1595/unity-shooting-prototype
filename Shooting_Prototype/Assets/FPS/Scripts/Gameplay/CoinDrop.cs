using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class CoinDrop : Drop
    {
        public int coinAmout;
        protected override void OnClaim(PlayerCharacterController player)
        {
            CoinsStat coinsStat = player.GetComponent<CoinsStat>();

            if (coinsStat)
            {

                base.OnClaim(player);

                coinsStat.set(coinsStat.get() + coinAmout);

                Destroy(gameObject);
            }
        }
    }

}