using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Drop : MonoBehaviour
{
    [Tooltip("Sound played on claim")] public AudioClip PickupSfx;
    [Tooltip("VFX spawned on claim")] public GameObject PickupVfxPrefab;

    protected Collider m_Collider;
    protected Rigidbody m_Rigidbody;

    bool m_HasPlayedFeedback = false;

    public float speed;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_Collider = GetComponent<Collider>();
        m_Rigidbody = GetComponent<Rigidbody>();

        m_Rigidbody.isKinematic = false;
        m_Rigidbody.useGravity = true;

        m_Collider.isTrigger = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        PlayerCharacterController pickingPlayer = other.GetComponent<PlayerCharacterController>();

        if (pickingPlayer != null)
        {
            OnClaim(pickingPlayer);

            PickupEvent evt = Events.PickupEvent;
            evt.Pickup = gameObject;
            EventManager.Broadcast(evt);
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
