// -----------------------------------------------------------------
// File:    IServiceGraph.cs
// Author:  mouguangyi
// Date:    2016.06.30
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Framework
{
    /// <summary>
    /// @details Service图表接口。用来在运行时展现当前服务的各种调试或状态数据，用来为开发提供直接的信息帮助。IServiceGraph是在OnGUI中执行，支持所有的IMGUI操作。
    /// </summary>
    public interface IServiceGraph
    {
        /// <summary>
        /// Graph宽度。ServiceCenter会为该服务预留Width宽度的空间。
        /// </summary>
        float Width { get; }

        /// <summary>
        /// Graph高度。ServiceCenter会为该服务预留Height高度的空间。
        /// </summary>
        float Height { get; }

        /// <summary>
        /// 绘制入口。Service需要在该方法中完成所有希望展现给开发者的各种数据或状态，比如通过文本展现，或者通过图表展现等等。
        /// </summary>
        void Draw();
    }
}