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
                visualizier.VisualizeSingleWall(wallPosition
                    , WallDirections.GetDirection(wallPosition, floorPositions));
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
    static class WallDirections
    {
        static HashSet<string> RightDirections = new HashSet<string>
        {
            "0001",

        };
        static HashSet<string> LeftDirections = new HashSet<string>
        {
            "0100",

        };
        static HashSet<string> DownDirections = new HashSet<string>
        {
            "1000",

        };
        static HashSet<string> UpDirections = new HashSet<string>
        {
            "0010",

        };

        static HashSet<string> RightUpDirections = new HashSet<string>
        {
            "0011",

        };
        static HashSet<string> RightDownDirections = new HashSet<string>
        {
            "1001",

        };
        static HashSet<string> LeftUpDirections = new HashSet<string>
        {
            "0101",

        };
        static HashSet<string> LeftDownDirections = new HashSet<string>
        {
            "0011",

        };

        private static string GetDirectionName(string direction)
        {
            Debug.Log("direction: " + direction);


            if (RightUpDirections.Contains(direction))
            {
                return "RightUp";
            }
            else if (RightDownDirections.Contains(direction))
            {
                return "RightDown";
            }
            else if (LeftUpDirections.Contains(direction))
            {
                return "LeftUp";
            }
            else if (LeftDownDirections.Contains(direction))
            {
                return "LeftDown";
            }

            else if (UpDirections.Contains(direction))
            {
                return "Up";
            }
            else if (DownDirections.Contains(direction))
            {
                return "Down";
            }
            else if (RightDirections.Contains(direction))
            {
                return "Right";
            }
            else if (LeftDirections.Contains(direction))
            {
                return "Left";
            }

            else return "Up";
        }

        public static string GetDirection(Vector3Int wall, HashSet<Vector3Int> floorPositions)
        {
            Vector3Int[] directions = Directions.GetAllDirections();

            string directionString = "";

            for (int i = 0; i < directions.Length; i++)
            {
                if (floorPositions.Contains(wall + directions[i]))
                {
                    directionString += "1";
                }
                else
                {
                    directionString += "0";
                }

                Debug.Log("direction " + i + " : " + directions[i] + ",  " + directionString);
            }

            return GetDirectionName(directionString);
        }
    }
}
