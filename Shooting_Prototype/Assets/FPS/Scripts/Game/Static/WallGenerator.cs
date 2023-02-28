using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace Unity.FPS.Game
{
    public static class WallGenerator
    {
        public static void GenerateWalls(HashSet<Vector3Int> floorPositions, DungeonVisualizier visualizier)
        {
            HashSet<Vector3Int> wallPositions = GetWallPositions(floorPositions, Directions.GetAllDirections());

            foreach (Vector3Int wallPosition in wallPositions)
            {
                visualizier.VisualizeSingleWall(wallPosition);
            }
        }

        private static HashSet<Vector3Int> GetWallPositions(HashSet<Vector3Int> floorPositions, Vector3Int[] directions)
        {
            HashSet<Vector3Int> wallPositions = new HashSet<Vector3Int>();

            foreach (Vector3Int position in floorPositions)
            {
                foreach (Vector3Int direction in directions)
                {
                    Vector3Int wallPosition = position + direction;

                    if (!floorPositions.Contains(wallPosition))
                    {
                        wallPositions.Add(wallPosition);
                    }
                }
            }

            return wallPositions;
        }
    }
}
