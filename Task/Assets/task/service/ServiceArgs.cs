
using System.Collections.Generic;

namespace GameFramework
{
    sealed class ServiceArgs : IServiceArgs
    {
        public const string ARGS_FOLDER = "GameBox/";
        public const string ARGS_CONFFILE = "com.giant.gamebox.conf";
        public const string ARGS_EXTENSION = ".args";

        public ServiceArgs()
        { }

        public T Get<T>(string key)
        {
            return (T)this.args[key];
        }

        public void Set<T>(string key, T value)
        {
            this.args[key] = value;
        }

        private Dictionary<string, object> args = new Dictionary<string, object>();
    }
}