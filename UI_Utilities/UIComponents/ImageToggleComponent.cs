using System;
using UnityEngine;
using UnityEngine.UI;

namespace Malgo.Utilities.UI
{
    [RequireComponent(typeof(Image))]
    public class ImageToggleComponent : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite enableSprite;
        [SerializeField] private Sprite disableSprite;
        
        [SerializeField] private bool isEnabled;

        public bool IsEnabled => isEnabled;

        private void OnValidate()
        {
            SetEnabled(isEnabled);
        }

        private void Start()
        {
            SetEnabled(isEnabled);
        }
        
        public void SetEnabled(bool enabled)
        {
            isEnabled = enabled;
            
            image.sprite = isEnabled ? enableSprite : disableSprite;
        }
    }
}
