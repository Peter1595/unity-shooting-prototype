using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class LaserBeamVFX : MonoBehaviour
    {
        public LineRenderer beam;

        public Transform BeamEndBall;

        public Transform BeamBall;

        PlayerCharacterController player;

        LaserProjectile laserProjectile;

        RamPageParammeters ramPageParammeters;

        // Start is called before the first frame update
        void Start()
        {
            ramPageParammeters = GetComponent<RamPageParammeters>();

            laserProjectile = GetComponent<LaserProjectile>();

            player = FindAnyObjectByType<PlayerCharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            Ray mouseRay = player.PlayerCamera.ScreenPointToRay(Input.mousePosition);

            Vector3 mousePosition = Vector3.zero;

            if (Physics.Raycast(mouseRay, out RaycastHit hit))
            {
                mousePosition = hit.point;
            }
            else
            {
                mousePosition = player.PlayerCamera.transform.forward * -laserProjectile.MaxRange;
            }

            //Vector3 lastBeamPosition = beam.transform.position;

            //for (int i = 0; i < beam.positionCount - 1; i++)
            //{
            //    Vector3 position = beam.GetPosition(i);

            //    lastBeamPosition += position;
            //}

            float distance = Vector3.Distance(mousePosition, beam.transform.position);

            if (distance > laserProjectile.MaxRange)
            {
                mousePosition = player.PlayerCamera.transform.forward * -laserProjectile.MaxRange;
            }

            beam.SetPosition(beam.positionCount - 1, new Vector3(0,0, distance));

            beam.transform.LookAt(mousePosition);

            BeamEndBall.transform.LookAt(beam.transform.position);

            BeamEndBall.position = mousePosition;


            //transform.localRotation =
            //    Quaternion.EulerAngles(transform.localEulerAngles + new Vector3(0, 180, 0));
        }
    }
}
