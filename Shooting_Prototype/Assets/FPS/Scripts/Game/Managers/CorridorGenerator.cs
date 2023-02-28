using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity.FPS.Game
{
    public class CorridorGenerator : AbstractDungeonGenerator
    {
        [SerializeField]
        private CorridorSO parameters;

        protected override void RunPrecduarlDungeonGenerator()
        {
            CorridorFirstGeneration();
        }

        private void CorridorFirstGeneration()
        {
            HashSet<Vector3Int> floorPositions = GenerateCorridors(
                out HashSet<Vector3Int> spawnRoomsPositions
                , out HashSet<Vector3Int> deadEnds, StartingPosition);

            visualizier.VisualizeFloor(floorPositions);

            WallGenerator.GenerateWalls(floorPositions, visualizier);
        }

        public HashSet<Vector3Int> GenerateCorridors(out HashSet<Vector3Int> spawnRoomsPositions
            , out HashSet<Vector3Int> deadEnds, Vector3Int StartingPosition, CorridorSO parameters = null)
        {
            if (!parameters)
            {
                parameters = this.parameters;
            }

            HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

            spawnRoomsPositions = new HashSet<Vector3Int>();

            Vector3Int currentPosition = StartingPosition;

            spawnRoomsPositions.Add(currentPosition);

            for (int i = 0; i < parameters.corridorAmount; i++)
            {
                List<Vector3Int> corridor =
                    ProceduralDungeonGeneration.RandomWalkCorridor(currentPosition, parameters.corridorLength);

                currentPosition = corridor[corridor.Count - 1];

                spawnRoomsPositions.Add(currentPosition);

                floorPositions.UnionWith(corridor);
            }

            deadEnds = GetDeadEnds(floorPositions, spawnRoomsPositions);

            spawnRoomsPositions = TakePercentFromSpawnRoomsPositions(spawnRoomsPositions, parameters.roomRatio);

            return floorPositions;
        }

        private HashSet<Vector3Int> GetDeadEnds(HashSet<Vector3Int> floorPositions, HashSet<Vector3Int> spawnRoomsPositions)
        {
            HashSet<Vector3Int> deadEnds = new HashSet<Vector3Int>();

            foreach (Vector3Int spawnRoomPosition in spawnRoomsPositions)
            {
                int sidePositionsCount = 0;
                foreach (Vector3Int direction in Directions.GetAllDirections())
                {
                    if (floorPositions.Contains(spawnRoomPosition + direction))
                    {
                        sidePositionsCount++;
                    }
                }

                if (sidePositionsCount <= 1)
                {
                    deadEnds.Add(spawnRoomPosition);
                }
            }

            return deadEnds;
        }

        private HashSet<Vector3Int> TakePercentFromSpawnRoomsPositions(HashSet<Vector3Int> spawnRoomsPositions, float percent)
        {
            int newSpawnRoomsPositionsCount = Mathf.RoundToInt(spawnRoomsPositions.Count * percent);

            List<Vector3Int> newSpawnRoomsPositions = spawnRoomsPositions.OrderBy(x => Guid.NewGuid()).Take(newSpawnRoomsPositionsCount).ToList();

            return newSpawnRoomsPositions.ToHashSet();
        }
    }

}