
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// @details ServiceCenter是所有服务的运行载体。它承载所有Service的初始化，运行和终止。\n
    /// ServiceCenter提供了获取指定Service的同步方法：\n
    /// @li @c 在获取服务前确保所有依赖的服务都已经装载
    /// @li @c 通过ServiceCenter直接同步获取需要的服务接口。
    /// 
    /// @section example 例子
    /// @code{.cs}
    /// var assetManager = ServiceCenter.GetService<IAssetManager>();
    /// @endcode
    /// 
    /// @code{.cs}
    /// var assetManager = ServiceCenter.GetService("com.giant.service.assetmanager") as IAssetManager;
    /// @endcode
    /// </summary>
    public class ServiceCenter
    {
        //public ServiceCenter()
        //{
        //    _Enter(null);
        //}
        internal void _Enter(IServiceHost host)
        {
            this.host = host;

            if (null != ServiceCenter.instance)
            {
                throw new Exception("Duplicated ServiceCenter component！");
            }

            ServiceCenter.instance = this;

            _Init();
        }

        internal void _Execute(float delta)
        {
            var count = this.runners.Count;
            for (var i = count - 1; i >= 0; --i)
            {
                this.runners[i].Pulse(delta);
            }

            if (this.isActionQueueEmpty)
            {
                return;
            }

            this.localActionQueue.Clear();
            lock (this.actionQueue)
            {
                this.localActionQueue.AddRange(this.actionQueue);
                this.actionQueue.Clear();
                this.isActionQueueEmpty = true;
            }

            for (var i = 0; i < this.localActionQueue.Count; ++i)
            {
                this.localActionQueue[i].Invoke();//跨线程调用（使用主线程调用委托）
            }
        }

        internal void _Exit()
        {
            for (var i = this.runners.Count - 1; i >= 0; --i)
            {
                this.runners[i].Shutdown();
            }

            this.runners = null;
            this.host = null;
            ServiceCenter.instance = null;

            //AnyLogger._GetInstance()._Dispose();
            //Console.Stop();
        }

        internal void _DrawSplash()
        {
            if (!this.ready)
            {
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.splashTexture, ScaleMode.StretchToFill);
            }
        }

        internal void _OnGraph()
        {
            GraphStyle._Initialize();

            ServiceRunner runner = null;
            float y = SERVICE_MARGIN;
            var count = this.runners.Count;
            for (var i = count - 1; i >= 0; --i)
            {
                runner = this.runners[i];
                if (GUI.Button(new Rect(SERVICE_MARGIN, y, GraphStyle.ServiceWidth, SERVICE_TITLE_HEIGHT), runner.Service.Id, GraphStyle.ServiceGrayBox) || i == this.graphIndex)
                {
                    this.graphIndex = i;
                    y += runner.DrawGraph(y + SERVICE_TITLE_HEIGHT);
                }
                y += SERVICE_TITLE_HEIGHT + SERVICE_MARGIN;
            }
        }

        internal void _RunOnGameThread(Action action)
        {
            if (null == action)
            {
                throw new ArgumentNullException("Action");
            }

            lock (this.actionQueue)//线程等待
            {
                this.actionQueue.Add(action);
                this.isActionQueueEmpty = false;
            }
        }

        internal void _RunAsyncTask(AsyncTask task)
        {
            _RunOnGameThread(() =>
            {
                this.host.RunAsyncTask(task);
            });
        }

        internal void _AddRunner(ServiceRunner serviceRunner)
        {
            var index = this.runners.FindIndex(delegate (ServiceRunner runner)
            {
                return (runner.Service == serviceRunner.Service || runner.Service.Id == serviceRunner.Service.Id);
            });
            if (index < 0)
            {
                this.runners.Add(serviceRunner);
            }
        }

        internal void _RemoveRunner(ServiceRunner serviceRunner)
        {
            _FireShutdown(serviceRunner);

            var index = this.runners.FindIndex(delegate (ServiceRunner runner)
            {
                return (runner.Service == serviceRunner.Service || runner.Service.Id == serviceRunner.Service.Id);
            });
            if (index >= 0)
            {
                this.runners.RemoveAt(index);
            }
        }

        internal ServiceRunner _GetRunnerByIndex(int index)
        {
            if (index < 0 || index >= this.runners.Count)
            {
                return null;
            }

            return this.runners[index];
        }

        internal IService _FindService(string id)
        {
            for (var i = 0; i < this.runners.Count; ++i)
            {
                if (this.runners[i].Service.Id == id && this.runners[i]._IsReady)
                {
                    return this.runners[i].Service;
                }
            }

            return null;
        }

        internal T _FindService<T>()
        {
            for (var i = 0; i < this.runners.Count; ++i)
            {
                if (this.runners[i].Service is T && this.runners[i]._IsReady)
                {
                    return (T)this.runners[i].Service;
                }
            }

            return default(T);
        }

        internal Color _SplashColor { get; set; }

        internal LogType _LogType { get; set; }

        internal LogLevel _LogLevel { get; set; }

        internal int _RunnerCount { get { return this.runners.Count; } }

        private void _Init()
        {
            if (_SplashColor.a > 0)
            {
                this.ready = false;
                this.splashTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                this.splashTexture.SetPixel(0, 0, _SplashColor);
                this.splashTexture.Apply();
            }

            AnyLogger._GetInstance()._Init();
        }

        private void _FireShutdown(ServiceRunner runner)
        {
            for (var i = 0; i < this.runners.Count; ++i)
            {
                if (this.runners[i] != runner)
                {
                    this.runners[i].NotifyShutdown(runner.Service);
                }
            }
        }

        private IServiceHost host = null;
        private bool ready = true;
        private Texture2D splashTexture = null;
        private bool isActionQueueEmpty = true;
        private List<Action> actionQueue = new List<Action>();
        private List<Action> localActionQueue = new List<Action>();
        private List<ServiceRunner> runners = new List<ServiceRunner>();
        private int graphIndex = -1;

        internal const float SERVICE_TITLE_HEIGHT = 60f;
        internal const float SERVICE_MARGIN = 1f;

        /// <summary>
        /// 通知ServiceCenter进入游戏，ServiceCenter关闭SplashColor。
        /// </summary>
        public static void Launch()
        {
            Center().ready = true;
        }

        /// <summary>
        /// 根据Service的Id获取Service实例接口。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IService GetService(string id)
        {
            return Center()._FindService(id);
        }

        /// <summary>
        /// 根据Service类型获取Service实例接口。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>() where T : IService
        {
            return Center()._FindService<T>();
        }

        /// <summary>
        /// 启动远程Debug，允许其他人通过游戏运行的设备IP和端口在浏览器中实时查看日志。
        /// </summary>
        /// <param name="port"></param>
        public static void RemoteDebug(int port)
        {
            //Console.Start(port);
        }

        internal static ServiceCenter Center()
        {
            if (null == ServiceCenter.instance)
            {
                throw new MissingComponentException("ServiceCenter component is missing!");
            }

            return ServiceCenter.instance;
        }

        private static ServiceCenter instance = null;
    }
}
