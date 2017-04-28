// -----------------------------------------------------------------
// File:    ScrollView.cs
// Author:  mouguangyi
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace GameBox.Service.UI
{
    class ScrollView : Element, IScrollView
    {
        public ScrollView(string path, RectTransform transform)
            : base(path, transform, UIType.SCROLLVIEW)
        {
            this.scrollRect = this.transform.GetComponent<ScrollRect>();
        }

        public IElement[] GetItems(int type)
        {
            var content = this.transform.Find("Viewport/Content");
            var count = content.childCount;
            var elements = new IElement[count];
            for (var i = 0; i < count; ++i) {
                var transform = content.GetChild(i) as RectTransform;
                elements[i] = Find("Viewport/Content/" + transform.gameObject.name, type);
            }

            return elements;
        }

        public void ScrollToBottom()
        {
            new CompletedTask().Start().Continue(task =>
            {
                this.scrollRect.verticalNormalizedPosition = 0f;
                return null;
            });
        }

        public void ScrollToTop()
        {
            new CompletedTask().Start().Continue(task =>
            {
                this.scrollRect.verticalNormalizedPosition = 1.0f;
                return null;
            });
        }

        private ScrollRect scrollRect = null;
    }
}
