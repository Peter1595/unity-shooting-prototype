using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class LaserProjectile : ProjectileBase
    {
        public float MaxRange;

        public GameObject laserBeam;

        public Transform laserBeamBall;

        [Tooltip("Layers this projectile can collide with")]
        public LayerMask HittableLayers = -1;

        const QueryTriggerInteraction k_TriggerInteraction = QueryTriggerInteraction.Collide;

        DamageArea damageArea;

        ProjectileBase m_ProjectileBase;

        RamPageParammeters ramPage;

        GameObject GetOwner()
        {
            UnityEngine.Debug.Log("Projectile base: " + m_ProjectileBase);

            return m_ProjectileBase.GetOwner();
        }

        // Start is called before the first frame update
        void OnEnable()
        {
            m_ProjectileBase = GetComponent<ProjectileBase>();
            DebugUtility.HandleErrorIfNullGetComponent<ProjectileBase, ProjectileStandard>(m_ProjectileBase, this,
                gameObject);

            ramPage = GetComponent<RamPageParammeters>();

            damageArea = GetComponent<DamageArea>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!laserBeam.active)
            {
                ramPage.CanRamPage = false;

                return;
            }

            Debug.Log("Ram page damage: " + ramPage.Damage);

            Damageable[] damaged =
                damageArea.InflictDamageInArea(ramPage.Damage * Time.deltaTime
                , laserBeamBall.position, HittableLayers, k_TriggerInteraction, GetOwner());

            if (damaged.Length <= 0)
            {
                ramPage.CanRamPage = false;

                return;
            }

            ramPage.CanRamPage = true;

        }
    }
}
