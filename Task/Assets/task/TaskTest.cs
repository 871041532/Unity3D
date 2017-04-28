using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameBox;
using GameBox.Framework;
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
//巨人GameBox Unity中间件介绍：
//巨人GameBox Unity中间件是为Unity开发的基于插件思想的客户端中间件框架，所有的模块都是以DLL形式提供，功能都是以interface对外暴露，在语言层面规范访问的边界。
//所有的模块都是以Service定义，通过ServiceCenter获取到相应的Service，然后转换成对应的Service接口，就可以直接使用Service对外暴露的各种方法。

    
//AsyncTask类分析：
//（1）Start方法将自身添加到ServiceCenter中去,并执行ServerCenter中_RunAsyncTask方法将自身加添到List<AsyncTask> actionQueue中
//（2）每次Continue创建一个childAsyncTask，此child再调用start()将自身添加到同一个List<AsyncTask> actionQueue中

//ServerCenter与ServerPlayer类分析：
//（1）在ServerCenter _RunAsyncTask回调中执行了ServicePlayer中RunAsyncTask方法，在其中将task Executer属性添加到末尾或者加入协程等待，以此实现迭代执行。
//（2）ServicePlayer脚本添加至GameObject上，FixedUpdate循环调用_Execute()，进而调用serverCenter中_execute()
//（3）ServerCenter中_execute()遍历actionQueue列表，并回调执行其中方法。（其实没必要，迭代方法每次调用均只调一个）
//（4）ServicePlayer开了一个线程运行_RunAsyncThread，内部while循环不断执行Task迭代器MoveNext方法。


//总结：
//（1）链式迭代结构，利用协程实现异步。（Executer类型属性实现接口IEnumerator，如此支持Func<AsyncTask,object> handler内部协程）
//（2）迭代调用开启者为脚本ServicePlayer FixedUpdate函数；迭代至末尾自动终止。
//（3）回调其实在主线程中执行，子线程执行的其实是迭代过程的逻辑。用于协程串联效率较高
//（4）扩展任务，创建一个继承自AsyncTask的类，重写protected virtual bool IsDone()方法（类似Job显式调用success()）

//Job与Task对比：
//（1）Serverplayer单独开辟一个线程跑while专门调用Executer MoveNext迭代，Job则主线程代码中调用success以示结束，专门为Task开个线程跑While不合算。
//（2）Task将不操纵u3d对象的逻辑放入isDone()函数在第二线程执行效率更高；Task Lambda回调其实是在主线程执行的，这点与Job无异。
//（3）Task是链式结构，Job是树形执行结构。
//（4）Job更适合写扩展脚本如控制特效播放等，Task更适合写unity协程或IO异步操作以防止阻塞UI线程。

namespace GameFramework
{
    public class TaskTest
    {
        public static TestUsing use;
        [MenuItem("Test/Task/TestInit")]
        public static void TestInit()//初始化Test
        {
            GameObject TestObject = new GameObject("TaskTestObject");
            TestObject.AddComponent<ServicePlayer>();
            use = TestObject.AddComponent<TestUsing>();
        }

        [MenuItem("Test/Task/AsyncTaskTest")]//AsyncTask测试
        public static void AsyncTaskTest()
        {       
            use.AsyncTaskTest();
        }
        [MenuItem("Test/Task/DownLoadAndWrite")]
        public static void DownLoadAndWrite()//资源下载测试
        {
            use.DownLoadAndWrite();
        }

    }
    public class TestUsing:MonoBehaviour
    {    
        public void DownLoadAndWrite()
        {
            new HttpDownloadTask(@"https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1493373803330&di=811ffbf9429ec68e8b8e23d3733c4939&imgtype=0&src=http%3A%2F%2Fmvimg1.meitudata.com%2F56125377d1f169287.jpg").Start().Continue((task)=> {
                Debug.Log("下载完成");

                new FileWriteTask(@"E:\download.jpg", task.Result as byte[]).Start().Continue((task2) => {
                    Debug.Log("写入完成");
                    return null;
                });

                return null;
            });
        }

        public void AsyncTaskTest()
        {
            new AsyncTask(true).Start().Continue((task)=>
            {
                StartCoroutine(loadResourcesAsync((UnityEngine.Object a) =>
                {
                    GameObject resources = a as GameObject;
                    GameObject temp = Instantiate(resources, transform);
                    temp.name = "effect";
                    temp.transform.position = new Vector3(-10, 0, 0);
                }));
                return null;
            }).Continue((task) =>
            {
                StartCoroutine(loadResourcesAsync((UnityEngine.Object a) =>
                {
                    GameObject resources = a as GameObject;
                    GameObject temp = Instantiate(resources, transform);
                    temp.name = "effect";
                    temp.transform.position = new Vector3(0, 0, 0);
                }));
                return null;
            }).Continue((task) =>
            {
                Debug.Log("All resources haven done");
                return null;
            }).Start();
        }

        //加载资源
        IEnumerator loadResourcesAsync(Action<UnityEngine.Object> callback)
        {
            ResourceRequest temp = Resources.LoadAsync<GameObject>("test");
            yield return temp;
            callback(temp.asset);
        }
    }
}
