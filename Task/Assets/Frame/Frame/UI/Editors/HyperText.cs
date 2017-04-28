// -----------------------------------------------------------------
// File:    TextPic.cs
// Author:  liuwei
// Date:    2017.02.04
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine.Events;

[Serializable]
public class HrefClickEvent : UnityEvent<string> { }

namespace GameBox.Service.UI.Extension
{
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using GameBox.Framework;
    using GameBox.Service.AssetManager;

    [RequireComponent(typeof(ContentSizeFitter))]
    public class HyperText : Text, IPointerClickHandler
    {
        public string OriginalHrefColor = "#00BFFF";
        public string ClickHrefColor = "#FF0000";

        /// <summary>
        /// 超链接点击事件
        /// </summary>
        public HrefClickEvent OnHrefClick
        {
            get { return onHrefClick; }
            set { onHrefClick = value; }
        }

        protected override void Start()
        {
            base.Start();
            var contentSizeFitter = this.GetComponent<ContentSizeFitter>();
            if (null != contentSizeFitter)
            {
                contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
        }

        public override float preferredWidth
        {
            get
            {
                var settings = GetGenerationSettings(Vector2.zero);
                return cachedTextGeneratorForLayout.GetPreferredWidth(_GetText(), settings) / pixelsPerUnit;
            }
        }

        public override float preferredHeight
        {
            get
            {
                var settings = GetGenerationSettings(new Vector2(rectTransform.rect.size.x, 0.0f));
                return cachedTextGeneratorForLayout.GetPreferredHeight(_GetText(), settings) / pixelsPerUnit;
            }
        }

        public override string text
        {
            get
            {
                return m_Text;
            }
            set
            {
                this.clickHrefFlag = false;
                if (String.IsNullOrEmpty(value))
                {
                    if (String.IsNullOrEmpty(m_Text))
                        return;
                    m_Text = "";
                    SetVerticesDirty();
                }
                else if (m_Text != value)
                {
                    m_Text = value;
                    SetVerticesDirty();
                    SetLayoutDirty();
                }
            }
        }

        public override void SetVerticesDirty()
        {
            base.SetVerticesDirty();
            UpdateQuadImage();
        }

        protected void UpdateQuadImage()
        {
#if UNITY_EDITOR
            if (UnityEditor.PrefabUtility.GetPrefabType(this) == UnityEditor.PrefabType.Prefab)
            {
                return;
            }
#endif
            this.outputText = GetOutputText();
            this.imagesVertexIndex.Clear();
            this.removeEmotionTexts.Clear();
            foreach (Match match in this.emotionRegex.Matches(this.outputText))
            {
                var picIndex = match.Index;
                var endIndex = picIndex * 4 + 3;
                this.imagesVertexIndex.Add(endIndex);
                this.imagesPool.RemoveAll(image => image == null);
                if (this.imagesPool.Count == 0)
                {
                    GetComponentsInChildren<Image>(this.imagesPool);
                }
                if (this.imagesVertexIndex.Count > this.imagesPool.Count)
                {
                    var resources = new DefaultControls.Resources();
                    var go = DefaultControls.CreateImage(resources);
                    go.layer = this.gameObject.layer;
                    var rt = go.transform as RectTransform;
                    if (rt)
                    {
                        rt.SetParent(this.rectTransform);
                        rt.localPosition = Vector3.zero;
                        rt.localRotation = Quaternion.identity;
                        rt.localScale = Vector3.one;
                    }
                    this.imagesPool.Add(go.GetComponent<Image>());
                }

                this.removeEmotionTexts.Add(new RemoveText(match.Groups[0].Value, "情"));
                string[] spriteInfo = match.Groups[1].Value.Split('|');
                var spritePath = spriteInfo[0];
                var spriteName = spriteInfo[1];
                var emotionName = spriteInfo[2];
                var saveType = int.Parse(match.Groups[2].Value);
                var frameNum = int.Parse(match.Groups[3].Value);
                var size = this.fontSize;
                //var size = float.Parse(match.Groups[2].Value);
                if (saveType == 0)
                {
                    string[] spriteArray = spritePath.Split('/');
                }

                var img = this.imagesPool[this.imagesVertexIndex.Count - 1];

                if (img.sprite == null || !img.name.Equals(emotionName))
                {
                    if (frameNum > 1)
                    {
                        ImageAnimation imageAnimation = img.GetComponent<ImageAnimation>();
                        if (imageAnimation == null)
                            imageAnimation = img.gameObject.AddComponent<ImageAnimation>();
                        imageAnimation.FramesPerSecond = 10;
                        List<Sprite> sprites = new List<Sprite>();
                        if (saveType == 0)
                        {
                            img.sprite = _LoadSpriteFromAtlas(string.Format(spritePath, 1), string.Format(spriteName, 1));
                            for (int i = 0; i < frameNum; i++)
                            {
                                var sprite = _LoadSpriteFromAtlas(string.Format(spritePath, (i + 1)), string.Format(spriteName, (i + 1)));
                                if (null != sprite) {
                                    sprites.Add(sprite);
                                }
                            }
                        }
                        else
                        {
                            img.sprite = _LoadSpriteFromAtlas(spritePath, string.Format(spriteName, 1));
                            for (int i = 0; i < frameNum; i++)
                            {
                                var sprite = _LoadSpriteFromAtlas(spritePath, string.Format(spriteName, (i + 1)));
                                if (null != sprite)
                                    sprites.Add(sprite);
                            }
                        }

                        imageAnimation.Sprites = sprites;
                    }
                    else if (frameNum == 1)
                    {
                        ImageAnimation uiImageAni = img.GetComponent<ImageAnimation>();
                        if (uiImageAni != null)
                            GameObject.Destroy(uiImageAni);
                        if (saveType == 0)
                        {
                            img.sprite = _LoadSpriteFromAtlas(string.Format(spritePath, 1), string.Format(spriteName, 1));
                        }
                        else
                        {
                            img.sprite = _LoadSpriteFromAtlas(spritePath, string.Format(spriteName, 1));
                        }
                    }
                }
                img.rectTransform.sizeDelta = new Vector2(size, size);
                img.enabled = true;
                img.name = emotionName;
            }

            for (var i = this.imagesVertexIndex.Count; i < this.imagesPool.Count; i++)
            {
                if (this.imagesPool[i])
                {
                    this.imagesPool[i].enabled = false;
                }
            }
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            var orignText = m_Text;
            this.m_Text = this.outputText;
            base.OnPopulateMesh(toFill);
            this.m_Text = orignText;

            UIVertex vert = new UIVertex();
            for (var i = 0; i < this.imagesVertexIndex.Count; i++)
            {
                var endIndex = this.imagesVertexIndex[i];
                var rt = this.imagesPool[i].rectTransform;
                var size = rt.sizeDelta;
                if (endIndex < toFill.currentVertCount)
                {
                    toFill.PopulateUIVertex(ref vert, endIndex);
                    rt.anchoredPosition = new Vector2(vert.position.x + size.x / 2, vert.position.y + size.y / 2 - 4);

                    // 抹掉左下角的小黑点
                    toFill.PopulateUIVertex(ref vert, endIndex - 3);
                    var pos = vert.position;
                    for (int j = endIndex, m = endIndex - 3; j > m; j--)
                    {
                        toFill.PopulateUIVertex(ref vert, endIndex);
                        vert.position = pos;
                        toFill.SetUIVertex(vert, j);
                    }
                }
            }

            if (this.imagesVertexIndex.Count != 0)
            {
                this.imagesVertexIndex.Clear();
            }

            // 处理超链接包围框
            foreach (var hrefInfo in this.hrefInfos)
            {
                hrefInfo.boxes.Clear();
                if (hrefInfo.startIndex >= toFill.currentVertCount)
                {
                    continue;
                }

                // 将超链接里面的文本顶点索引坐标加入到包围框
                toFill.PopulateUIVertex(ref vert, hrefInfo.startIndex);
                var pos = vert.position;
                var bounds = new Bounds(pos, Vector3.zero);
                for (int i = hrefInfo.startIndex, m = hrefInfo.endIndex; i < m; i++)
                {
                    if (i >= toFill.currentVertCount)
                    {
                        break;
                    }

                    toFill.PopulateUIVertex(ref vert, i);
                    pos = vert.position;
                    if (pos.x < bounds.min.x) // 换行重新添加包围框
                    {
                        hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
                        bounds = new Bounds(pos, Vector3.zero);
                    }
                    else
                    {
                        bounds.Encapsulate(pos); // 扩展包围框
                    }
                }
                hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
            }
        }

        /// <summary>
        /// 获取超链接解析后的最后输出文本
        /// </summary>
        /// <returns></returns>
        protected string GetOutputText()
        {
            textBuilder.Length = 0;
            var indexText = 0;
            if (this.clickHrefFlag)
            {
                var indexHrefInfo = 0;
                foreach (Match match in hrefRegex.Matches(this.text))
                {
                    var hrefInfo = this.hrefInfos[indexHrefInfo];
                    textBuilder.Append(this.text.Substring(indexText, match.Index - indexText));
                    textBuilder.Append(string.Format("<color={0}>", hrefInfo.HrefColor));  // 超链接颜色

                    textBuilder.Append(match.Groups[2].Value);
                    textBuilder.Append("</color>");
                    indexText = match.Index + match.Length;
                    indexHrefInfo++;
                }
            }
            else
            {
                this.hrefInfos.Clear();
                this.removeHrefTexts.Clear();
                foreach (Match match in hrefRegex.Matches(this.text))
                {
                    textBuilder.Append(this.text.Substring(indexText, match.Index - indexText));
                    textBuilder.Append(string.Format("<color={0}>", this.OriginalHrefColor));  // 超链接颜色

                    var group = match.Groups[1];
                    var hrefInfo = new HrefInfo(this)
                    {
                        startIndex = textBuilder.Length * 4, // 超链接里的文本起始顶点索引
                        endIndex = (textBuilder.Length + match.Groups[2].Length - 1) * 4 + 3,
                        name = group.Value
                    };
                    this.hrefInfos.Add(hrefInfo);

                    textBuilder.Append(match.Groups[2].Value);
                    textBuilder.Append("</color>");
                    indexText = match.Index + match.Length;
                    this.removeHrefTexts.Add(new RemoveText(match.Groups[0].Value, match.Groups[2].Value));
                }
            }
            textBuilder.Append(text.Substring(indexText, text.Length - indexText));
            return textBuilder.ToString();
        }

        /// <summary>
        /// 点击事件检测是否点击到超链接文本
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            Vector2 lp;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, eventData.position, eventData.pressEventCamera, out lp);

