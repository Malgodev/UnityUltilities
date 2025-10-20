using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Malgo.Utilities.UI
{
    public class UIFadeAnimation : UIBaseAnimation
    {
        [Header("Fade stat")]
        public float alpha;
        public Image image;

        public enum FadeType
        {
            ToTarget = 0,
            FadeIn = 1,
            FadeOut = 2,
        }
        
        public FadeType fadeType;
        
        public override void Play()
        {
            Sequence sequence = CreateSequence();
            
            sequence.SetLoops(loopCount, loopType);
            sequence.Play();
        }

        [Sirenix.OdinInspector.Button]
        public override void PlayOnce(Action onComplete = null)
        {
            Sequence sequence = CreateSequence();
            
            sequence.Play();
        }

        public override Sequence CreateSequence()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.SetDelay(delayDuration);
            
            float targetAlpha = alpha;

            switch (fadeType)
            {
                case FadeType.FadeIn:
                    targetAlpha = 1f;
                    break;
                case FadeType.FadeOut:
                    targetAlpha = 0f;
                    break;
            }
            
            sequence.Append(
                image
                    .DOFade(targetAlpha, animationDuration)
                    .SetEase(ease)
                );
            
            return sequence;
        }
    }
}
