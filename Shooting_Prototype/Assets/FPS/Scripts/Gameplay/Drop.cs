using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    public class Drop : MonoBehaviour
    {
        [Tooltip("Sound played on claim")] public AudioClip PickupSfx;
        [Tooltip("VFX spawned on claim")] public GameObject PickupVfxPrefab;

        protected BoxCollider m_Collider;
        protected Rigidbody m_Rigidbody;
        protected ParabolaController m_ParabolaController;


        protected Transform parabolaRoot;

        protected Transform lastParabola;
        protected Transform middleParabola;
        protected Transform firstParabola;

        bool m_HasPlayedFeedback = false;

        public Transform Item;
        public float Speed;
        public float SpeedIncressPerSecond;
        public Vector3 Offset;

        protected Transform Target;

        protected Vector3 randomOffset;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            m_Collider = GetComponent<BoxCollider>();

            if (!m_Collider)
            {
                m_Collider = GetComponentInChildren<BoxCollider>();
            }

            m_Rigidbody = GetComponent<Rigidbody>();

            if (!m_Rigidbody)
            {
                m_Rigidbody = GetComponentInChildren<Rigidbody>();
            }

            m_ParabolaController = GetComponent<ParabolaController>();

            if (!m_ParabolaController)
            {
                m_ParabolaController = GetComponentInChildren<ParabolaController>();
            }

            parabolaRoot = m_ParabolaController.ParabolaRoot.transform;

            m_ParabolaController.ConstantlyUpdateTransforms = true;


            m_Rigidbody.isKinematic = true;
            m_Rigidbody.useGravity = true;

            m_Collider.isTrigger = true;

            Target = FindAnyObjectByType<PlayerCharacterController>().transform;

            lastParabola = parabolaRoot.GetChild(parabolaRoot.childCount - 1);
            middleParabola = parabolaRoot.GetChild(parabolaRoot.childCount - 2);
            firstParabola = parabolaRoot.GetChild(0);

            randomOffset
            = middleParabola.right * Random.Range(-Offset.x, Offset.x)
            + middleParabola.up * Random.Range(-Offset.y, Offset.y)
            + middleParabola.forward * Random.Range(-Offset.z, Offset.z);

            transform.LookAt(Target.position);

            lastParabola.position = Target.position + new Vector3(0, 1, 0);

            middleParabola.position = Vector2.Lerp(firstParabola.position, lastParabola.position, 0.5f);

            middleParabola.LookAt(lastParabola.position);

            float distancePercent = Vector3.Distance(lastParabola.position, firstParabola.position) / 100;

            m_ParabolaController.Speed = Speed / distancePercent;

            middleParabola.position
            += randomOffset / distancePercent;

            m_Collider.center = Item.localPosition;

            m_ParabolaController.Run();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (!Target)
            {
                return;
            }

            transform.LookAt(Target.position);

            lastParabola.position = Target.position + new Vector3(0, 1, 0);

            middleParabola.position = Vector2.Lerp(firstParabola.position, lastParabola.position, 0.5f);

            middleParabola.LookAt(lastParabola.position);

            float distancePercent = Vector3.Distance(lastParabola.position, firstParabola.position) / 100;

            m_ParabolaController.Speed += SpeedIncressPerSecond * Time.deltaTime;

            middleParabola.position
            += randomOffset / distancePercent;

            m_Collider.center = Item.localPosition;
        }

        void OnTriggerEnter(Collider other)
        {
            PlayerCharacterController pickingPlayer = other.GetComponent<PlayerCharacterController>();
            if (pickingPlayer != null)
            {
                OnClaim(pickingPlayer);
            }
        }
        protected virtual void OnClaim(PlayerCharacterController player)
        {
            PlayClaimFeedback();
        }

        public void PlayClaimFeedback()
        {
            if (m_HasPlayedFeedback)
                return;

            if (PickupSfx)
            {
                AudioUtility.CreateSFX(PickupSfx, transform.position, AudioUtility.AudioGroups.Pickup, 0f);
            }

            if (PickupVfxPrefab)
            {
                var pickupVfxInstance = Instantiate(PickupVfxPrefab, transform.position, Quaternion.identity);
            }

            m_HasPlayedFeedback = true;
        }
    }

}