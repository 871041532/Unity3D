// -----------------------------------------------------------------
// File:    IInputField.cs
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
    public interface IInputField : IElement
    {
        /// <summary>
        /// 
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int InputType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int KeyboardType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int LineType { get; set; }
    }
}
