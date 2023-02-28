using Codice.Client.BaseCommands;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Unity.FPS.Game
{
    public class DungeonVisualizier : MonoBehaviour
    {
        private Dictionary<Vector3Int, GameObject> floorMap = new Dictionary<Vector3Int, GameObject>();
        private Dictionary<Vector3Int, GameObject> wallMap = new Dictionary<Vector3Int, GameObject>();

        public GameObject floorBase;
        public GameObject wallBase;

        GameObject Parent;

        public void VisualizeFloor(IEnumerable<Vector3Int> floorPositions)
        {
            VisualizeObjects(floorPositions, floorMap, floorBase);
        }

        public void VisualizeSingleWall(Vector3Int wallPosition)
        {
            VisualizeSingleObject(wallPosition, wallMap, wallBase);
        }

        private void VisualizeObjects(IEnumerable<Vector3Int> positions,
            Dictionary<Vector3Int, GameObject> map,
            GameObject baseObject)
        {
            foreach (var position in positions)
            {
                VisualizeSingleObject(position, map, baseObject);
            }
        }

        internal void VisualizeSingleObject(Vector3Int position, Dictionary<Vector3Int, GameObject> map, GameObject baseObject)
        {
            if (!Parent)
            {
                Parent = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Parent.name = "VisualizedDungeon";
            }

            GameObject gameObject = Instantiate(baseObject, position, Quaternion.identity);

            Debug.Log("Visualizing: " + gameObject);

            map.Add(position, gameObject);

            gameObject.transform.parent = Parent.transform;
        }

        internal void ClearMap(Dictionary<Vector3Int, GameObject> map)
        {
            foreach (GameObject item in map.Values)
            {
                DestroyImmediate(item);
            }

            map.Clear();
        }

        public void Clear()
        {
            ClearMap(floorMap);
            ClearMap(wallMap);
        }
    }
}
