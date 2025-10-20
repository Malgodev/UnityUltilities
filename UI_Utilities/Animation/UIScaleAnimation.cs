using System;
using DG.Tweening;
using Malgo.Utilities.UI;
using UnityEngine;

namespace Malgo.Utilities.UI
{
    public class UIScaleAnimation :  UIBaseAnimation
    {
        [Header("Scale Stat")] 
        public Vector3 scale;
        public Transform targetTransform;
        
        public enum ScaleType
        {
            ToTarget = 0,
            ScaleOut = 1,
        }
        
        public ScaleType scaleType;
        
        public override void Play()
        {
            
        }

        public override void PlayOnce(Action onComplete = null)
        {
            Sequence sequence = CreateSequence();
            sequence.Play();
        }

        public override Sequence CreateSequence()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.SetDelay(delayDuration);
            
            Vector3 targetScale = scale;

            if (scaleType == ScaleType.ScaleOut)
            {
                targetScale = targetTransform.localScale;
            }
            
            sequence.Append(
                targetTransform
                    .DOScale(targetScale, animationDuration)
                    .SetEase(ease)
            );
            
            return sequence;
        }
    }
}
