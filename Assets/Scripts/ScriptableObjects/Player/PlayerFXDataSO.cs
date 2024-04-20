using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "newPlayerFXData", menuName = "Data/Player Data/FX Data")]
    public class PlayerFXDataSO : SerializedScriptableObject
    {
        public Dictionary<SurfaceType, SurfaceDetails> surfaceDictionary = new();
    }

    [Serializable]
    public struct SurfaceDetails
    {
        [Header("Sound Effects")]
        public SoundEffectSO footstepSFX;
        public SoundEffectSO jumpSFX;
        public SoundEffectSO landSFX;
    }
}