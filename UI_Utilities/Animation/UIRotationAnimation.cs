using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Malgo.Utilities.UI
{
    public class UIRotationAnimation : UIBaseAnimation
    {
        [Header("Rotation Settings")]
        [Tooltip("Target rotation in degrees on Z axis (UI rotation).")]
        public float targetRotationZ = 360f;

        [Tooltip("Whether to rotate relative to current rotation or to absolute value.")]
        public bool rotateRelative = true;

        [Tooltip("Target UI element to rotate.")]
        public RectTransform target;

        public override void Play()
        {
            Sequence sequence = CreateSequence();

            if (delayBetweenLoop > 0)
            {
                if (loopType == LoopType.Yoyo)
                {
                    sequence.AppendInterval(delayBetweenLoop);
                    sequence.SetLoops(loopCount * 2, LoopType.Yoyo);
                }
                else
                {
                    sequence.AppendInterval(delayBetweenLoop);
                    sequence.SetLoops(loopCount, LoopType.Restart);
                }
            }
            else
            {
                sequence.SetLoops(loopCount, loopType);
            }

            sequence.Play();
        }

        [Button]
        public override void PlayOnce(Action onComplete = null)
        {
            Sequence sequence = CreateSequence();
            sequence.OnComplete(() => onComplete?.Invoke());
            sequence.Play();
        }

        public override Sequence CreateSequence()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetDelay(delayDuration);

            if (target == null)
            {
                Debug.LogWarning($"{nameof(UIRotationAnimation)} has no target assigned.");
                return sequence;
            }

            // Determine end rotation
            float finalRotation = rotateRelative
                ? target.localEulerAngles.z + targetRotationZ
                : targetRotationZ;

            sequence.Append(
                target.DOLocalRotate(
                    new Vector3(0, 0, finalRotation),
                    animationDuration,
                    RotateMode.FastBeyond360
                ).SetEase(ease)
            );

            return sequence;
        }
    }
}