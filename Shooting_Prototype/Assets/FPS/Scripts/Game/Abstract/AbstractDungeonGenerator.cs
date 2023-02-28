using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Game
{
    public abstract class AbstractDungeonGenerator : MonoBehaviour
    {
        [SerializeField]
        protected DungeonVisualizier visualizier = null;
        [SerializeField]
        protected Transform StartPoint = null;

        protected Vector3Int StartingPosition
        {
            get
            {
                if (!StartPoint)
                {
                    return Vector3Int.zero;
                }

                return Vector3Int.RoundToInt(StartPoint.position);
            }
        }

        public void GenerateDungeon()
        {
            if (visualizier == null)
            {
                return;
            }

            visualizier.Clear();

            RunPrecduarlDungeonGenerator();
        }

        protected abstract void RunPrecduarlDungeonGenerator();
    }
}