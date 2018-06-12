using UnityEngine;
using UnityEngine.UI;

namespace FrameWork
{
    public class IphoneXAdapter : Singleton<IphoneXAdapter>
    {
        public bool isIphoneX { get; set; }

        private void Check()
        {
            #if UNITY_IPHONE
            string device = SystemInfo.deviceModel;
            isIphoneX = device.Contains("iPhone10,3") || device.Contains("iPhone10,6");
            #endif
            if (!isIphoneX)
            {
                //2436*1125
                float screenScale = (float) Screen.width / (float) Screen.height;
                if (screenScale > 2f)
                    isIphoneX = true;
            }
        }

        private void ApplySafeArea(RectTransform[] roots)
        {
            var area = Screen.safeArea;
            float xOffset = area.size.x * 44f / 812f;
            float yBottomOffset = area.size.y * 21f / 375f; 
            //positionOffset = new Vector2 (area.size.x * 44f / 812f, area.size.y * 21f / 375f);
            foreach (var rectTransform in roots)
            {
                rectTransform.offsetMin = new Vector2(xOffset,yBottomOffset);
                rectTransform.offsetMax = new Vector2(-xOffset,0);
            }
        }
        public void Adapt(CanvasScaler[] scalers,RectTransform[] roots)
        {
            Check();
            foreach (var scaler in scalers)
            {
                scaler.matchWidthOrHeight = isIphoneX ? 1f : 0f;
            }
            if(isIphoneX)
                ApplySafeArea(roots);
        }
    }
}