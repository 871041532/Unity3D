// -----------------------------------------------------------------
// File:    Element.cs
// Author:  mouguangyi
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameBox.Service.UI
{
    public class Element : C0, IElement
    {
        public Element(string path, RectTransform transform, int type = UIType.ELEMENT)
        {
            this.path = path;
            this.transform = transform;
            this.type = type;
        }

        public override void Dispose()
        {
            this.transform = null;

            base.Dispose();
        }

        public virtual string Path
        {
            get
            {
                return this.path;
            }
        }

        public bool Visible
        {
            get
            {
                return this.transform.gameObject.activeInHierarchy;
            }
            set
            {
                this.transform.gameObject.SetActive(value);
            }
        }

        public object UserData
        {
            get
            {
                _InitUserData();

                return this.userData.Data;
            }
            set
            {
                _InitUserData();

                this.userData.Data = value;
            }
        }

        public int Index
        {
            get
            {
                return this.transform.GetSiblingIndex();
            }
        }

        public IElement Find(string path, int type)
        {
            path = (UIType.WINDOW == this.type ? path : this.path + "/" + path);
            return this.window._FindElement(path, type);  // Compose full path
        }

        public IElement Clone(int index)
        {
            var go = this.transform.gameObject;
            var clone = GameObject.Instantiate(go) as GameObject;
            clone.name = go.name + "#" + index;
            var transform = clone.transform as RectTransform;
            transform.SetParent(this.transform.parent);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;

            return this.window._FindElement(this.path + "#" + index, this.type);
        }

        public virtual void OnClick(Action handler)
        {
            if (null == this.trigger)
            {
                this.trigger = new ElementTrigger(this.transform);
            }

            this.trigger.AddClickHandler(handler);
        }

        public virtual void OnDragging(Action<float, float> handler)
        {
            if (null == this.trigger)
            {
                this.trigger = new ElementTrigger(this.transform);
            }

            this.trigger.AddDraggingHandler(handler);
        }

        public void MoveTo(int index)
        {
            this.transform.SetSiblingIndex(index);
        }

        public void MoveToFirst()
        {
            this.transform.SetAsFirstSibling();
        }

        public void MoveToLast()
        {
            this.transform.SetAsLastSibling();
        }

        public void SetPosition(Vector3 pos)
        {
            this.transform.anchoredPosition = pos;
        }

        public Vector3 GetPosition()
        {
            return this.transform.anchoredPosition;
        }

        public void SetScale(Vector3 scale)
        {
            this.transform.localScale = scale;
        }

        public Vector3 GetScale()
        {
            return this.transform.localScale;
        }

        public void SetEulerAngles(Vector3 eulerAngles)
        {
            this.transform.localEulerAngles = eulerAngles;
        }

        public Vector3 GetEulerAngles()
        {
            return this.transform.localEulerAngles;
        }

        internal virtual void _Reset()
        {
            if (null != this.trigger)
            {
                this.trigger._Reset();
                this.trigger = null;
            }
        }

        private void _InitUserData()
        {
            if (null == this.userData)
            {
                this.userData = this.transform.GetComponent<ElementUserData>();
                if (null == this.userData)
                {
                    this.userData = this.transform.gameObject.AddComponent<ElementUserData>();
                }
            }
        }

        //"{position:{x:20,y:30;z:50}, scale:1.5, alpha:0.5, rotate:{x:20,y:30;z:50}, blink:2}"`
        //duration 动画经历时间
        //forever 0 播放完结束  1 循环播放
        //relative 相对动画 1:相对 0：绝对 (如果为 闪烁 淡入淡出 则此值无效)
        public void Animate(string json, float duration = 1.0f, bool forever = false, bool relative = false, Action callBack = null)
        {
            List<AnimationBase> animations = new List<AnimationBase>();
            //var obj = SimpleJson.SimpleJson.DeserializeObject<AnimationObject>(json);
         //   if (obj.blink > 0)
         //   {
            //    animations.Add(new AnimationBlink(duration, obj.blink));
          //  }
         //   if (obj.alpha >= 0.0f)
         //   {
          //      animations.Add(new AnimationFade(duration, obj.alpha));
       //     }
        //    if (!relative)
         //   {
           //     if (obj.position != null)
            //    {
            //        if(obj.bezier_position != null && obj.bezier_position.Length > 0)
            //        {
            //            if(obj.bezier_position.Length == 1)
            //            {
            //                animations.Add(new AnimationBezierMoveTo(duration, obj.bezier_position[0].ToVector3(), obj.position.ToVector3()));
            //            }
            //            else if(obj.bezier_position.Length == 2)
            //            {
            //                animations.Add(new AnimationBezierMoveTo(duration, obj.bezier_position[0].ToVector3(), obj.bezier_position[1].ToVector3(), obj.position.ToVector3()));
            //            }
            //        }
            //        else if(obj.shake_position_modulus > 0)
            //        {
            //            animations.Add(new AnimationShakeMoveTo(duration, obj.position.ToVector3(), obj.shake_position_modulus));
            //        }
            //        else
            //        {
            //            animations.Add(new AnimationMoveTo(duration, obj.position.ToVector3()));
            //        }
            //    }
            //    if (obj.rotation != 0.0f)
            //    {
            //        animations.Add(new AnimationRotateTo(duration, new Vector3(0,0, obj.rotation)));
            //    }
            //    if (obj.scale >= 0.0f)
            //    {
            //        if (obj.bezier_scale != null && obj.bezier_scale.Length > 0)
            //        {
            //            if (obj.bezier_scale.Length == 1 && obj.bezier_scale[0] >= 0.0f)
            //            {
            //                animations.Add(new AnimationBezierScaleTo(duration, obj.bezier_scale[0], obj.scale));
            //            }
            //            else if (obj.bezier_position.Length == 2 && obj.bezier_scale[0] >= 0.0f && obj.bezier_scale[1] >= 0.0f)
            //            {
            //                animations.Add(new AnimationBezierScaleTo(duration, obj.bezier_scale[0], obj.bezier_scale[1], obj.scale));
            //            }
            //        }
            //        else if (obj.shake_scale_modulus > 0)
            //        {
            //            animations.Add(new AnimationShakeScaleTo(duration, obj.scale, obj.shake_scale_modulus));
            //        }
            //        else
            //        {
            //            animations.Add(new AnimationScaleTo(duration, obj.scale));
            //        }  
            //    }
            //}
            //else
            //{
            //    if (obj.position != null)
            //    {
            //        if (obj.bezier_position != null && obj.bezier_position.Length > 0)
            //        {
            //            if (obj.bezier_position.Length == 1)
            //            {
            //                animations.Add(new AnimationBezierMoveBy(duration, obj.bezier_position[0].ToVector3(), obj.position.ToVector3()));
            //            }
            //            else if (obj.bezier_position.Length == 2)
            //            {
            //                animations.Add(new AnimationBezierMoveBy(duration, obj.bezier_position[0].ToVector3(), obj.bezier_position[1].ToVector3(), obj.position.ToVector3()));
            //            }
            //        }
            //        else if (obj.shake_position_modulus > 0)
            //        {
            //            animations.Add(new AnimationShakeMoveBy(duration, obj.position.ToVector3(), obj.shake_position_modulus));
            //        }
            //        else
            //        {
            //            animations.Add(new AnimationMoveBy(duration, obj.position.ToVector3()));
            //        }
            //    }
            //    if (obj.rotation != 0.0f)
            //    {
            //        animations.Add(new AnimationRotateBy(duration, new Vector3(0, 0, obj.rotation)));
            //    }
            //    if (obj.scale >= 0.0f)
            //    {
            //        if (obj.bezier_scale != null && obj.bezier_scale.Length > 0)
            //        {
            //            if (obj.bezier_scale.Length == 1 && obj.bezier_scale[0] >= 0.0f)
            //            {
            //                animations.Add(new AnimationBezierScaleBy(duration, obj.bezier_scale[0], obj.scale));
            //            }
            //            else if (obj.bezier_position.Length == 2 && obj.bezier_scale[0] >= 0.0f && obj.bezier_scale[1] >= 0.0f)
            //            {
            //                animations.Add(new AnimationBezierScaleBy(duration, obj.bezier_scale[0], obj.bezier_scale[1], obj.scale));
            //            }
            //        }
            //        else if(obj.shake_scale_modulus > 0)
            //        {
            //            animations.Add(new AnimationShakeScaleBy(duration, obj.scale, obj.shake_scale_modulus));
            //        }
            //        else
            //        {
            //            animations.Add(new AnimationScaleBy(duration, obj.scale));
            //        }
            //    }
            //}
            //if (animations.Count == 0)
            //{
            //    return;
            //}
            //if(obj.acceleration > 0.0f)
            //{
            //    for(int i=0;i< animations.Count; i++)
            //    {
            //        animations[i] = new AnimationEase(obj.acceleration, animations[i] as AnimationInterval);
            //    }
            //}
            //if (animations.Count == 1)
            //{
            //    if (!forever)
            //    {
            //        _AddAnimation(new AnimationSequence(animations[0], new AnimationCallback(callBack)));
            //    }
            //    else
            //    {
            //        _AddAnimation(new AnimationForever(new AnimationSequence(animations[0], new AnimationCallback(callBack))));

            //    }
            //}
            //else
            //{
            //    if (!forever)
            //    {
            //        _AddAnimation(new AnimationSequence(new AnimationSpawn(animations), new AnimationCallback(callBack)));
            //    }
            //    else
            //    {
            //        _AddAnimation(new AnimationForever(new AnimationSequence(new AnimationSpawn(animations), new AnimationCallback(callBack))));
            //    }
            //}
        }

        public void ClearAnimation()
        {
            _ClearAnimations();
        }

        private void _AddAnimation(AnimationBase animation)
        {
            this.animations.Add(animation);
            animation.Transform = transform;
            animation.Start();
            this.window.AddAnimationElement(this);
        }

        private void _ClearAnimations()
        {
            this.window.RemoveAnimationElement(this);
            this.animations.Clear();
        }

        public virtual void Update(float delta)
        {
            if (this.animations.Count <= 0)
            {
                ClearAnimation();
                return;
            }
            for (int i = 0; i < this.animations.Count; i++)
            {
                if (!this.animations[i].IsDone())
                {
                    this.animations[i].Step(delta);
                }
                else
                {
                    this.animations.RemoveAt(i);
                    break;
                }
            }
        }

        // - EventTrigger wrapper
        private class ElementTrigger
        {
            public ElementTrigger(RectTransform transform)
            {
                this.trigger = transform.GetComponent<EventTrigger>();
                EventTriggerUtility.Clear(this.trigger);
            }

            public void AddClickHandler(Action handler)
            {
                if (null == this.clickSupport)
                {
                    this.clickSupport = new ClickSupport(this.trigger);
                }

                this.clickSupport.AddHandler(handler);
            }

            public void AddDraggingHandler(Action<float, float> handler)
            {
                if (null == this.draggingSupport)
                {
                    this.draggingSupport = new DraggingSupport(this.trigger);
                }

                this.draggingSupport.AddHandler(handler);
            }

            internal void _Reset()
            {
                if (null != this.clickSupport)
                {
                    this.clickSupport.Reset();
                    this.clickSupport = null;
                }

                if (null != this.draggingSupport)
                {
                    this.draggingSupport.Reset();
                    this.draggingSupport = null;
                }
            }

            // Support click event
            private class ClickSupport
            {
                public ClickSupport(EventTrigger trigger)
                {
                    this.trigger = trigger;
                    EventTriggerUtility.Add(this.trigger, EventTriggerType.PointerClick, _OnPointerClick);
                }

                public void AddHandler(Action handler)
                {
                    this.handlers.Add(handler);
                }

                public void Reset()
                {
                    EventTriggerUtility.Remove(this.trigger, EventTriggerType.PointerClick, _OnPointerClick);
                }

                private void _OnPointerClick(BaseEventData data)
                {
                    for (var i = 0; i < this.handlers.Count; ++i)
                    {
                        this.handlers[i]();
                    }
                }

                private EventTrigger trigger = null;
                private List<Action> handlers = new List<Action>();
            }

            // Support dragging event
            private class DraggingSupport
            {
                public DraggingSupport(EventTrigger trigger)
                {
                    this.trigger = trigger;
                    EventTriggerUtility.Add(this.trigger, EventTriggerType.BeginDrag, _OnBeginDrag);
                    EventTriggerUtility.Add(this.trigger, EventTriggerType.EndDrag, _OnEndDrag);
                }

                public void AddHandler(Action<float, float> handler)
                {
                    this.handlers.Add(handler);
                }

                public void Reset()
                {
                    EventTriggerUtility.Remove(this.trigger, EventTriggerType.BeginDrag, _OnBeginDrag);
                    EventTriggerUtility.Remove(this.trigger, EventTriggerType.EndDrag, _OnEndDrag);
                }

                private void _OnBeginDrag(BaseEventData data)
                {
                    var eventData = data as PointerEventData;
                    beginPosition.x = eventData.position.x;
                    beginPosition.y = eventData.position.y;
                }

                private void _OnEndDrag(BaseEventData data)
                {
                    var eventData = data as PointerEventData;
                    var endPosition = eventData.position;
                    this.deltaPosition.x = endPosition.x - this.beginPosition.x;
                    this.deltaPosition.y = endPosition.y - this.beginPosition.y;
                    for (var i = 0; i < this.handlers.Count; ++i)
                    {
                        this.handlers[i](this.deltaPosition.x, this.deltaPosition.y);
                    }
                }

                private EventTrigger trigger = null;
                private Vector2 beginPosition = new Vector2();
                private Vector2 deltaPosition = new Vector2();
                private List<Action<float, float>> handlers = new List<Action<float, float>>();
            }

            private EventTrigger trigger = null;
            private ClickSupport clickSupport = null;
            private DraggingSupport draggingSupport = null;
        }

        private class ElementUserData : MonoBehaviour
        {
            public object Data { get; set; }
        }

        private string path = null;
        protected RectTransform transform = null;
        private int type = UIType.ELEMENT;
        private ElementTrigger trigger = null;
        internal Window window = null;
        private ElementUserData userData = null;
        private List<AnimationBase> animations = new List<AnimationBase>();
    }
}