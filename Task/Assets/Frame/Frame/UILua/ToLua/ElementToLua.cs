// -----------------------------------------------------------------
// File:    ElementToLua.cs
// Author:  fuzhun
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Service.LuaRuntime;
using GameBox.Service.UI;
using System;

namespace GameBox.Service.UIToLua
{
    class ElementToLua
    {
        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaGetVisible);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaSetVisible);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaGetUserData);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaSetUserData);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaFind);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaCloneElement);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaGetIndex);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaMoveToFirst);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaMoveToLast);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaMoveTo);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaOnClick);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaOnDragging);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaGetPosition);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaSetPosition);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaGetScale);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaSetScale);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaSetEulerAngles);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaGetEulerAngles);
            luaRuntime.RegLuaBridgeFunction(ElementToLua.LuaAddAnimation);
        }

        [LuaBridge("element_getVisible", 1, 1)]
        public static void LuaGetVisible(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            if (null != element) {
                executer.PushResult(element.Visible);
            } else {
                executer.PushResult(false);
            }
        }

        [LuaBridge("element_setVisible", 2, 0)]
        public static void LuaSetVisible(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            if (null != element && null != parameters[1]) {
                element.Visible = (bool)parameters[1];
            }
        }

        [LuaBridge("element_getData", 1, 1)]
        public static void LuaGetUserData(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            if (null != element) {
                executer.PushResult(element.UserData);
            }
        }

        [LuaBridge("element_setData", 2, 0)]
        public static void LuaSetUserData(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            if (null != element) {
                element.UserData = parameters[1];
            }
        }

        [LuaBridge("element_find", 3, 1)]
        public static void LuaFind(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            if (null != element && null != parameters[1] && null != parameters[2]) {
                var path = parameters[1].ToString();
                var type = Convert.ToInt32(parameters[2]);
                var childElement = element.Find(path, type);
                if (null != childElement) {
                    executer.PushResult(childElement);
                    return;
                }
            }

            executer.PushResult(null);
        }

        [LuaBridge("element_clone", 2, 1)]
        public static void LuaCloneElement(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            if (null != element && null != parameters[1]) {
                var index = Convert.ToInt32(parameters[1]);
                executer.PushResult(element.Clone(index));
            } else {
                executer.PushResult(null);
            }
        }

        [LuaBridge("element_getIndex", 1, 1)]
        public static void LuaGetIndex(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            if (null != element) {
                executer.PushResult(element.Index);
            }
        }

        [LuaBridge("element_moveToFirst", 1, 0)]
        public static void LuaMoveToFirst(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            if (null != element) {
                element.MoveToFirst();
            }
        }

        [LuaBridge("element_moveToLast", 1, 0)]
        public static void LuaMoveToLast(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            if (null != element) {
                element.MoveToLast();
            }
        }

        [LuaBridge("element_moveTo", 2, 0)]
        public static void LuaMoveTo(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            if (null != element && null != parameters[1]) {
                element.MoveTo(Convert.ToInt32(parameters[1]));
            }
        }

        [LuaBridge("element_onClick", 2, 0)]
        public static void LuaOnClick(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            var handler = parameters[1];
            if (null != element) {
                element.OnClick(() =>
                {
                    if (null != handler) {
                        executer.Call(handler);
                    }
                });
            }
        }

        [LuaBridge("element_onDragging", 2, 0)]
        public static void LuaOnDragging(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            var handler = parameters[1];
            if (null != element) {
                element.OnDragging((x, y) =>
                {
                    if (null != handler) {
                        executer.Call(handler, new object[] { x, y });
                    }
                });
            }
        }

        [LuaBridge("element_getPosition", 1, 3)]
        public static void LuaGetPosition(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            if (null != element)
            {
                var pos = element.GetPosition();
                executer.PushResult(pos.x);
                executer.PushResult(pos.y);
                executer.PushResult(pos.z);
            }
        }

        [LuaBridge("element_setPosition", 4, 0)]
        public static void LuaSetPosition(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            var x = Convert.ToSingle(parameters[1]);
            var y = Convert.ToSingle(parameters[2]);
            var z = Convert.ToSingle(parameters[3]);
            if (null != element)
            {
                element.SetPosition(new UnityEngine.Vector3(x, y, z));
            }
        }

        [LuaBridge("element_getScale", 1, 3)]
        public static void LuaGetScale(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            if (null != element)
            {
                var pos = element.GetScale();
                executer.PushResult(pos.x);
                executer.PushResult(pos.y);
                executer.PushResult(pos.z);
            }
        }

        [LuaBridge("element_setScale", 4, 0)]
        public static void LuaSetScale(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            var x = Convert.ToSingle(parameters[1]);
            var y = Convert.ToSingle(parameters[2]);
            var z = Convert.ToSingle(parameters[3]);
            if (null != element)
            {
                element.SetScale(new UnityEngine.Vector3(x, y, z));
            }
        }

        [LuaBridge("element_getEuler", 1, 3)]
        public static void LuaGetEulerAngles(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            if (null != element)
            {
                var pos = element.GetEulerAngles();
                executer.PushResult(pos.x);
                executer.PushResult(pos.y);
                executer.PushResult(pos.z);
            }
        }

        [LuaBridge("element_setEuler", 4, 0)]
        public static void LuaSetEulerAngles(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            var x = Convert.ToSingle(parameters[1]);
            var y = Convert.ToSingle(parameters[2]);
            var z = Convert.ToSingle(parameters[3]);
            if (null != element)
            {
                element.SetEulerAngles(new UnityEngine.Vector3(x, y, z));
            }
        }

        [LuaBridge("element_addAnimation", 2, 0)]
        public static void LuaAddAnimation(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var element = parameters[0] as IElement;
            var json = parameters[1].ToString();
            if (null != element)
            {
                var duration = parameters.Length > 2 ? Convert.ToSingle(parameters[2]) : 1.0f;
                var forever = parameters.Length > 3 ? Convert.ToBoolean(parameters[3]) : false;
                var relative = parameters.Length > 4 ? Convert.ToBoolean(parameters[4]) : false;
                var handler = parameters.Length > 5 ? parameters[5] : null;
                element.Animate(json, duration, forever, relative, delegate()
                {
                    if(null != handler)
                    {
                        executer.Call(handler);
                    }
                });
            }
        }
    }
}