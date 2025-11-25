using System;
using UnityEngine;

namespace Hapiga.FoodSort
{
    [Serializable]
    public class ObservableValue<T>
    {
        [SerializeField]
        private T value;

        public event Action<T> OnValueChanged;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }

        public ObservableValue(T initialValue = default(T))
        {
            value = initialValue;
        }
    }
}