using Codice.Client.BaseCommands;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    public static class ProceduralDungeonGeneration
    {
        public static HashSet<Vector3Int> SimpleRandomWalk(Vector3Int startingPosition, int walkLength)
        {
            HashSet<Vector3Int> path = new HashSet<Vector3Int>();

            path.Add(startingPosition);

            Vector3Int lastPosition = startingPosition;

            for (int i = 0; i < walkLength; i++)
            {
                Vector3Int newPosition = lastPosition + Directions.PickRandomDirection();

                lastPosition = newPosition;

                path.Add(newPosition);
            }

            return path;
        }

        public static List<Vector3Int> RandomWalkCorridor(Vector3Int startPosition, int corridorLength)
        {
            List<Vector3Int> corridor = new List<Vector3Int>();

            Vector3Int direction = Directions.PickRandomDirection();

            Vector3Int currentPosition = startPosition;

            corridor.Add(currentPosition);

            for(int i = 0;i < corridorLength;i++)
            {
                currentPosition += direction;

                corridor.Add(currentPosition);
            }

            return corridor;
        }

        public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
        {
            Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();

            List<BoundsInt> rooms = new List<BoundsInt>();

            roomsQueue.Enqueue(spaceToSplit);

            while (roomsQueue.Count > 0)
            {
                BoundsInt room = roomsQueue.Dequeue();

                Debug.Log("Queuing...: " + room);

                if (room.size.z >= minHeight && room.size.x >= minWidth)
                {
                    Debug.Log("Splitting...: " + room);

                    if (GenerationRandom.RandomIntRange(0, 100) < 50)
                    {
                        Debug.Log("start hor");

                        if (room.size.z >= minHeight * 2)
                        {
                            SplitHor(minWidth, roomsQueue, room);
                        }
                        else if (room.size.x >= minWidth * 2)
                        {
                            SplitVer(minHeight, roomsQueue, room);
                        }
                        else
                        {
                            Debug.Log("Adding...");

                            rooms.Add(room);
                        }
                    }
                    else
                    {
                        Debug.Log("start ver");

                        if (room.size.x >= minWidth * 2)
                        {
                            SplitVer(minWidth, roomsQueue, room);
                        }
                        else if (room.size.z >= minHeight * 2)
                        {
                            SplitHor(minHeight, roomsQueue, room);
                        }
                        else
                        {
                            Debug.Log("Adding...");

                            rooms.Add(room);
                        }
                    }
                }
            }

            Debug.Log("Rooms: " + rooms.Count);

            return rooms;
        }

        private static void SplitHor(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
        {
            Debug.Log("Splitting Hor...");

            int zSplit = GenerationRandom.RandomIntRange(1, room.size.z);

            BoundsInt room1 = new BoundsInt(room.min, room.size + new Vector3Int(0, 0, -room.size.z + zSplit));
            BoundsInt room2 = new BoundsInt(room.min + new Vector3Int(0, 0, zSplit)
                , room.size - new Vector3Int(0, 0, zSplit));

            roomsQueue.Enqueue(room1);
            roomsQueue.Enqueue(room2);
        }

        private static void SplitVer(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
        {
            Debug.Log("Splitting Ver...");

            int xSplit = GenerationRandom.RandomIntRange(1, room.size.x);

            BoundsInt room1 = new BoundsInt(room.min, room.size + new Vector3Int(-room.size.x + xSplit, 0, 0));
            BoundsInt room2 = new BoundsInt(room.min + new Vector3Int(xSplit, 0, 0)
                , room.size - new Vector3Int(xSplit, 0, 0));

            roomsQueue.Enqueue(room1);
            roomsQueue.Enqueue(room2);
        }
    }

    static class Directions
    {
        private static List<Vector3Int> _directions = new List<Vector3Int>
        {
            new Vector3Int(0,0,1), // FORWARD
            new Vector3Int(1,0,0), // RIGHT
            new Vector3Int(0,0,-1), // BACK
            new Vector3Int(-1,0,0), // LEFT
        };

        public static Vector3Int PickRandomDirection()
        {
            return _directions[GenerationRandom.RandomIntRange(0, _directions.Count)];
        }

        public static Vector3Int[] GetAllDirections()
        {
            return _directions.ToArray();
        }

        public static int GetDirectionsCount()
        {
            return _directions.Count;
        }
    }

    class GenerationRandom
    {
        private static System.Random mainRandom = null;

        private static System.Random GetMainRandom()
        {
            if (mainRandom == null)
            {
                mainRandom = new System.Random();
            }

            return mainRandom;
        }

        public static int RandomIntValue()
        {
            return GetMainRandom().Next();
        }
        public static double RandomDoubleValue()
        {
            return GetMainRandom().NextDouble();
        }
        public static int RandomIntRange(int min, int max)
        {
            return GetMainRandom().Next(min, max);
        }

        private System.Random random;

        public GenerationRandom(int seed)
        {
            random = new System.Random(seed);
        }

        public int NextIntValue()
        {
            return random.Next();
        }
        public double NextDoubleValue()
        {
            return random.NextDouble();
        }
        public int NextIntRange(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}