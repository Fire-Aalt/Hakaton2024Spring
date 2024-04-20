using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerDeathData : PlayerStateData
    {
        [Header("References")]
        public MMF_Player DeathFeedback;
        public MMF_Player RespawnFeedback;
    }
}
