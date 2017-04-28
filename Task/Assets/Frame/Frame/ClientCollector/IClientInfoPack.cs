// -----------------------------------------------------------------
// File:    IClientInfoPack.cs
// Author:  mouguangyi
// Date:    2016.05.16
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.ClientCollector
{
    /// <summary>
    /// �ͻ��˵Ļ�����Ϣ����
    /// </summary>
    public interface IClientInfoPack
    {
        /// <summary>
        /// �豸ģʽ��
        /// </summary>
        string DeviceMode { get; }

        /// <summary>
        /// �豸���ơ�
        /// </summary>
        string DeviceName { get; }

        /// <summary>
        /// �豸Ψһ��ʶ����
        /// </summary>
        string DeviceIdentifier { get; }

        /// <summary>
        /// ͼ���豸��ʶ����
        /// </summary>
        int GraphicsDeviceId { get; }

        /// <summary>
        /// ͼ���豸���ơ�
        /// </summary>
        string GraphicsDeviceName { get; }

        /// <summary>
        /// ͼ���豸���͡�
        /// </summary>
        int GraphicsDeviceType { get; }

        /// <summary>
        /// ͼ���豸�����̡�
        /// </summary>
        string GraphicsDeviceVendor { get; }

        /// <summary>
        /// ͼ���豸�汾��
        /// </summary>
        string GraphicsDeviceVersion { get; }

        /// <summary>
        /// ͼ���豸�ڴ��С��
        /// </summary>
        int GraphicsMemorySize { get; }

        /// <summary>
        /// ����ϵͳ��
        /// </summary>
        string OperatingSystem { get; }

        /// <summary>
        /// �豸������������
        /// </summary>
        int ProcessorCount { get; }

        /// <summary>
        /// �豸������Ƶ�ʡ�
        /// </summary>
        int ProcessorFrequency { get; }

        /// <summary>
        /// �豸���������͡�
        /// </summary>
        string ProcessorType { get; }

        /// <summary>
        /// �Ƿ�֧�������ǡ�
        /// </summary>
        bool DoesSupportGyroscope { get; }

        /// <summary>
        /// �Ƿ�֧�ֶ�λ����
        /// </summary>
        bool DoesSupportLocationService { get; }

        /// <summary>
        /// �豸�ڴ��С��
        /// </summary>
        int SystemMemorySize { get; }

        /// <summary>
        /// �������͡�
        /// </summary>
        string NetworkReachability { get; }

        /// <summary>
        /// ��Ļ�ֱ��ʿ�ȡ�
        /// </summary>
        int ResolutionWidth { get; }

        /// <summary>
        /// ��Ļ�ֱ��ʸ߶ȡ�
        /// </summary>
        int ResolutionHeight { get; }

        /// <summary>
        /// ��Ļˢ���ʡ�
        /// </summary>
        int ResolutionRefreshRate { get; }

        /// <summary>
        /// ���ҡ�
        /// </summary>
        string Nation { get; }

        /// <summary>
        /// ��Ӫ�����ơ�
        /// </summary>
        string Carrier { get; }
    }
}