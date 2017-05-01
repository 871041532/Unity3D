// -----------------------------------------------------------------
// File:    IPrefabLoader.cs
// Author:  lvguanghai
// Date:    2016.06.06
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details Prefabװ������
    /// 
    /// @note Prefab������������ά��ʹ����װ�ص���Դʵ������GameObject���������ڣ�ʹ���ߴ�����GameObject��Ҫ�ڲ���Ҫʱ�Լ����ٻ�����ObjectPoolѭ��ʹ�á�
    /// 
    /// @section example ����
    /// @code{.cs}
    /// var assetManager = TaskCenter.GetService<IAssetManager>();
    /// var loader = assetManager.CreateLoader<IPrefabLoader>("XXX.prefab");
    /// var asset = loader.Load(); // ͬ��װ��
    /// var go = GameObject.Instantiate(asset) as GameObject;
    /// ...
    /// // ����
    /// loader.Dispose();
    /// GameObject.Destroy(go);
    /// @endcode
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface IPrefabLoader : IAssetLoader
    {
        /// <summary>
        /// ͬ��װ�ط���
        /// </summary>
        /// <returns>�ɹ��򷵻�UnityEngine.Object�����򷵻�null��</returns>
        UnityEngine.Object Load();
    }
}