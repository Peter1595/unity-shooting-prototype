using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    public static class SpawnAreaGenerator
    {
        public static Vector3 GetHitPosition(Vector3 spawnPosition)
        {
            if (Physics.Raycast(spawnPosition + new Vector3(0, 999999, 0), Vector3.down, out RaycastHit hit, float.MaxValue))
            {
                return hit.point;
            }

            return Vector3.zero;
        }

        public static Vector3 GetSpawnPosition(Bounds area)
        {
            Vector3 hitPosition = GetHitPosition(area.center +
                new Vector3(Random.Range(-area.size.x / 2, area.size.x / 2), 0, Random.Range(-area.size.z / 2, area.size.z / 2)));

            if (hitPosition == Vector3.zero)
            {
                return GetSpawnPosition(area);
            }

            return hitPosition;
        }


        public static Bounds GetRandomSpawnArea(Vector3 Center, Vector3 Size)
        {
            Vector3 hitPosition = GetHitPosition(Center +
                new Vector3(Random.Range(-Size.x / 2, Size.x / 2), 0, Random.Range(-Size.z / 2, Size.z / 2)));

            if (hitPosition == Vector3.zero)
            {
                return GetRandomSpawnArea(Center, Size);
            }

            return new Bounds(hitPosition, new Vector3(10, 0, 10));
        }
    }

}
