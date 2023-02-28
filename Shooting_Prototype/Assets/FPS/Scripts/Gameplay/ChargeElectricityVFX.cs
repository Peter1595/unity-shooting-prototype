using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.VFX;

namespace Unity.FPS.Gameplay
{
    public class ChargeElectricityVFX : MonoBehaviour
    {
        public Transform Muzzle;

        public VisualEffect mainVFX;

        VisualEffect vfx = null;

        WeaponController m_WeaponController;

        // Start is called before the first frame update
        void Start()
        {
            m_WeaponController = GetComponent<WeaponController>();
        }

        // Update is called once per frame
        void Update()
        {
            if ((!m_WeaponController.IsCharging) && vfx)
            {
                Debug.Log("charging : " + m_WeaponController.IsCharging);

                Destroy(vfx.gameObject);

                vfx = null;

                return;
            }

            if ((!vfx) && m_WeaponController.IsCharging)
            {
                Debug.Log("vfx is null");

                vfx = Instantiate(mainVFX, mainVFX.transform.position, Quaternion.identity);
            }

            if (!vfx)
            {
                return;
            }

            vfx.enabled = true;

            vfx.transform.position = Muzzle.position;

            vfx.SetFloat("FlashScale", 1.25f * (m_WeaponController.CurrentCharge * 2));
            vfx.SetFloat("ElectricityScale", 1f * (m_WeaponController.CurrentCharge * 2));

            vfx.SetFloat("SparkesScale", 0.125f);
        }
    }
}
