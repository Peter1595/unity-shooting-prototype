using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    [CreateAssetMenu(fileName = "CorridorParameters_", menuName = "PCG/CorridorData")]
    public class CorridorSO : ScriptableObject
    {
        public int corridorLength = 10, corridorAmount = 5;
        public float roomRatio = 0.8f;
    }
}
