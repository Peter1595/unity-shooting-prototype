using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class JumpPickUp : Pickup
    {
        [Header("Parameters")]
        [Tooltip("Amount of jump power to buff on pickup")]
        public float JumpBuffRatio;

        protected override void OnPicked(PlayerCharacterController player)
        {
            PlayerCharacterController ployerController = player.GetComponent<PlayerCharacterController>();
            if (ployerController)
            {
                ployerController.buffJump(JumpBuffRatio);
                PlayPickupFeedback();
                Destroy(gameObject);
            }
        }
    }
}