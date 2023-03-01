 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    public class RoomGenerator : AbstractDungeonGenerator
    {
        [SerializeField]
        private BinarySplitSO parameters;

        protected override void RunPrecduarlDungeonGenerator()
        {
            RoomsGeneration();
        }

        private void RoomsGeneration()
        {
            HashSet<Vector3Int> floorPositions = SplitDungeon(StartingPosition
                , out List<Vector3Int> centers);

            visualizier.VisualizeFloor(floorPositions);

            WallGenerator.GenerateWalls(floorPositions, visualizier);
        }

        private List<Vector3Int> GetCenters(List<BoundsInt> rooms)
        {
            List<Vector3Int> centers = new List<Vector3Int>();

            foreach (BoundsInt room in rooms)
            {
                centers.Add(Vector3Int.RoundToInt(room.center));
            }

            return centers;
        }

        public HashSet<Vector3Int> SplitDungeon(
            Vector3Int StartingPosition
            , out List<Vector3Int> roomCenters
            , BinarySplitSO parameters = null)
        {
            if (!parameters)
            {
                parameters = this.parameters;
            }

            List<BoundsInt> rooms = GenerateRooms(StartingPosition
                , out roomCenters);

            HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

            foreach (BoundsInt room in rooms)
            {
                floorPositions.UnionWith(GenerateRoom(room, parameters.offset));
            }

            return floorPositions;
        }

        public List<BoundsInt> GenerateRooms(
            Vector3Int StartingPosition
            , out List<Vector3Int> roomCenters
            , BinarySplitSO parameters = null)
        {
            if (!parameters)
            {
                parameters = this.parameters;
            }

            List<BoundsInt> rooms =
                ProceduralDungeonGeneration.BinarySpacePartitioning(
                    new BoundsInt(StartingPosition
                    , new Vector3Int(parameters.minDungeonWidth, 0, parameters.minDungeonHeight))
                    , parameters.minRoomWidth, parameters.minRoomHeight);

            roomCenters = GetCenters(rooms);

            return rooms;
        }

        public HashSet<Vector3Int> GenerateRoom(BoundsInt room, int offset)
        {
            HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.z - offset; row++)
                {
                    floorPositions.Add(room.min + new Vector3Int(col, 0, row));
                }
            }

            return floorPositions;
        }
    }

}