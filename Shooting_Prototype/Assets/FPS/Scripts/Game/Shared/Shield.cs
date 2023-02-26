using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public class Shield : MonoBehaviour
    {
        public float StarterShield;

        public UnityAction<float, GameObject> OnDamaged;
        public UnityAction<float> OnGained;
        public UnityAction OnDestruction;

        float currentShield;

        Health Health;

        public float CurrentShield
        {
            get { return currentShield; }
            set 
            {
                currentShield = value;

                if (currentShield <= 0)
                {
                    currentShield = 0;

                    OnDestruction?.Invoke();
                }
                else if (Health && (currentShield > Health.MaxHealth))
                {
                    currentShield = Health.MaxHealth;
                }
            }
        }
        public bool Invincible { get; set; }

        public bool CanPickup
        {
            get
            {
                if (!Health)
                {
                    return true;
                }

                return currentShield < Health.MaxHealth;
            }
            private set { }
        }

        // Start is called before the first frame update
        void Start()
        {
            Health = GetComponent<Health>();
            if (!Health)
            {
                Health = GetComponentInParent<Health>();
            }

            CurrentShield = StarterShield;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GainShield(float shield)
        {
            float healthBefore = CurrentShield;
            CurrentShield += shield;

            // call OnGained action
            float trueHealAmount = CurrentShield - healthBefore;
            if (trueHealAmount > 0f)
            {
                OnGained?.Invoke(trueHealAmount);
            }
        }

        public void DestroyShield()
        {
            float shieldBefore = CurrentShield;

            CurrentShield = 0f;

            // call OnDamage action
            OnDamaged?.Invoke(shieldBefore, null);
        }

        public void DamageShield(float damage, GameObject damageSource)
        {
            if (Invincible)
                return;
            if (CurrentShield <= 0)
                return;

            float shieldBefore = CurrentShield;
            CurrentShield -= damage;
            CurrentShield = Mathf.Clamp(CurrentShield, 0f, CurrentShield);

            // call OnDamage action
            float trueDamageAmount = shieldBefore - CurrentShield;
            if (trueDamageAmount > 0f)
            {
                OnDamaged?.Invoke(trueDamageAmount, damageSource);
            }
        }
    }
}
