using UnityEngine;

namespace Unity.FPS.Game
{
    // The Game Events used across the Game.
    // Anytime there is a need for a new event, it should be added here.

    public static class EventsReturnData
    {
        public static PlayerTransformEventReturnData playerTransformReturnData = new PlayerTransformEventReturnData();
    }

    public class PlayerTransformEventReturnData : GameEventReturnData
    {
        public Transform Transform;
    }
}
