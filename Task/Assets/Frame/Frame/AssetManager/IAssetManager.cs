// -----------------------------------------------------------------
// File:    IAssetManager.cs
// Author:  mouguangyi
// Date:    2016.05.26
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using UnityEngine.SceneManagement;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details �ʲ����������GameBoxƽ̨�ṩ�ķ������õ���Դװ�غͶ�ȡ����ͨ����ȡ��IAssetManager�ӿڣ�ʹ���߿��Ը�����Դ�����·������Դ���ʹ�����Ӧ��װ������װ�����ṩ�����ʲ�װ�ط�ʽ��ͬ�����첽��ʹ���߸���ʹ�ó���������Ҫͬ�����ػ����첽���ء��ʲ����������������Դ�Ĵ洢λ�ã�ʹ�������������Դ�Ǵ洢��Resources�£�����StreamingAssets�£������PersistentDataPath�¡�ͬʱ�ʲ��������Ҳ��������Դ�Ĵ洢��ʽ��ʹ����Ҳ���������Դ���Դ���ķ�ʽ����һ��AssetBundle�У��������ԭʼ״̬�����洢��
    /// 
    /// @note ��Ϊ�ʲ�������������������ṩ��һ����Դ�����ļ�������ʲ���������ڳ�ʼ��ʱ���������ļ������������ʱ������Դ���£����µ���Դ���Ǳ��أ���ô��Ҫʹ��������Դ������ɺ���������UpdateAsync��������ȷ������ʱʹ�����µ���Դ��
    /// 
    /// @section example ����
    /// @code{.cs}
    /// // �����ʲ���������ID�첽��ȡ�ʲ��������
    /// new ServiceTask("com.giant.service.assetmanager").Start().Continue(task =>
    /// {
    ///   var assetManager = task.Result as IAssetManager;
    ///   return null;
    /// });
    /// @endcode
    /// 
    /// @code{.cs}
    /// // �����ʲ��������ӿ��첽��ȡ�ʲ��������
    /// new ServiceTask<IAssetManager>().Start().Continue(task =>
    /// {
    ///   var assetManager = task.Result as IAssetManager;
    ///   return null;
    /// });
    /// @endcode
    /// 
    /// @code {.cs}
    /// // �����ȷ���ʲ���������Ѿ����У�����ֱ�ӻ�ȡ
    /// var assetManager = TaskCenter.GetService<IAssetManager>();
    /// @endcode
    /// </summary>
    public interface IAssetManager : IService
    {
        /// <summary>
        /// �ʲ��汾��
        /// </summary>
        string AssetVersion { get; }

        /// <summary>
        /// ������Դ·�������ͣ�ͬ��װ����Դ��
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IAsset Load(string path, AssetType type);

        /// <summary>
        /// ������Դ·�������ͣ��첽װ����Դ��
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        void LoadAsync(string path, AssetType type, Action<IAsset> handler);

        /// <summary>
        /// ����·����ģʽ��ͬ��װ�س�����
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        IAsset LoadScene(string path, LoadSceneMode mode);

        /// <summary>
        /// ����·����ģʽ���첽װ�س�����
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <param name="handler"></param>
        void LoadSceneAsync(string path, LoadSceneMode mode, Action<IAsset> handler);

        /// <summary>
        /// ����·�������ʹ���װ������
        /// @note ��Դ·��Ҫ�����ļ���չ��������.prefab��.txt�ȵȡ�
        /// </summary>
        /// <param name="path">��Դ·����</param>
        /// <param name="type">��Դ���͡�</param>
        /// <returns>���ض�Ӧ����Դ��������</returns>
        [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
        IAssetLoader CreateLoader(string path, AssetType type);

        /// <summary>
        /// ����·����װ�������ʹ���װ������
        /// @note ��Դ·��Ҫ�����ļ���չ��������.prefab��.txt�ȵȡ�
        /// </summary>
        /// <typeparam name="T">��Դ���������͡�</typeparam>
        /// <param name="path">��Դ·����</param>
        /// <returns>���ض�Ӧ����Դ��������</returns>
        [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
        T CreateLoader<T>(string path) where T : IAssetLoader;

        /// <summary>
        /// ��Ч�ʲ��������ա�
        /// </summary>
        void GC();

        /// <summary>
        /// ��������ʱ��Դ���ñ���Դ���ñ���AssetManager��������ʱ���Զ�����һ�Σ������֮������Դ�ȸ��£���ô�ڸ��º�ҲӦ����������һ����ȷ�������µ���Դһ�¡�
        /// </summary>
        /// <param name="callback">������ɵĻص����������</param>
        void UpdateAsync(Action callback);
    }
}