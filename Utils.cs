using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;

namespace TechnoRex.Utils
{
    public class Utils
    {
        /// <summary>
        /// Deep copy of object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prototype"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(T prototype) where T : class
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, prototype);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(memoryStream) as T;
            }
        }

        /// <summary>
        /// Whole copy of directory
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyDir(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyDir(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name));
        }


        public static IIdentity GetCurrentIdentity()
        {
            return WindowsIdentity.GetCurrent();
        }

        public static IdentityReference GeUserSID(string userName)
        {
            return new NTAccount(userName).Translate(typeof(SecurityIdentifier));
        }

        public static IdentityReference GetCurrentUserSID()
        {
            return new NTAccount(WindowsIdentity.GetCurrent().Name).Translate(typeof(SecurityIdentifier));
        }


        public static object GetPropertyValueByPath(object src, string propName)
        {
            if (propName.Contains("."))
            {
                string[] Split = propName.Split('.');
                string RemainingProperty = propName.Substring(propName.IndexOf('.') + 1);
                return GetPropertyValueByPath(src.GetType().GetProperty(Split[0]).GetValue(src, null), RemainingProperty);
            }
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }




    }
}