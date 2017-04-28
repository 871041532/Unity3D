// -----------------------------------------------------------------
// File:    AssetPackLoadType.cs
// Author:  mouguangyi
// Date:    2016.07.08
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.AssetManager
{
    // �ʲ���װ�ط�ʽ��
    enum AssetPackLoadType
    {
        // App�ڳ�����
        INTERNALSCENE,

        // ResourcesĿ¼�¡�
        RESOURCES,

        // StreamingAssetsĿ¼�¡�
        STREAMINGASSETS,

        // PersistentPathDataĿ¼�¡�
        PERSISTENTDATAPATH,

        // Զ�˷�������
        REMOTE,
    }
}