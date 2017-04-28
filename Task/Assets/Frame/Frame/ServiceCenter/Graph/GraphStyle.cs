// -----------------------------------------------------------------
// File:    GraphStyle.cs
// Author:  mouguangyi
// Date:    2016.06.30
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;

namespace GameBox.Framework
{
    /// <summary>
    /// @details Graph样式表。
    /// </summary>
    public sealed class GraphStyle
    {
        /// <summary>
        /// Service宽度。
        /// </summary>
        public static float ServiceWidth
        {
            get {
                return GraphStyle.serviceWidth;
            }
        }

        /// <summary>
        /// 深灰色。
        /// </summary>
        public static Color DarkGrayColor
        {
            get {
                return GraphStyle.darkGrayColor;
            }
        }

        /// <summary>
        /// 浅灰色。
        /// </summary>
        public static Color LightGrayColor
        {
            get {
                return GraphStyle.lightGrayColor;
            }
        }

        /// <summary>
        /// 红色。
        /// </summary>
        public static Color RedColor
        {
            get {
                return GraphStyle.redColor;
            }
        }

        /// <summary>
        /// 绿色。
        /// </summary>
        public static Color GreenColor
        {
            get {
                return GraphStyle.greenColor;
            }
        }

        /// <summary>
        /// 浅灰色纹理。
        /// </summary>
        public static Texture2D LightGrayTexture
        {
            get {
                return GraphStyle.lightGrayTexture;
            }
        }

        /// <summary>
        /// 红色纹理。
        /// </summary>
        public static Texture2D RedTexture
        {
            get {
                return GraphStyle.redTexture;
            }
        }

        /// <summary>
        /// 绿色纹理。
        /// </summary>
        public static Texture2D GreenTexture
        {
            get {
                return GraphStyle.greenTexture;
            }
        }

        /// <summary>
        /// 服务的灰色Box样式。
        /// </summary>
        public static GUIStyle ServiceGrayBox
        {
            get {
                return GraphStyle.serviceGrayBox;
            }
        }

        /// <summary>
        /// 浅灰色Box样式。
        /// </summary>
        public static GUIStyle LightGrayBox
        {
            get {
                return GraphStyle.lightGrayBox;
            }
        }

        /// <summary>
        /// 绿色Box样式。
        /// </summary>
        public static GUIStyle GreenBox
        {
            get {
                return GraphStyle.greenBox;
            }
        }

        /// <summary>
        /// 红色Box样式。
        /// </summary>
        public static GUIStyle RedBox
        {
            get {
                return GraphStyle.redBox;
            }
        }

        /// <summary>
        /// mini字体。
        /// </summary>
        public static GUIStyle MiniLabel
        {
            get {
                return GraphStyle.miniLabel;
            }
        }

        /// <summary>
        /// 小字体。
        /// </summary>
        public static GUIStyle SmallLabel
        {
            get {
                return GraphStyle.smallLabel;
            }
        }

        /// <summary>
        /// 中字体。
        /// </summary>
        public static GUIStyle MiddleLabel
        {
            get {
                return GraphStyle.middleLabel;
            }
        }

        /// <summary>
        /// 大字体。
        /// </summary>
        public static GUIStyle BigLabel
        {
            get {
                return GraphStyle.bigLabel;
            }
        }

        internal static void _Initialize()
        {
            if (GraphStyle.initialized) {
                return;
            }

            GraphStyle.serviceWidth = Mathf.Min(GraphStyle.serviceWidth, Mathf.Max(Screen.width, Screen.height) * 0.5f);

            GraphStyle.darkGrayColor = new Color(0.2f, 0.2f, 0.2f);

            GraphStyle.lightGrayColor = new Color(0.8f, 0.8f, 0.8f);

            GraphStyle.redColor = new Color(0.77f, 0f, 0f);

            GraphStyle.greenColor = new Color(0f, 0.65f, 0f);

            GraphStyle.lightGrayTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            GraphStyle.lightGrayTexture.SetPixel(0, 0, GraphStyle.lightGrayColor);
            GraphStyle.lightGrayTexture.Apply();

            GraphStyle.redTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            GraphStyle.redTexture.SetPixel(0, 0, GraphStyle.redColor);
            GraphStyle.redTexture.Apply();

            GraphStyle.greenTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            GraphStyle.greenTexture.SetPixel(0, 0, GraphStyle.greenColor);
            GraphStyle.greenTexture.Apply();

            var backTexture = new Texture2D(4, 4, TextureFormat.ARGB32, false);
            for (var i = 0; i < 4; ++i) {
                for (var j = 0; j < 4; ++j) {
                    if (0 == i || 3 == i || 0 == j || 3 == j) {
                        backTexture.SetPixel(i, j, GraphStyle.darkGrayColor);
                    } else {
                        backTexture.SetPixel(i, j, GraphStyle.lightGrayColor);
                    }
                }
            }
            backTexture.Apply();
            GraphStyle.serviceGrayBox = new GUIStyle(GUI.skin.box) {
                alignment = TextAnchor.MiddleRight,
                fontSize = 22,
                fontStyle = FontStyle.Bold,
                border = new RectOffset(2, 2, 2, 2),
                normal = { background = backTexture, textColor = Color.black }
            };

            GraphStyle.lightGrayBox = new GUIStyle(GUI.skin.box) {
                fontSize = 22,
                normal = { background = GraphStyle.lightGrayTexture, textColor = Color.black }
            };

            GraphStyle.redBox = new GUIStyle(GUI.skin.box) {
                fontSize = 22,
                normal = { background = GraphStyle.redTexture, textColor = Color.black }
            };

            GraphStyle.greenBox = new GUIStyle(GUI.skin.box) {
                fontSize = 22,
                normal = { background = GraphStyle.greenTexture, textColor = Color.black }
            };

            GraphStyle.miniLabel = new GUIStyle(GUI.skin.label) {
                fontSize = 16,
            };

            GraphStyle.smallLabel = new GUIStyle(GUI.skin.label) {
                fontSize = 18,
            };

            GraphStyle.middleLabel = new GUIStyle(GUI.skin.label) {
                fontSize = 20,
            };

            GraphStyle.bigLabel = new GUIStyle(GUI.skin.label) {
                fontSize = 22,
            };

            GraphStyle.initialized = true;
        }

        private static float serviceWidth = 600f;
        private static Color darkGrayColor;
        private static Color lightGrayColor;
        private static Color redColor;
        private static Color greenColor;
        private static Texture2D lightGrayTexture = null;
        private static Texture2D redTexture = null;
        private static Texture2D greenTexture = null;
        private static GUIStyle serviceGrayBox = null;
        private static GUIStyle lightGrayBox = null;
        private static GUIStyle redBox = null;
        private static GUIStyle greenBox = null;
        private static GUIStyle miniLabel = null;
        private static GUIStyle smallLabel = null;
        private static GUIStyle middleLabel = null;
        private static GUIStyle bigLabel = null;
        private static bool initialized = false;
    }
}