            foreach (var hrefInfo in this.hrefInfos)
            {
                var boxes = hrefInfo.boxes;
                for (var i = 0; i < boxes.Count; ++i)
                {
                    if (boxes[i].Contains(lp))
                    {
                        hrefInfo.clickHrefFlag = true;
                        _ClickHref();
                        this.onHrefClick.Invoke(hrefInfo.name);
                        return;
                    }
                }
            }
        }

        private void _ClickHref()
        {
            this.clickHrefFlag = true;
            SetVerticesDirty();
        }

        private string _GetText()
        {
            var text = m_Text;
            for (int i = 0; i < this.removeHrefTexts.Count; i++)
            {
                text = text.Replace(this.removeHrefTexts[i].FromText, this.removeHrefTexts[i].ToText);
            }
            for (int i = 0; i < this.removeEmotionTexts.Count; i++)
            {
                text = text.Replace(this.removeEmotionTexts[i].FromText, this.removeEmotionTexts[i].ToText);
            }
            return text;
        }

        private Sprite _LoadSpriteFromAtlas(string spritePath, string spriteName)
        {
            using (var asset = ServiceCenter.GetService<IAssetManager>().Load(spritePath, AssetType.SPRITEATLAS)) {
                var sprites = asset.Cast<Sprite[]>();
                for (var i = 0; i < sprites.Length; ++i) {
                    if (sprites[i].name == spriteName) {
                        return sprites[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 图片池
        /// </summary>
        private readonly List<Image> imagesPool = new List<Image>();

        /// <summary>
        /// 图片的最后一个顶点的索引
        /// </summary>
        private readonly List<int> imagesVertexIndex = new List<int>();

        /// <summary>
        /// 正则取出所需要的属性
        /// </summary>
        private readonly Regex emotionRegex = new Regex(@"<quad path=(.+?) savetype=(\d*\.?\d+%?) framenum=(\d*\.?\d+%?) />", RegexOptions.Singleline);

        /// <summary>
        /// 解析完最终的文本
        /// </summary>
        private string outputText;

        private List<RemoveText> removeEmotionTexts = new List<RemoveText>();

        //-------------------------------------------------------------------

        private List<RemoveText> removeHrefTexts = new List<RemoveText>();

        private bool clickHrefFlag = false;

        /// <summary>
        /// 超链接信息列表
        /// </summary>
        private readonly List<HrefInfo> hrefInfos = new List<HrefInfo>();

        /// <summary>
        /// 文本构造器
        /// </summary>
        private static readonly StringBuilder textBuilder = new StringBuilder();

        /// <summary>
        /// 超链接正则
        /// </summary>
        private static readonly Regex hrefRegex = new Regex(@"<a href=([^>\n\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

        [SerializeField]
        private HrefClickEvent onHrefClick = new HrefClickEvent();



        /// <summary>
        /// 超链接信息类
        /// </summary>
        private class HrefInfo
        {
            public int startIndex;

            public int endIndex;

            public string name;

            public readonly List<Rect> boxes = new List<Rect>();

            private HyperText hyperText;

            public bool clickHrefFlag = false;
            public string HrefColor
            {
                get
                {
                    if (this.clickHrefFlag)
                    {
                        return this.hyperText.ClickHrefColor;
                    }
                    else
                    {
                        return this.hyperText.OriginalHrefColor;
                    }
                }
            }

            public HrefInfo(HyperText hyperText)
            {
                this.hyperText = hyperText;
            }
        }

        /// <summary>
        /// 移除字符串信息类
        /// </summary>
        public class RemoveText
        {
            public string FromText;
            public string ToText;

            public RemoveText(string fromText, string toText)
            {
                this.FromText = fromText;
                this.ToText = toText;
            }
        }
    }
}
