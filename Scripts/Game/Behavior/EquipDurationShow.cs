using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class EquipDurationShow : MonoBehaviour
    {
        private enum EquipParts
        {
            HAT = 1,
            CLOTH = 2,
            PANTS = 3,
            SHOES = 4,
            //POCKET = 6,//辅助位
            WEAPON = 7,//主武器位
        }

        [System.Serializable]
        public class durationImg
        {
            public Image normal;
            public Image damaged;
        }
        public durationImg[] images;

        public void ShowEquipImgs(bool active)
        {
            gameObject.SetActive(active);
        }

        public void ShowEquipDuration(int index, bool damaged)
        {
            var equipIndex = getEquipIndex(index);
            if (equipIndex > images.Length) return;
            images[equipIndex].normal.enabled = !damaged;
            images[equipIndex].damaged.enabled = damaged;
        }
        private int getEquipIndex(int index)
        {
            var resultIndex = --index;
            if ( index == (int)EquipPart.WEAPON)
            {
                resultIndex = 4;
            }

            return resultIndex;
        }
    }
}