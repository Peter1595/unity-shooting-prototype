using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    public class ProjectileLightiningChain : MonoBehaviour
    {

        [Tooltip("Impact VFX Data")]
        public float ImpactVfxLifeTime = 1f;
        public GameObject ImpactVfx;

        [Tooltip("Chain VFX Data")]
        public float ChainVfxLifeTime = 1f;
        public GameObject ChainVfx;

        [Tooltip("The damage of each chainning shock")]
        public float damage = 25f;

        [Tooltip("Layers this Shock can collide with")]
        public LayerMask HittableLayers = -1;

        ProjectileStandard projectileStandard;
        DamageArea damageArea;

        const QueryTriggerInteraction k_TriggerInteraction = QueryTriggerInteraction.Collide;

        // Start is called before the first frame update
        void Start()
        {
            projectileStandard = GetComponent<ProjectileStandard>();

            if (!projectileStandard)
            {
                projectileStandard = GetComponentInParent<ProjectileStandard>();
            }

            damageArea = GetComponent<DamageArea>();

            if (!damageArea)
            {
                damageArea = GetComponentInParent<DamageArea>();
            }

            if (!(damageArea && projectileStandard))
            {
                return;
            }

            projectileStandard.OnDamaged += ChainAll;

        }

        // Update is called once per frame
        void Update()
        {

        }

        void ChainAll(GameObject FirstDamageable)
        {
            if (!(damageArea && projectileStandard))
            {
                return;
            }

            Damageable[] damageables = damageArea.InflictDamageInArea(0, FirstDamageable.transform.position,
                HittableLayers, k_TriggerInteraction, projectileStandard.GetOwner());

            foreach (Damageable damageable in damageables)
            {
                Chain(damageable, FirstDamageable.transform.position);
            }
        }

        void Chain(Damageable enemy, Vector3 center)
        {
            GameObject chainVfxInstance = Instantiate(ChainVfx, center, Quaternion.identity);

            chainVfxInstance.transform.LookAt(enemy.transform.position);

            if (ImpactVfxLifeTime > 0)
            {
                Destroy(chainVfxInstance.gameObject, ImpactVfxLifeTime);
            }

            ParticleSystem particleSystem = chainVfxInstance.GetComponent<ParticleSystem>();

            if (!particleSystem)
            {
                return;
            }

            float distance = Vector3.Distance(enemy.transform.position, center);

            particleSystem.startSpeed = distance / particleSystem.startLifetime;

            particleSystem.loop = false;

            Shock(enemy);
        }

        void Shock(Damageable enemy)
        {
            enemy.InflictDamage(damage, true, projectileStandard.GetOwner());

            GameObject impactVfxInstance = Instantiate(ImpactVfx, enemy.transform.position, Quaternion.identity);
            if (ImpactVfxLifeTime > 0)
            {
                Destroy(impactVfxInstance.gameObject, ImpactVfxLifeTime);
            }

            ParticleSystem particleSystem = impactVfxInstance.GetComponent<ParticleSystem>();

            if (!particleSystem)
            {
                return;
            }

            particleSystem.loop = false;
        }
    }
}
