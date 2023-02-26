using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.FPS.Gameplay
{
    public class ChargeRailRotate : MonoBehaviour
    {
        public GameObject[] Rails;

        public Vector3 TargetRotate;

        List<MinMaxVector3> MinMaxRotates = new List<MinMaxVector3>();

        WeaponController m_WeaponController;

        // Start is called before the first frame update
        void Start()
        {
            m_WeaponController = GetComponent<WeaponController>();

            foreach (GameObject rail in Rails)
            {
                MinMaxVector3 minmaxRotate = new MinMaxVector3();

                minmaxRotate.Min = rail.transform.localRotation.ToEulerAngles();

                minmaxRotate.Max = TargetRotate;

                Debug.Log("min max rotate: " + minmaxRotate.Min + " - " + minmaxRotate.Max);

                MinMaxRotates.Add(minmaxRotate);
            }
        }

        // Update is called once per frame
        void Update()
        {
            int i = 0;

            foreach (var maxmin in MinMaxRotates)
            {
                GameObject rail = Rails[i];

                rail.transform.localRotation = Quaternion.EulerAngles(maxmin.GetValueFromRatio(m_WeaponController.CurrentCharge));

                i += 1;
            }
        }
    }
}
