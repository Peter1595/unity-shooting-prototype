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
            HashSet<Vector3Int> floorPositions = SplitDungeon(StartingPosition);

            visualizier.VisualizeFloor(floorPositions);

            WallGenerator.GenerateWalls(floorPositions, visualizier);
        }

        public HashSet<Vector3Int> SplitDungeon(Vector3Int StartingPosition, BinarySplitSO parameters = null)
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

            HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

            Debug.Log("rooms: " + rooms.Count);

            foreach (BoundsInt room in rooms)
            {
                
                floorPositions.UnionWith(GenerateRoom(room, parameters.offset));

                Debug.Log(room.size + "   " + floorPositions.Count);
            }

            return floorPositions;
        }

        public HashSet<Vector3Int> GenerateRoom(BoundsInt room, int offset)
        {
            HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

            for (int col = offset; col < room.size.x - offset; col++)
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