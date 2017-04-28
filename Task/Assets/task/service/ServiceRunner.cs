
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace GameFramework
{
    abstract class ServiceRunner : IServiceRunner
    {
        public event ServiceShutdown OnShutdown = null;

        public ServiceRunner(ServiceCenter center, ServiceArgs args, IService service)
        {
            this.center = center;
            this.args = args;
            this.service = service;
            this.isReady = false;
        }

        public void Ready(Action terminateMethod)
        {
            this.terminateMethod = terminateMethod;
            this.isReady = true;
        }

        public virtual void Startup()
        {
            var argsPath = PathUtility.ComposeDataPath(ServiceArgs.ARGS_FOLDER + this.service.Id + ServiceArgs.ARGS_EXTENSION);
            if (File.Exists(argsPath))
            {
                new FileReadTask(argsPath).Start().Continue(task =>
                {
                    var bytes = task.Result as byte[];
                    // var dict = SimpleJson.SimpleJson.DeserializeObject<Dictionary<string, object>>(Encoding.UTF8.GetString(bytes));
                    //   foreach (var item in dict) {
                    //    this.args.Set(item.Key, item.Value);
                    //  }

                    this.service.Run(this);

                    return null;
                });
            }
            else
            {
                this.service.Run(this);
            }
        }

        public void Pulse(float delta)
        {
            if (this.isReady)
            {
                this.service.Pulse(delta);
            }
        }

        public virtual void Shutdown()
        {
            this.center._RemoveRunner(this);
            if (null != this.terminateMethod)
            {
                this.terminateMethod();
                this.terminateMethod = null;
            }

            this.center = null;
            this.service = null;
        }

        public T GetArgs<T>(string key)
        {
            return this.args.Get<T>(key);
        }

        public float DrawGraph(float y)
        {
            float yMax = 0f;
            if (this.isReady && (this.service is IServiceGraph))
            {
                var graph = this.service as IServiceGraph;
                GUI.BeginGroup(new Rect(ServiceCenter.SERVICE_MARGIN, y, graph.Width, graph.Height));
                {
                    GUI.matrix.MultiplyPoint3x4(new Vector3(0, y, 0));
                    graph.Draw();
                    GUI.matrix.MultiplyPoint3x4(new Vector3(0, -y, 0));
                }
                GUI.EndGroup();
                yMax += graph.Height;
            }

            return yMax;
        }

        public void NotifyShutdown(IService service)
        {
            if (null != OnShutdown)
            {
                OnShutdown(service);
            }
        }

        public IService Service
        {
            get { return this.service; }
        }

        internal bool _IsReady
        {
            get { return this.isReady; }
        }

        protected ServiceCenter center = null;
        private ServiceArgs args = null;
        private IService service = null;
        private Action terminateMethod = null;
        private bool isReady = false;
    }

    class ServiceRunner<T> : ServiceRunner where T : IService
    {
        public ServiceRunner(ServiceCenter center, ServiceArgs args, IService service, bool enableLog) : base(center, args, service)
        {
            this.logger = new Logger<T>(this.center._LogType, this.center._LogLevel, enableLog);
        }

        public override void Startup()
        {
            base.Startup();

            this.logger._Init();
        }

        public override void Shutdown()
        {
            this.logger._Dispose();
            this.logger = null;

            base.Shutdown();
        }

        private Logger<T> logger = null;
    }
}
