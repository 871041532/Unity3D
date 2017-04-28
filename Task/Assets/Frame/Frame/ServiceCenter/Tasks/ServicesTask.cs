// -----------------------------------------------------------------
// File:    ServicesTask.cs
// Author:  mouguangyi
// Date:    2016.05.26
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Framework
{
    /// <summary>
    /// @details ��ȡ���Service���첽���񡣷��ص�Result��һ��IService[]�����е�Service
    /// ˳��ʹ����Service��Id��string[]�е�˳��һ�¡�
    /// </summary>
    public sealed class ServicesTask : AsyncTask
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="serviceIds">Service��Id���鼯�ϡ�</param>
        public ServicesTask(string[] serviceIds) : base(true)
        {
            this.serviceIds = serviceIds;
            Result = this.services = new IService[this.serviceIds.Length];
        }

        /// <summary>
        /// �Ƿ���ɡ�
        /// </summary>
        /// <returns>true��ʾ��ɣ�false��ʾδ��ɡ�</returns>
        protected override bool IsDone()
        {
            bool done = true;
            for (var i = 0; i < this.services.Length; ++i) {
                if (null == this.services[i]) {
                    done = false;
                    this.services[i] = this.center._FindService(this.serviceIds[i]);
                }
            }

            return done;
        }

        private string[] serviceIds = null;
        private IService[] services = null;
    }
}
