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

        private Vector3Int GetClosestPosition(Vector3Int first, HashSet<Vector3Int> other)
        {
            float minDistance = float.MaxValue;
            Vector3Int closest = Vector3Int.zero;

            foreach (Vector3Int position in other)
            {
                float distance = Vector3Int.Distance(position, first);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = position;
                }
            }

            Debug.Log("closest floor: " + closest + ",  " + first);

            return closest;
        }

        private Vector3Int GetCenter(BoundsInt room, HashSet<Vector3Int> floor)
        {
            Vector3Int center = GetClosestPosition(Vector3Int.RoundToInt(room.center),floor);

            return center;
        }

        private List<BoundsInt> ApplyOffsetToRooms(List<BoundsInt> rooms, int offset)
        {
            offset *= Directions.GetSize();

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

            roomCenters = new List<Vector3Int>();

            rooms = GenerateRooms(StartingPosition
                , parameters);

            HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

            rooms = ApplyOffsetToRooms(rooms, parameters.offset);

            foreach (BoundsInt room in rooms)
            {
                HashSet<Vector3Int> floor = GenerateRoom(room);

                roomCenters.Add(GetCenter(room, floor));

                floorPositions.UnionWith(floor);
            }

            if (generateSimpleRooms)
            {
                return floorPositions;
            }
            return new HashSet<Vector3Int>();
        }

        public List<BoundsInt> GenerateRooms(Vector3Int StartingPosition
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

            return rooms;
        }

        public HashSet<Vector3Int> GenerateRoom(BoundsInt room)
        {
            HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

            for (int col = 0; col < room.size.x / Directions.GetSize(); col++)
            {
                for (int row = 0; row < room.size.z / Directions.GetSize(); row++)
                {
                    floorPositions.Add(room.min + (new Vector3Int(col, 0, row) * Directions.GetSize()));
                }
            }

            return floorPositions;
        }
    }

}