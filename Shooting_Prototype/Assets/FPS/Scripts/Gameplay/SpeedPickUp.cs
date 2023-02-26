using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class SpeedPickUp : Pickup
    {
        [Header("Parameters")]
        [Tooltip("Amount of speed to buff on pickup")]
        public float SpeedBuffRatio;

        protected override void OnPicked(PlayerCharacterController player)
        {
            PlayerCharacterController ployerController = player.GetComponent<PlayerCharacterController>();
            if (ployerController)
            {
                ployerController.buffSpeed(SpeedBuffRatio);
                PlayPickupFeedback();
                Destroy(gameObject);
            }
        }
    }
}