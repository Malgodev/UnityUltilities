using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Malgo.Utilities.UI
{
    public abstract class UIBaseAnimation : MonoBehaviour
    {
        public bool isPlayingOnStart;
        
        [Space(10)]
        [FoldoutGroup("Base Stat")] public float delayDuration;
        [FoldoutGroup("Base Stat")] public float animationDuration = 0.5f;
        [FoldoutGroup("Base Stat")] public Ease ease;

        [FoldoutGroup("Base Stat")] public LoopType loopType;
        [FoldoutGroup("Base Stat")] public float delayBetweenLoop;
        [FoldoutGroup("Base Stat")] public int loopCount;

        private void Start()
        {
            if (isPlayingOnStart)
            {
                Play();
            }
        }

        public abstract void Play();
        public abstract void PlayOnce(Action onComplete = null);
        
        public void Configure(
            float? delayDuration = null,
            float? animationDuration = null,
            Ease? ease = null,
            LoopType? loopType = null,
            float? delayBetweenLoop = null,
            int? loopCount = null)
        {
            if (delayDuration.HasValue) this.delayDuration = delayDuration.Value;
            if (animationDuration.HasValue) this.animationDuration = animationDuration.Value;
            if (ease.HasValue) this.ease = ease.Value;
            if (loopType.HasValue) this.loopType = loopType.Value;
            if (delayBetweenLoop.HasValue) this.delayBetweenLoop = delayBetweenLoop.Value;
            if (loopCount.HasValue) this.loopCount = loopCount.Value;
        }
        
        public abstract Sequence CreateSequence();
    }
}