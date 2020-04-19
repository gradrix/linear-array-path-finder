using System;
using Microsoft.Extensions.DependencyInjection;

namespace Models
{
    public static class DependencyResolver
    {
        private static Func<IServiceProvider> _getInstance;
        private static IServiceProvider _instance;

        public static Func<IServiceProvider> GetInstance
        {
            get => _getInstance;
            set
            {
                _getInstance = value;
                _instance = null;
            }
        }

        public static IServiceProvider Instance
        {
            get
            {
                if (_instance != null) return _instance;

                if (_getInstance == null)
                {
                    throw new InvalidOperationException(
                        "Attempted to access DependencyResolver.Instance before setting GetInstance"
                    );
                }
                _instance = GetInstance();

                return _instance;
            }
        }

        public static T GetService<T>()
        {
            return Instance.GetService<T>();
        }
    }
}