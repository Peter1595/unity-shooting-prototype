using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class LaserBeamVFX : MonoBehaviour
    {
        public Vector3[] scaleOverStages;

        public LineRenderer beam;

        public Transform BeamEndBall;

        public Transform BeamBall;

        PlayerCharacterController player;

        LaserProjectile laserProjectile;

        [Tooltip("Layers this projectile can collide with")]
        public LayerMask HittableLayers = -1;

        const QueryTriggerInteraction k_TriggerInteraction = QueryTriggerInteraction.Collide;

        Vector3 currentScale;

        Vector3 endBallScale;
        Vector3 ballScale;

        // Start is called before the first frame update
        void Start()
        {
            endBallScale = BeamEndBall.transform.localScale;
            ballScale = BeamBall.transform.localScale;

            laserProjectile = GetComponent<LaserProjectile>();

            player = FindAnyObjectByType<PlayerCharacterController>();

            currentScale = scaleOverStages[laserProjectile.GetRamPageStage()];
        }

        Vector3 GetMouseDirection()
        {
            return Vector3.Slerp(Input.mousePosition, UnityEngine.Random.insideUnitSphere,
            0);
        }

        Vector3 GetHitPosition()
        {

            Vector3 hitPosition = GetMouseDirection();

            Ray ray = player.PlayerCamera.ScreenPointToRay(hitPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, laserProjectile.MaxRange, HittableLayers, k_TriggerInteraction))
            {
                hitPosition = hit.point;
            }
            else
            {
                hitPosition = player.PlayerCamera.transform.forward * laserProjectile.MaxRange;
            }

            return hitPosition;
        }

        // Update is called once per frame
        void Update()
        {
            currentScale = scaleOverStages[laserProjectile.GetRamPageStage()];

            Vector3 hitPosition = GetHitPosition();

            float distance = Vector3.Distance(hitPosition, beam.transform.position);

            BeamEndBall.LookAt(beam.transform.position);

            BeamEndBall.position = hitPosition;

            BeamEndBall.localScale =
                Vector3.LerpUnclamped(BeamEndBall.localScale, endBallScale + currentScale, 0.1f);
            BeamBall.localScale =
                Vector3.LerpUnclamped(BeamBall.localScale, ballScale + currentScale, 0.1f);

            beam.SetPosition(beam.positionCount - 1, new Vector3(0,0, distance));

            beam.transform.LookAt(BeamEndBall.position);
        }
    }
}
