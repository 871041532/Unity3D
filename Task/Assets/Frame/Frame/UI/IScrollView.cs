// -----------------------------------------------------------------
// File:    IScrollView.cs
// Author:  fuzhun
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.UI
{
    /// <summary>
    /// 
    /// </summary>
    public interface IScrollView : IElement
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IElement[] GetItems(int type);

        /// <summary>
        /// 
        /// </summary>
        void ScrollToBottom();

        /// <summary>
        /// 
        /// </summary>
        void ScrollToTop();
    }
}
