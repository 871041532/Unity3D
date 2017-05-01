
namespace GameBox
{
    /// <summary>
    /// 巨人GameBox 
    /// @mainpage Gaint GameBox Unity中间件
    ///
    /// @section intrudction 简介
    /// 巨人GameBox Unity中间件是为Unity开发的基于插件思想的客户端中间件框架，所有的模块都是以DLL形式提供，功能都是以interface对外暴露，在语言层面规范访问的边界。所有的模块都是以Service定义，通过ServiceCenter获取到相应的Service，然后转换成对应的Service接口，就可以直接使用Service对外暴露的各种方法。
    ///
    /// @section getstart 快速入门
    ///
    /// @subsection init 初始化
    /// @li @c 在Unity项目的Assets目录下创建Editor目录
    /// @li @c 将<A HREF="https://muguangyi.github.io/GameBox/GameBox/Editor/TaskCenter.Editor.dll"><B>TaskCenter.Editor.dll</B></A>下载放在Editor目录
    /// @li @c 切换回Unity，在GameBox菜单中选择ServiceCenter
    /// @li @c 在弹出的工具窗口点击“Create”按钮
    /// @li @c 在更新后的界面勾选需要的组件，并点击“Update”按钮
    /// @subsection conf 配置
    /// @li @c 在场景中创建一个GameObject对象
    /// @li @c 将ServiceCenter下的ServicePlayer组件挂接到该GameObject上
    /// @li @c 将需要的其余组件依次挂接到该GameObject上即可
    ///
    /// @subsection use 使用
    /// @subsubsection getserviceasync 异步获取服务
    ///
    /// @code{.cs}
    /// // 通过服务接口获取资产管理服务
    /// new ServiceTask<IAssetManager>().Start().Continue(task =>
    /// {
    ///   var assetManager = task.Result as IAssetManager;
    ///   return null;
    /// });
    /// @endcode
    ///
    /// @code{.cs}
    /// // 通过服务ID获取资产管理服务
    /// new ServiceTask("com.giant.serivce.assetmanager").Start().Continue(task =>
    /// {
    ///   var assetManager = task.Result as IAssetManager;
    ///   return null;
    /// });
    /// @endcode
    ///
    /// @subsubsection getservicesync 同步获取服务
    /// 获取前需确保相关服务已经装载完毕。
    ///
    /// @code{.cs}
    /// // 使用ServiceCenter通过服务接口获取资产管理服务
    /// var assetManager = TaskCenter.GetService<IAssetManager>();
    /// @endcode
    ///
    /// @code{.cs}
    /// // 使用ServiceCenter通过服务ID获取资产管理服务
    /// var assetManager = TaskCenter.GetService("com.giant.service.assetmanager") as IAssetManager;
    /// @endcode
    ///
    /// @section asynctask 异步任务（AsyncTask）
    /// 在开发过程中经常需要实现异步操作以防止阻塞UI线程，AsyncTask是在.NET3.5基础上模拟实现的一套异步Task系统，方便使用者实现一个或多个异步任务的执行。AsyncTask是建立在Unity的Coroutine和多线程的基础之上的实现，使用者不需要了解具体任务是执行在哪一种情况下。
    /// 
    /// @subsection way AsyncTask使用方式
    /// 使用者启动一个AsyncTask，并在其Continue方法中等待回调结束。
    /// 
    /// @subsubsection first 第一个例子
    /// @code{.cs}
    /// new CompletedTask().Start().Continue(task =>
    /// {
    ///   return null;    // 返回null表示没有待执行的任务。
    /// });
    /// @endcode
    /// 从上面的例子看到，首先new一个AsyncTask，然后调用Start方法启动，并直接在其Continue方法中等待任务执行完毕。执行结束后会将结束的AsyncTask作为参数传入，使用者可以从AsyncTask的Result属性中获取任务结果，并决定后续步骤。
    /// 
    /// @subsubsection second 将多个任务串起来
    /// @code{.cs}
    /// new HttpDownloadTask("http://gamebox.com/a.txt").Start()
    /// .Continue(task =>
    /// {
    ///   // 获取下载文件的内容
    ///   var bytes = task.Result as byte[];
    ///   
    ///   // 开始一个文件存储任务
    ///   return new FileWriteTask("c:/files/a.txt", bytes);
    /// })
    /// .Continue(task =>
    /// {
    ///   // 文件存储结束
    ///   Debug.Log("a.txt file has been saved.");
    ///   return null;  // 任务结束
    /// });
    /// @endcode
    /// 
    /// @subsubsection third 按顺序执行一序列任务
    /// @code{.cs}
    /// var fileList = new string[10];
    /// ...
    /// 
    /// var task = new CompletedTask();
    /// for (var i = 0; i < fileList.Length; ++i) {
    ///   task = task.Continue(task2 =>
    ///   {
    ///     return new FileReadTask("c:/files/" + fileList[i]);
    ///   });
    /// }
    /// 
    /// task.Start().Continue(_ =>
    /// {
    ///   Debug.Log("All tasks are completed.");
    ///   return null;
    /// });
    /// @endcode
    /// 
    /// @subsubsection fouth 同时执行一系列任务
    /// @code{.cs}
    /// var fileList = new string[10];
    /// ...
    /// 
    /// var tasks = new string[fileList.Length];
    /// for (var i = 0; i < fileList.Length; ++i) {
    ///   tasks[i] = new FileReadTask("c:/files/" + fileList[i]);
    /// }
    /// 
    /// new WaitAllTask(tasks).Start().Continue(task =>
    /// {
    ///   Debug.Log("All tasks are completed.");
    ///   return null;
    /// });
    /// @endcode
    /// 
    /// @subsubsection fifth 自定义任务
    /// 使用者也可以自己实现定制化的任务，将特有的逻辑以异步的方式实现。只需要继承AsyncTask，实现构造方法并重写IsDone方法即可。
    /// @note 1.构造方法中AsyncTask需要指明该任务是否可以执行在子线程中，因为Unity的很多操作是不可以在子线程中执行的，因此根据具体执行的逻辑决定构造函数的参数值。
    /// @note 2.IsDone方法是protected，而不是public。
    /// 
    /// @code{.cs}
    /// // 封装WWW下载操作
    /// class WWWTask : AsyncTask
    /// {
    ///   public WWWTask(string url) : base(false)  // 该任务不可以执行在子线程中
    ///   {
    ///     this.w = new WWW(url);
    ///   }
    ///   
    ///   protected override bool IsDone()
    ///   {
    ///     return this.w.isDone;
    ///   }
    ///   
    ///   private WWW w = null;
    /// }
    /// @endcode
    /// </summary>
}

