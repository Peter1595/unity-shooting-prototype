using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.VFX;

namespace Unity.FPS.Gameplay
{
    public class ChargeElectricityVFX : MonoBehaviour
    {
        public VisualEffect vfx;

        WeaponController m_WeaponController;

        // Start is called before the first frame update
        void Start()
        {
            m_WeaponController = GetComponent<WeaponController>();

        }

        // Update is called once per frame
        void Update()
        {
            vfx.SetFloat("FlashScale", 1.25f * (m_WeaponController.CurrentCharge * 5));
            vfx.SetFloat("ElectricityScale", 1f * (m_WeaponController.CurrentCharge * 5));
            vfx.SetFloat("SparkesScale", 0.125f);
        }
    }
}
