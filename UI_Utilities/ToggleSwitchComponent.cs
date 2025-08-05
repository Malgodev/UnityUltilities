using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Credit to Christina Creates Games

namespace Malgo.Utilities.UI
{
    public class ToggleSwitchComponent : MonoBehaviour, IPointerClickHandler
    {
        [Header("Slider setup")]
        
        [SerializeField, Range(0, 1f)]
        protected float minValue;

        [SerializeField, Range(0, 1f)]
        protected float maxValue;

        [SerializeField, Range(0, 1f)]
        protected float sliderValue;
        public bool CurrentValue { get; private set; }

        private bool _previousValue;
        private Slider _slider;

        [Header("Animation")]
        [SerializeField, Range(0, 1f)] private float animationDuration = 0.5f;
        [SerializeField]
        private AnimationCurve slideEase =
            AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Coroutine _animateSliderCoroutine;

        [Header("Events")]
        [SerializeField] private UnityEvent onToggleOn;
        [SerializeField] private UnityEvent onToggleOff;

        private ToggleSwitchComponentGroupManager _toggleSwitchGroupManager;

        public event Action OnToggleClicked;
        protected Action transitionEffect;

        protected virtual void OnValidate()
        {
            SetupToggleComponents();

            _slider.value = sliderValue;
        }

        private void SetupToggleComponents()
        {
            if (_slider != null)
                return;

            SetupSliderComponent();
        }

        private void SetupSliderComponent()
        {
            _slider = GetComponent<Slider>();

            if (_slider == null)
            {
                Debug.Log("No slider found!", this);
                return;
            }

            _slider.interactable = false;
            var sliderColors = _slider.colors;
            sliderColors.disabledColor = Color.white;
            _slider.colors = sliderColors;
            _slider.transition = Selectable.Transition.None;
        }

        public void SetupForManager(ToggleSwitchComponentGroupManager manager)
        {
            _toggleSwitchGroupManager = manager;
        }


        protected virtual void Awake()
        {
            SetupSliderComponent();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Toggle();
            OnToggleChanged?.Invoke();
        }


        private void Toggle()
        {
            if (_toggleSwitchGroupManager != null)
                _toggleSwitchGroupManager.ToggleGroup(this);
            else
                SetStateAndStartAnimation(!CurrentValue);
        }

        public void SetToggleValue(bool targetValue)
        {
            _slider.value = sliderValue = targetValue ? maxValue : minValue;
            //_previousValue = !targetValue;
            CurrentValue = targetValue;
        }

        public void ToggleByGroupManager(bool valueToSetTo)
        {
            SetStateAndStartAnimation(valueToSetTo);
        }


        private void SetStateAndStartAnimation(bool state)
        {
            _previousValue = CurrentValue;
            CurrentValue = state;

            if (_previousValue != CurrentValue)
            {
                if (CurrentValue)
                    onToggleOn?.Invoke();
                else
                    onToggleOff?.Invoke();
            }

            if (_animateSliderCoroutine != null)
                StopCoroutine(_animateSliderCoroutine);

            _animateSliderCoroutine = StartCoroutine(AnimateSlider());
        }


        private IEnumerator AnimateSlider()
        {
            float startValue = _slider.value;
            float endValue = CurrentValue ? maxValue : minValue;

            float time = 0;
            if (animationDuration > 0)
            {
                while (time < animationDuration)
                {
                    time += Time.deltaTime;

                    float lerpFactor = slideEase.Evaluate(time / animationDuration);
                    _slider.value = sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);

                    transitionEffect?.Invoke();

                    yield return null;
                }
            }

            _slider.value = endValue;
        }
    }
}