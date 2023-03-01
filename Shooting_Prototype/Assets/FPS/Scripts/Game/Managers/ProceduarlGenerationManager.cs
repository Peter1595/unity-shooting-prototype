using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Unity.FPS.Game
{
    public enum GeneratingMethod
    {
        corridor,
        binarySplit,
    }


    public class ProceduarlGenerationManager : AbstractDungeonGenerator
    {
        [SerializeField]
        private SimpleRandomWalkGenerator simpleRandomWalkGenerator;
        [SerializeField]
        private CorridorGenerator corridorGenerator;
        [SerializeField]
        private RoomGenerator roomGenerator;
        [SerializeField]
        private SimpleRandomWalkSO roomParameters;
        [SerializeField]
        private GeneratingMethod generationMethod;

        protected override void RunPrecduarlDungeonGenerator()
        {
            if (generationMethod == GeneratingMethod.corridor)
            {
                UseCorridor();
            }
            else if (generationMethod == GeneratingMethod.binarySplit)
            {
                UseBinarySplit();
            }
        }

        private void UseCorridor()
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

        private void UseBinarySplit()
        {
            List<BoundsInt> rooms = roomGenerator.GenerateRooms(StartingPosition
                , out List<Vector3Int> roomCenters);


            HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

            foreach (BoundsInt room in rooms)
            {
                HashSet<Vector3Int> roomFloor = GenerateRoom(Vector3Int.RoundToInt(room.center)
                    , roomParameters);

                foreach (Vector3Int position in roomFloor)
                {
                    if (Directions.IsInRange(position
                        , Vector3Int.RoundToInt(room.center)
                        , room.size))
                    {
                        floorPositions.Add(position);
                    }
                }
            }

            floorPositions.UnionWith(ConnectRooms(roomCenters));

            visualizier.VisualizeFloor(floorPositions);

            WallGenerator.GenerateWalls(floorPositions, visualizier);
        }

        private HashSet<Vector3Int> ConnectRooms(List<Vector3Int> roomCenters)
        {
            HashSet<Vector3Int> corridors = new HashSet<Vector3Int>();

            Vector3Int currentRoomCenter =
                roomCenters[GenerationRandom.RandomIntRange(0, roomCenters.Count)];

            roomCenters.Remove(currentRoomCenter);

            while (roomCenters.Count > 0)
            {
                Vector3Int closest = FindClosestRoom(currentRoomCenter, roomCenters);

                roomCenters.Remove(closest);

                HashSet<Vector3Int> newCorridor = CreateCorridor(currentRoomCenter, closest);

                currentRoomCenter = closest;

                corridors.UnionWith(newCorridor);
            }

            return corridors;
        }

        private HashSet<Vector3Int> CreateCorridor(Vector3Int currentRoomCenter, Vector3Int closest)
        {
            HashSet<Vector3Int> corridor = new HashSet<Vector3Int>();

            Vector3Int midPoint;

            if (GenerationRandom.RandomIntRange(0,100) < 50)
            {
                midPoint = new Vector3Int(currentRoomCenter.x, currentRoomCenter.y, closest.z);
            }
            else
            {
                midPoint = new Vector3Int(closest.x, currentRoomCenter.y, currentRoomCenter.z);

            }

            List<Vector3Int> corridor1 = ProceduralDungeonGeneration
                .WalkCorridorToPoint(currentRoomCenter
                , midPoint);

            List<Vector3Int> corridor2 = ProceduralDungeonGeneration
                .WalkCorridorToPoint(midPoint
                , closest);

            corridor.UnionWith(corridor1);
            corridor.UnionWith(corridor2);

            return corridor;
        }

        private Vector3Int FindClosestRoom(Vector3Int currentRoomPosition, List<Vector3Int> roomCenters)
        {
            Vector3Int closest = Vector3Int.zero;

            float distance = float.MaxValue;

            foreach (Vector3Int roomCenter in roomCenters)
            {
                float currentDistance = Vector3Int.Distance(roomCenter, currentRoomPosition);

                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    closest = roomCenter;
                }
            }

            return closest;
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

        private HashSet<Vector3Int> GenerateRoom(Vector3Int StartingPosition
            , SimpleRandomWalkSO parameters = null)
        {
            return simpleRandomWalkGenerator.GenerateRandomWalk(StartingPosition, parameters);
        }
    }
}
