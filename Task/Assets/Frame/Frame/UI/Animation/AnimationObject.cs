// -----------------------------------------------------------------
// File:    AnimationFadeOut.cs
// Author:  liuwei
// Date:    2017.02.21
// Description:
//      
// -----------------------------------------------------------------

using UnityEngine;

namespace GameBox.Service.UI
{
    /// <summary>
    /// 
    /// </summary>
    class AnimationObject
    {
        //位移
        public PositionV2 position;

        //大小
        public float scale = -1.0f;

        //欧拉角变换
        public float rotation = 0.0f;

        //淡入淡出
        public float alpha = -1.0f;

        //闪烁 次数
        public int blink = 0;

        //曲线位置
        public PositionV2[] bezier_position;

        //平滑大小变化
        public float[] bezier_scale;

        //位移摇摆系数 大于1则越摇幅度越大
        public float shake_position_modulus = 0.0f;

        //大小摇摆系数 大于1则越摇幅度越大
        public float shake_scale_modulus = 0.0f;

        //加速度 用于加速动画 类似于物理学里的加速度
        public float acceleration = 0.0f;
    }

    class PositionV2
    {
        public float x;
        public float y;

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, 0);
        }
    }
}