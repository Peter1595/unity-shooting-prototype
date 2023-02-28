using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    [CreateAssetMenu(fileName = "BinarySplitParameters_", menuName = "PCG/BinarySplitData")]
    public class BinarySplitSO : ScriptableObject
    {
        public int minRoomWidth = 4, minRoomHeight = 4;
        public int minDungeonWidth = 20, minDungeonHeight = 20;
        public int offset = 1;
    }
}
