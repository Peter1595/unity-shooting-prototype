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

        public GameObject wallUpBase;
        public GameObject wallDownBase;
        public GameObject wallRightBase;
        public GameObject wallLeftBase;

        public GameObject wallRightUpBase;
        public GameObject wallRightDownBase;
        public GameObject wallLeftUpBase;
        public GameObject wallLeftDownBase;

        GameObject Parent;

        public void VisualizeFloor(IEnumerable<Vector3Int> floorPositions)
        {
            VisualizeObjects(floorPositions, floorMap, floorBase);
        }

        private GameObject ChooseWallBase(string direction)
        {
            Debug.Log("direction name : " + direction);

            switch (direction)
            {
                case "Up":
                    return wallUpBase;
                case "Down":
                    return wallDownBase;
                case "Right":
                    return wallRightBase;
                case "Left":
                    return wallLeftBase;

                case "RightUp":
                    return wallRightUpBase;
                case "RightDown":
                    return wallRightDownBase;
                case "LeftUp":
                    return wallLeftUpBase;
                case "LeftDown":
                    return wallLeftDownBase;

                default:
                    return wallUpBase;

            }
        }

        public void VisualizeSingleWall(Vector3Int wallPosition, string direction)
        {
            VisualizeSingleObject(wallPosition, wallMap, ChooseWallBase(direction));
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

            GameObject gameObject = Instantiate(baseObject, position, baseObject.transform.rotation);

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
        internal void ReScaleMap(Dictionary<Vector3Int, GameObject> map)
        {
            foreach (GameObject item in map.Values)
            {
                item.transform.localScale /= 3;
            }
        }

        public void Clear()
        {
            ClearMap(floorMap);
            ClearMap(wallMap);
        }

        public void ScaleParent()
        {
            Parent.transform.localScale = Vector3.one * 3;

            ReScaleMap(floorMap);
            ReScaleMap(wallMap);
        }
    }
}
