using System;
using System.Reflection;

namespace TechnoRex.Utils.Externals
{
    public class ExternaMethodInvoker : MarshalByRefObject
    {
        object CallInternal(string dll, string typename, string method, object[] parameters)
        {
            Assembly a = Assembly.LoadFile(dll);
            object o = a.CreateInstance(typename);
            Type t = o.GetType();
            MethodInfo m = t.GetMethod(method);
            return m.Invoke(o, parameters);
        }
        public static object Call(string dll, string typename, string method, params object[] parameters)
        {
            AppDomain dom = AppDomain.CreateDomain("MyNewDomain");
            ExternaMethodInvoker ld = (ExternaMethodInvoker)dom.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(ExternaMethodInvoker).FullName);
            object result = ld.CallInternal(dll, typename, method, parameters);
            AppDomain.Unload(dom);
            return result;
        }
    }
}