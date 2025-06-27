using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Malgo.Utilities.UI
{
    public class ToggleSwitchComponentGroupManager : MonoBehaviour
    {
        [Header("Start Value")]
        [SerializeField] private ToggleSwitchComponent initialToggleSwitchComponent;

        [Header("Toggle Options")]
        [SerializeField] private bool allCanBeToggledOff;

        private List<ToggleSwitchComponent> _ToggleSwitchComponents = new List<ToggleSwitchComponent>();

        private void Awake()
        {
            ToggleSwitchComponent[] ToggleSwitchComponentes = GetComponentsInChildren<ToggleSwitchComponent>();
            foreach (var ToggleSwitchComponent in ToggleSwitchComponentes)
            {
                RegisterToggleButtonToGroup(ToggleSwitchComponent);
            }
        }

        private void RegisterToggleButtonToGroup(ToggleSwitchComponent ToggleSwitchComponent)
        {
            if (_ToggleSwitchComponents.Contains(ToggleSwitchComponent))
                return;

            _ToggleSwitchComponents.Add(ToggleSwitchComponent);

            ToggleSwitchComponent.SetupForManager(this);
        }

        private void Start()
        {
            bool areAllToggledOff = true;
            foreach (var button in _ToggleSwitchComponents)
            {
                if (!button.CurrentValue)
                    continue;

                areAllToggledOff = false;
                break;
            }

            if (!areAllToggledOff || allCanBeToggledOff)
                return;

            if (initialToggleSwitchComponent != null)
                initialToggleSwitchComponent.ToggleByGroupManager(true);
            else
                _ToggleSwitchComponents[0].ToggleByGroupManager(true);
        }

        public void ToggleGroup(ToggleSwitchComponent ToggleSwitchComponent)
        {
            if (_ToggleSwitchComponents.Count <= 1)
                return;

            if (allCanBeToggledOff && ToggleSwitchComponent.CurrentValue)
            {
                foreach (var button in _ToggleSwitchComponents)
                {
                    if (button == null)
                        continue;

                    button.ToggleByGroupManager(false);
                }
            }
            else
            {
                foreach (var button in _ToggleSwitchComponents)
                {
                    if (button == null)
                        continue;

                    if (button == ToggleSwitchComponent)
                        button.ToggleByGroupManager(true);
                    else
                        button.ToggleByGroupManager(false);
                }
            }
        }
    }
}