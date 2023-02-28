using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEditor;
using UnityEngine;

namespace Unity.FPS.EditorExt
{
    [CustomEditor(typeof(AbstractDungeonGenerator), true)]
    public class DungeonGeneratorEditor : Editor
    {
        AbstractDungeonGenerator generator;

        private void Awake()
        {
            generator = (AbstractDungeonGenerator)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate Dungeon"))
            {
                generator.GenerateDungeon();
            }
        }
    }
}
