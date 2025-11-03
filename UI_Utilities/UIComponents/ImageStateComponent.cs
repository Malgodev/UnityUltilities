using UnityEngine;
using UnityEngine.UI;

namespace Hapiga.FoodSort
{
    public class ImageStateComponent : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite[] stateSprites;
        
        [SerializeField] private int currentStateIndex = 0;

        public int CurrentStateIndex => currentStateIndex;

        private void OnValidate()
        {
            SetState(currentStateIndex);
        }

        private void Start()
        {
            SetState(currentStateIndex);
        }
        
        public void SetState(int state)
        {
            currentStateIndex = state;
            
            image.sprite = stateSprites[currentStateIndex];
        }
    }
}
