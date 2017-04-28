// -----------------------------------------------------------------
// File:    ServicePlayerr.cs
// Author:  mouguangyi
// Date:    2016.11.17
// Description:
//      
// -----------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

namespace GameBox.Framework
{
    /// <summary>
    /// @details Service在Unity运行模式下的运行环境。
    /// 
    /// @section howtouse 如何使用
    /// 直接拖拽到一个GameObject上即可。
    /// 
    /// </summary>
    [HelpURL("https://muguangyi.github.io/GameBox/Help/class_game_box_1_1_framework_1_1_service_player.html")]
    [DisallowMultipleComponent]
    public sealed class ServicePlayerr : MonoBehaviour, IServiceHost
    {
        /// <summary>
        /// 启动页面颜色。默认关闭，如果需要，可以将颜色的alpha置为大于0。
        /// </summary>
        [Tooltip("Splash screen color before showing the first screen.")]
        public Color SplashColor = new Color(0, 0, 0, 0);

        /// <summary>
        /// Log类型。
        /// @see LogType
        /// </summary>
        [Tooltip("Recording log type.")]
        public LogType LogType = LogType.CONSOLE;

        /// <summary>
        /// Log等级。
        /// @see LogLevel
        /// </summary>
        [Tooltip("Recording log level.")]
        public LogLevel LogLevel = LogLevel.VERBOSE;

        /// <summary>
        /// Service参数文件服务器地址。
        /// @note 指定了Service参数文件的服务器地址，ServiceCenter会在启动后从该服务器尝试下载最新的Service参数文件，并设置到对应的Service中。不使用该功能则留空。
        /// </summary>
        [Tooltip("Service args file server. It is optional.")]
        public string ServiceArgsServer = null;

        /// <summary>
        /// 是否在运行时显示Service图表。
        /// </summary>
        [Tooltip("Whether need to show service graph during runtime?")]
        public bool ShowGraph = false;

        public void Awake()
        {
            this.asyncTerminateFlag = false;
            this.asyncWait = new AutoResetEvent(false);
            this.asyncThread = new Thread(new ThreadStart(_RunAsyncThread));
            this.asyncThread.Start();

            this.center = new ServiceCenter();
            this.center._SplashColor = this.SplashColor;
            this.center._LogType = this.LogType;
            this.center._LogLevel = this.LogLevel;
            this.center._Enter(this);

            DontDestroyOnLoad(this.gameObject);
        }

        //public void Start()
        //{
            //if (string.IsNullOrEmpty(this.ServiceArgsServer)) {
            //    _StartServices();
            //} 
                // Download remote gamebox service conf file (com.giant.gamebox.conf) whose format is JSON as follows:
                // ------------------------------------------
                // {
                //   "com.giant.service.xxxx": "[md5 code]",
                //   "com.giant.service.xxxx": "[md5 code]",
                //   ...
                // }
                // ------------------------------------------
        //        new HttpDownloadTask(this.ServiceArgsServer + ServiceArgs.ARGS_FOLDER + ServiceArgs.ARGS_CONFFILE).Start().Continue(task =>
        //        {
        //            if (null != task.Result) {
        //               /// var list = SimpleJson.SimpleJson.DeserializeObject<Dictionary<string, object>>(Encoding.UTF8.GetString(task.Result as byte[]));
        //              //  task = new CompletedTask();
        //               // foreach (var service in list.Keys) {
        //                   // var checkCode = list[service] as string;
        //                   // var relativePath = ServiceArgs.ARGS_FOLDER + service + ServiceArgs.ARGS_EXTENSION;
        //                  // // var fullPath = PathUtility.ComposeDataPath(relativePath);
        //                  //  if (File.Exists(fullPath)) {
        //                      //  task = task
        //                      //  .Continue(task2 =>
        //                      //  {
        //                      //      return new FileReadTask(fullPath);
        //                    //    })
        //                      //  .Continue(task2 =>
        //                      //  {
        //                      //      return (CryptoUtility.ComputeMD5Code(task2.Result as byte[]) == checkCode ? null : new HttpDownloadTask(this.ServiceArgsServer + relativePath));
        //                    //    })
        //                    //    .Continue(task2 =>
        //                    //    {
        //                     //       return (null == task2.Result ? null : new FileWriteTask(fullPath, task2.Result as byte[]));
        //                     //   });
        //    //                } else {
        //    //                    task = task
        //    //                    .Continue(task2 =>
        //    //                    {
        //    //                        return new HttpDownloadTask(this.ServiceArgsServer + relativePath);
        //    //                    })
        //    //                    .Continue(task2 =>
        //    //                    {
        //    //                        return (null == task2.Result ? null : new FileWriteTask(fullPath, task2.Result as byte[]));
        //    //                    });
        //    //                }
        //    //            }

        //    //            return task;
        //    //        } else {
        //    //            return null;
        //    //        }
        //    //    })
        //    //    .Continue(task =>
        //    //    {
        //    //        _StartServices();
        //    //        return null;
        //    //    });
        //    //}
        //}

        public void LateUpdate()
        {
            this.center._Execute(Time.deltaTime);
        }

        public void OnDestroy()
        {
            this.center._Exit();
            this.center = null;

            if (null != this.asyncThread) {
                this.asyncTerminateFlag = true;
                this.asyncWait.Set();
                this.asyncThread = null;
            }
        }

        public void OnGUI()
        {
            this.center._DrawSplash();

            if (ShowGraph) {
                this.center._OnGraph();
            }
        }

        public void RunAsyncTask(AsyncTask task)
        {
            if (task.CanRunOnThread) {
                lock (this.asyncTasks) {
                    this.asyncTasks.AddLast(task.GetExecuter());//立即执行
                }
                this.asyncWait.Set();
            } else {
                StartCoroutine(task.GetExecuter());//利用协程等待IEnumerator
            }
        }

        private void _StartServices()
        {
            var installers = GetComponents<ServiceBaseInstaller>();
            for (var i = installers.Length - 1; i >= 0; --i) {
                var installer = installers[i];
                if (null != installer) {
                    installer._Install(this.center);
                    Object.Destroy(installer);
                }
            }

            var serviceCount = this.center._RunnerCount;
            for (var i = 0; i < serviceCount; ++i) {
                var runner = this.center._GetRunnerByIndex(i);
                runner.Startup();
            }
        }

        private void _RunAsyncThread()
        {
            while (!this.asyncTerminateFlag) {
                if (0 == this.asyncTasks.Count) {
                    this.asyncWait.WaitOne();
                }

                var executor = this.asyncTasks.First;
                while (null != executor) {
                    var current = executor;
                    executor = executor.Next;

                    if (!current.Value.MoveNext()) {
                        lock (this.asyncTasks) {
                            this.asyncTasks.Remove(current);
                        }
                    }
                }
            }

            this.asyncTasks.Clear();
        }

        private ServiceCenter center = null;
        private volatile bool asyncTerminateFlag = false;
        private AutoResetEvent asyncWait = null;
        private Thread asyncThread = null;
        private LinkedList<IEnumerator> asyncTasks = new LinkedList<IEnumerator>();
    }
}