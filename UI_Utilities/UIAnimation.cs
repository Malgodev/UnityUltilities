using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Malgo.Utilities.UI
{
#if DOTWEEN
    public class UIAnimation : MonoBehaviour
    {
        [SerializeField] private bool playOnStart = true;

        [Space]
        [SerializeField] private List<AnimationType> uiAnimationType;
        [SerializeField] private Sequence sequence;


        [Header("Animation stat")]
        [SerializeField] private float animationDelay;
        [SerializeField] int loopCount;
        [SerializeField] LoopType loopType;

        [Serializable]
        struct MoveAnimation
        {
            public float duration;
            public Vector3 positionOffset;
            public Ease easeType;
        }

        [SerializeField] private MoveAnimation moveAnimationSettings;


        [Serializable]
        struct RotateAnimation
        {
            [Serializable]
            public enum RotateType
            {
                None,
                RotateLeft,
                RotateRight,
                RotateInOut,
            }

            public RotateType rotateType;

            public float duration;
            public Vector3 rotationOffset;
            public Ease easeType;
        }

        [SerializeField] private RotateAnimation rotateAnimationSettings;

        [Serializable]
        struct ScaleAnimation
        {
            [Serializable]
            public enum ScaleType
            {
                None,
                ZoomIn,
                ZoomOut,
                ZoomInOut,
            }
            public ScaleType scaleType;
            public float duration;
            public Vector3 scaleOffset;
            public Ease easeType;
        }

        [SerializeField] private ScaleAnimation scaleAnimationSettings;


        private void Start()
        {
            sequence = DOTween.Sequence();

            sequence.Pause();
            sequence.SetAutoKill(false);

            Vector3 basePosition = transform.localPosition;
            Vector3 baseRotation = transform.localRotation.eulerAngles;
            Vector3 baseScale = transform.localScale;

            if (uiAnimationType.Contains(AnimationType.Move))
            {
                sequence.Join
                    (transform.DOLocalMove(basePosition + moveAnimationSettings.positionOffset, moveAnimationSettings.duration)
                    .SetEase(moveAnimationSettings.easeType));
            }

            if (uiAnimationType.Contains(AnimationType.Rotate))
            {
                Vector3 targetRotation;
                float duration = rotateAnimationSettings.duration;

                if (rotateAnimationSettings.rotateType == RotateAnimation.RotateType.RotateInOut)
                {
                    duration /= 4f;
                }

                if (rotateAnimationSettings.rotateType == RotateAnimation.RotateType.RotateRight)
                {
                    targetRotation = baseRotation - rotateAnimationSettings.rotationOffset;
                }
                else
                {
                    targetRotation = baseRotation + rotateAnimationSettings.rotationOffset;
                }

                sequence.Join
                    (transform.DOLocalRotate(targetRotation, duration)
                    .SetEase(rotateAnimationSettings.easeType));
            }


            if (uiAnimationType.Contains(AnimationType.Scale))
            {
                Vector3 targetScale;
                float duration = scaleAnimationSettings.duration;

                if (scaleAnimationSettings.scaleType == ScaleAnimation.ScaleType.ZoomInOut)
                {
                    duration /= 2f;
                }

                if (scaleAnimationSettings.scaleType == ScaleAnimation.ScaleType.ZoomIn)
                {
                    targetScale = baseScale - scaleAnimationSettings.scaleOffset;
                }
                else
                {
                    targetScale = baseScale + scaleAnimationSettings.scaleOffset;
                }

                sequence.Join
                    (transform.DOScale(targetScale, duration)
                    .SetEase(scaleAnimationSettings.easeType));
            }


            // Add remaining animations for InOut types
            // Get the current sequence duration to insert second parts at the same time
            float firstPartEndTime = sequence.Duration();

            if (uiAnimationType.Contains(AnimationType.Rotate)
                && rotateAnimationSettings.rotateType == RotateAnimation.RotateType.RotateInOut)
            {
                Vector3 targetRotation = baseRotation - rotateAnimationSettings.rotationOffset;
                float duration = rotateAnimationSettings.duration / 2f;

                sequence.Insert(firstPartEndTime,
                    transform.DOLocalRotate(targetRotation, duration)
                    .SetEase(rotateAnimationSettings.easeType));

                sequence.Insert(firstPartEndTime + duration,
                    transform.DOLocalRotate(baseRotation, duration / 2f)
                    .SetEase(rotateAnimationSettings.easeType));
            }

            if (uiAnimationType.Contains(AnimationType.Scale)
                && scaleAnimationSettings.scaleType == ScaleAnimation.ScaleType.ZoomInOut)
            {
                Vector3 targetScale;
                float duration = scaleAnimationSettings.duration / 2f;

                // For the second part of InOut, we go to the opposite direction
                if (scaleAnimationSettings.scaleType == ScaleAnimation.ScaleType.ZoomInOut)
                {
                    // First part was ZoomOut (baseScale + offset), now do ZoomIn (baseScale - offset)
                    targetScale = baseScale - scaleAnimationSettings.scaleOffset;
                }
                else
                {
                    targetScale = baseScale + scaleAnimationSettings.scaleOffset;
                }

                sequence.Insert(firstPartEndTime,
                    transform.DOScale(targetScale, duration)
                    .SetEase(scaleAnimationSettings.easeType));
            }

            sequence.SetDelay(animationDelay);

            sequence.SetLoops(loopCount, loopType);

            if (playOnStart)
            {
                PlayAnimation();
            }
        }

        public void PlayOnce()
        {
            if (sequence != null && sequence.IsActive())
            {
                // Temporarily store original loop settings
                int originalLoopCount = loopCount;
                LoopType originalLoopType = loopType;

                // Set to play only once
                sequence.SetLoops(1, LoopType.Restart);

                // Restart and play the sequence
                sequence.Restart();
                sequence.Play();

                // Restore original loop settings after completion
                sequence.OnComplete(() =>
                {
                    sequence.SetLoops(originalLoopCount, originalLoopType);
                });
            }
        }

        public void PlayAnimation()
        {
            sequence.Restart();

            sequence.Play();
        }

        public void CancelAnimations()
        {
            sequence.Pause();
        }

        private void OnDisable()
        {
            //sequence.Kill();
        }
    }

    public enum AnimationType
    {
        None,
        Move,
        Rotate,
        Scale,
    }
#endif
}