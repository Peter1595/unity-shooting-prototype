using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class PlayerShieldBar : MonoBehaviour
    {
        [Tooltip("Image component dispplaying current health")]
        public Image ShieldFillImage;

        Health m_PlayerHealth;

        Shield m_PlayerShield;

        float currentVelocity = 0;

        void Start()
        {
            PlayerCharacterController playerCharacterController =
                GameObject.FindObjectOfType<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, PlayerShieldBar>(
                playerCharacterController, this);

            m_PlayerHealth = playerCharacterController.GetComponent<Health>();
            //DebugUtility.HandleErrorIfNullGetComponent<Health, PlayerShieldBar>(m_PlayerHealth, this,
              //  playerCharacterController.gameObject);

            m_PlayerShield = playerCharacterController.GetComponent<Shield>();
            //DebugUtility.HandleErrorIfNullGetComponent<Shield, PlayerShieldBar>(m_PlayerShield, this,
              //  playerCharacterController.gameObject);
        }

        void Update()
        {
            // update health bar value
            ShieldFillImage.fillAmount = Mathf.SmoothDamp(ShieldFillImage.fillAmount, m_PlayerShield.CurrentShield / m_PlayerHealth.MaxHealth, ref currentVelocity, 10 * Time.deltaTime);
        }
    }
}