using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class PowerPickUp : Pickup
    {
        [Header("Parameters")]
        [Tooltip("Amount of power to buff on pickup")]
        public float PowerBuffRatio;

        protected override void OnPicked(PlayerCharacterController player)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth)
            {
                playerHealth.buffMaxHP(PowerBuffRatio);
                PlayPickupFeedback();
                Destroy(gameObject);
            }
        }
    }
}