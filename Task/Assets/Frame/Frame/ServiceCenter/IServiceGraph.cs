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
    /// @details Serviceͼ��ӿڡ�����������ʱչ�ֵ�ǰ����ĸ��ֵ��Ի�״̬���ݣ�����Ϊ�����ṩֱ�ӵ���Ϣ������IServiceGraph����OnGUI��ִ�У�֧�����е�IMGUI������
    /// </summary>
    public interface IServiceGraph
    {
        /// <summary>
        /// Graph��ȡ�ServiceCenter��Ϊ�÷���Ԥ��Width��ȵĿռ䡣
        /// </summary>
        float Width { get; }

        /// <summary>
        /// Graph�߶ȡ�ServiceCenter��Ϊ�÷���Ԥ��Height�߶ȵĿռ䡣
        /// </summary>
        float Height { get; }

        /// <summary>
        /// ������ڡ�Service��Ҫ�ڸ÷������������ϣ��չ�ָ������ߵĸ������ݻ�״̬������ͨ���ı�չ�֣�����ͨ��ͼ��չ�ֵȵȡ�
        /// </summary>
        void Draw();
    }
}