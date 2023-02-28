using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{

    public class ProceduarlGenerationManager : AbstractDungeonGenerator
    {
        [SerializeField]
        private SimpleRandomWalkGenerator simpleRandomWalkGenerator;
        [SerializeField]
        private CorridorGenerator corridorGenerator;

        protected override void RunPrecduarlDungeonGenerator()
        {
            GenerateCorridor();
        }

        private void GenerateCorridor()
        {
            HashSet<Vector3Int> floorPositions =
                corridorGenerator.GenerateCorridors(out HashSet<Vector3Int> spawnRoomsPositions
                , out HashSet<Vector3Int> deadEnds
                , StartingPosition);

            spawnRoomsPositions.UnionWith(deadEnds);

            floorPositions.UnionWith(GenerateRooms(spawnRoomsPositions));

            visualizier.VisualizeFloor(floorPositions);

            WallGenerator.GenerateWalls(floorPositions, visualizier);
        }

        private HashSet<Vector3Int> GenerateRooms(HashSet<Vector3Int> spawnRoomsPositions)
        {
            HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

            foreach (Vector3Int spawnRoomPosition in spawnRoomsPositions)
            {
                floorPositions.UnionWith(GenerateRoom(spawnRoomPosition));
            }

            return floorPositions;
        }

        private HashSet<Vector3Int> GenerateRoom(Vector3Int StartingPosition)
        {
            return simpleRandomWalkGenerator.GenerateRandomWalk(StartingPosition);
        }
    }
}
