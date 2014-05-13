using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Expressions;
namespace Helper
{
    public interface IPropertyCore
    {
        object GetPropertyValue(object instance, PropertyInfo property);
        void SetPropertyValue(object instance, PropertyInfo property, object val);
    }

    /// <summary>
    /// default
    /// </summary>
    public class DefaultPropertyCore : IPropertyCore
    {
        public void SetPropertyValue(object t, PropertyInfo property, object val)
        {
            string name = property.Name;
            Type type = property.PropertyType.BaseType;
            object oooo = val;
            if (type == typeof(ValueType))
            {
                Type[] arr = property.PropertyType.GetGenericArguments();
                oooo = Convert.ChangeType(val, arr[arr.Length - 1]);
            }

            property.SetValue(t, oooo, null);
        }

        public object GetPropertyValue(object t, PropertyInfo property)
        {
            return property.GetValue(t, null);
        }
    }


    public class ExpressionPropertyCore : IPropertyCore
    {
        public readonly static ExpressionPropertyCore Current = new ExpressionPropertyCore();

        public void SetPropertyValue(object t, PropertyInfo property, object val)
        {
            Action<object, object> action = CreateSetAction(t.GetType(), property);
            action(t, val);
        }

        public object GetPropertyValue(object t, PropertyInfo property)
        {
            Func<object, object> fun = CreateGetFunction(t.GetType(), property);

            return fun(t);
        }

        private Action<object, object> CreateSetAction(Type targetType, PropertyInfo property)
        {
            // PropertyInfo property = targetType.GetProperty(propertyName);
            MethodInfo setMethod = property.GetSetMethod();
            ParameterExpression target = Expression.Parameter(typeof(object), "target");
            ParameterExpression propertyValue = Expression.Parameter(typeof(object), "value");
            UnaryExpression castedTarget = setMethod.IsStatic ? null : Expression.Convert(target, property.PropertyType);
            UnaryExpression castedpropertyValue = Expression.Convert(propertyValue, property.PropertyType);
            MethodCallExpression propertySet = Expression.Call(castedTarget, setMethod, castedpropertyValue);
            return Expression.Lambda<Action<object, object>>(propertySet, target, propertyValue).Compile();
        }

        private Func<object, object> CreateGetFunction(Type targetType, PropertyInfo property)
        {
            // PropertyInfo property = targetType.GetProperty(propertyName);
            MethodInfo getMethod = property.GetGetMethod();
            ParameterExpression target = Expression.Parameter(typeof(object), "target");
            UnaryExpression castedTarget = getMethod.IsStatic ? null : Expression.Convert(target, targetType);
            MemberExpression getProperty = Expression.Property(castedTarget, property);
            UnaryExpression castPropertyValue = Expression.Convert(getProperty, typeof(object));
            return Expression.Lambda<Func<object, object>>(castPropertyValue, target).Compile();
        }
    }

    public class DelegatePropertyCore : IPropertyCore
    {
        public delegate object GetPropertyValueDelegate();
        public delegate void SetPropertyValueDelegate(object ins, object val);

        public object GetPropertyValue(object instance, PropertyInfo property)
        {
            GetPropertyValueDelegate dgv = CreateGetPropertyValueDelegate(instance, property);

            return dgv();
        }

        public void SetPropertyValue(object instance, PropertyInfo property, object val)
        {
            SetPropertyValueDelegate dsv = CreateSetPropertyValueDelegate(instance, property);
            dsv(instance, val);
        }

        public GetPropertyValueDelegate CreateGetPropertyValueDelegate(object instance, PropertyInfo property)
        {
            return (GetPropertyValueDelegate)Delegate.CreateDelegate(typeof(GetPropertyValueDelegate), instance, property.GetGetMethod());
        }

        public SetPropertyValueDelegate CreateSetPropertyValueDelegate(object instance, PropertyInfo property)
        {
            return (SetPropertyValueDelegate)Delegate.CreateDelegate(typeof(SetPropertyValueDelegate), instance, property.GetSetMethod());
        }

        T GetDelegate<T>(object instance, MethodInfo methodInfo) where T : class
        {
            return Delegate.CreateDelegate(typeof(T), instance, methodInfo) as T;
        }
    }
}