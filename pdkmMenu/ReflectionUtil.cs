using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class ReflectionUtil
{
    public static object ReflectMethod<T>(object instance, string Method, object[] args = null)
    {
        MethodInfo methodInfo = typeof(T).GetMethod(Method, BindingFlags.NonPublic | BindingFlags.Instance);
        return methodInfo.Invoke(instance, args);
    }
    public static object ReflectField<T>(object instance, string Field)
    {
        FieldInfo fieldInfo = typeof(T).GetField(Field, BindingFlags.NonPublic | BindingFlags.Instance);
        return fieldInfo.GetValue(instance);
    }

}

