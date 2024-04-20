using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game
{
    public class PlayerHitData : PlayerStateData
    {
        [Header("References")]
        public float FadeInDelay;
        public MMF_Player HitFeedback;
        public MMF_Player FadeInFeedback;
    }
}
