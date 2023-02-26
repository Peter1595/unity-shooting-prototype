using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ShieldPickUp : Pickup
    {
        [Header("Parameters")]
        [Tooltip("Amount of shield to gain on pickup")]
        public float GainAmount;

        protected override void OnPicked(PlayerCharacterController player)
        {
            Shield playerShield = player.GetComponent<Shield>();
            if (playerShield && playerShield.CanPickup)
            {
                Debug.Log("Gainning Shield: " + GainAmount);
                Debug.Log("From: " + playerShield.CurrentShield);

                playerShield.GainShield(GainAmount);
                PlayPickupFeedback();
                Destroy(gameObject);

                Debug.Log("To: " + playerShield.CurrentShield);
            }
        }
    }
}