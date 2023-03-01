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
            HashSet<Vector3Int> floorPositions = SplitDungeon(
                out List<Vector3Int> centers
                , out List<BoundsInt> rooms
                , StartingPosition
                , true);

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

        private List<BoundsInt> ApplyOffsetToRooms(List<BoundsInt> rooms, int offset)
        {
            List<BoundsInt> newRooms = new List<BoundsInt>();

            foreach (BoundsInt room in rooms)
            {
                BoundsInt newRoom = room;

                newRoom.min = newRoom.min + new Vector3Int(offset, 0, offset);
                newRoom.size = newRoom.size - new Vector3Int(offset, 0, offset);

                newRooms.Add(newRoom);
            }

            return newRooms;
        }

        public HashSet<Vector3Int> SplitDungeon(
            out List<Vector3Int> roomCenters
            , out List<BoundsInt> rooms
            , Vector3Int StartingPosition
            , bool generateSimpleRooms = false
            , BinarySplitSO parameters = null)
        {
            if (!parameters)
            {
                parameters = this.parameters;
            }

            rooms = GenerateRooms(out roomCenters
                , StartingPosition
                , parameters);

            HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

            rooms = ApplyOffsetToRooms(rooms, parameters.offset);

            if (generateSimpleRooms)
            {
                foreach (BoundsInt room in rooms)
                {
                    floorPositions.UnionWith(GenerateRoom(room));
                }
            }

            return floorPositions;
        }

        public List<BoundsInt> GenerateRooms(out List<Vector3Int> roomCenters
            , Vector3Int StartingPosition
            , BinarySplitSO parameters = null)
        {
            if (!parameters)
            {
                parameters = this.parameters;
            }

            List<BoundsInt> rooms =
                ProceduralDungeonGeneration.BinarySpacePartitioning(
                    new BoundsInt(StartingPosition
                    , new Vector3Int(parameters.minDungeonWidth, 0, parameters.minDungeonHeight)
                        * Directions.GetSize())
                    , parameters.minRoomWidth * Directions.GetSize()
                        , parameters.minRoomHeight * Directions.GetSize()
                );

            roomCenters = GetCenters(rooms);

            return rooms;
        }

        public HashSet<Vector3Int> GenerateRoom(BoundsInt room)
        {
            HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

            for (int col = 0; col < room.size.x; col++)
            {
                for (int row = 0; row < room.size.z; row++)
                {
                    floorPositions.Add(room.min + new Vector3Int(col, 0, row));
                }
            }

            return floorPositions;
        }
    }

}