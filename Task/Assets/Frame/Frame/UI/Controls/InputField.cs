// -----------------------------------------------------------------
// File:    InputField.cs
// Author:  fuzhun
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;

namespace GameBox.Service.UI
{
    class InputField : Element, IInputField
    {
        public InputField(string path, RectTransform transform)
            : base(path, transform, UIType.INPUT)
        {
            this.input = this.transform.GetComponent<UnityEngine.UI.InputField>();
        }

        public string Text
        {
            get {
                return this.input.text;
            }
            set {
                this.input.text = value;
            }
        }

        public int InputType
        {
            get {
                return (int)this.input.inputType;
            }
            set {
                this.input.inputType = (UnityEngine.UI.InputField.InputType)value;
            }
        }

        public int KeyboardType
        {
            get {
                return (int)this.input.keyboardType;
            }
            set {
                this.input.keyboardType = (TouchScreenKeyboardType)value;
            }
        }

        public int LineType
        {
            get {
                return (int)this.input.lineType;
            }
            set {
                this.input.lineType = (UnityEngine.UI.InputField.LineType)value;
            }
        }

        private UnityEngine.UI.InputField input = null;
    }
}
