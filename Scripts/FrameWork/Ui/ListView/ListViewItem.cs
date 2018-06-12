using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork
{
    public abstract class ListViewItem : MonoBehaviour
    {
        #region Var
        public int Index = -1;
        public abstract Vector2 Size { get; }

        private RectTransform rectTransform;

        /// <summary>
        /// Gets the RectTransform.
        /// </summary>
        /// <value>The RectTransform.</value>
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = transform as RectTransform;
                }
                return rectTransform;
            }
        }
        #endregion
    }
}
