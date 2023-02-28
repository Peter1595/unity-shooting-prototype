using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace Unity.FPS.Game
{

    public class SimpleRandomWalkGenerator : AbstractDungeonGenerator
    {
        [SerializeField]
        private SimpleRandomWalkSO parameters;

        protected override void RunPrecduarlDungeonGenerator()
        {
            SimpleWalkGeneration();
        }

        private void SimpleWalkGeneration()
        {
            HashSet<Vector3Int> floorPositions = GenerateRandomWalk(StartingPosition, parameters);

            visualizier.VisualizeFloor(floorPositions);

            WallGenerator.GenerateWalls(floorPositions, visualizier);
        }

        public HashSet<Vector3Int> GenerateRandomWalk(Vector3Int StartingPosition, SimpleRandomWalkSO parameters = null)
        {
            if (!parameters)
            {
                parameters = this.parameters;
            }

            Vector3Int currentPosition = StartingPosition;

            HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

            for (int i = 0; i < parameters.interations; i++)
            {
                HashSet<Vector3Int> path = ProceduralDungeonGeneration.SimpleRandomWalk(currentPosition, parameters.walkLength);

                floorPositions.UnionWith(path);

                if (parameters.startRandomlyEachInteration && floorPositions.Count > 0)
                {
                    currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
                }
            }

            return floorPositions;
        }
    }
}
