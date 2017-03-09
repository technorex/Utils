using System;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TechnoRex.Utils.Config
{
    public abstract class ProxyAppConfig : IDisposable
    {
        public static ProxyAppConfig Change(Assembly assembly)
        {
            return new ChangeAppConfig(GetCurrentLocationAppConfig(assembly));
        }

        //path => musi wskazywać na plik App.config nie na katalog
        public static ProxyAppConfig Change(string path)
        {
            return new ChangeAppConfig(path);
        }



        public static string GetCurrentLocationAppConfig(Assembly assembly)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return Path.Combine(Path.GetDirectoryName(assembly.Location), "App.config");

        }




        public abstract void Dispose();

        private class ChangeAppConfig : ProxyAppConfig
        {
            private readonly string oldConfig =
                AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString();

            private bool disposedValue;

            public ChangeAppConfig(string path)
            {
                AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", path);
                ResetConfigMechanism();
            }

            public override void Dispose()
            {
                if (!disposedValue)
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", oldConfig);
                    ResetConfigMechanism();


                    disposedValue = true;
                }
                GC.SuppressFinalize(this);
            }

            private static void ResetConfigMechanism()
            {
                typeof(ConfigurationManager)
                    .GetField("s_initState", BindingFlags.NonPublic |
                                             BindingFlags.Static)
                    .SetValue(null, 0);

                typeof(ConfigurationManager)
                    .GetField("s_configSystem", BindingFlags.NonPublic |
                                                BindingFlags.Static)
                    .SetValue(null, null);

                typeof(ConfigurationManager)
                    .Assembly.GetTypes()
                    .Where(x => x.FullName ==
                                "System.Configuration.ClientConfigPaths")
                    .First()
                    .GetField("s_current", BindingFlags.NonPublic |
                                           BindingFlags.Static)
                    .SetValue(null, null);
            }
        }
    }
}