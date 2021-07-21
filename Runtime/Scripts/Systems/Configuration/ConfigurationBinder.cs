using System;
using System.ComponentModel;
using System.Reflection;

namespace TinaX.Systems.Configuration
{
    public static class ConfigurationBinder
    {

        public static T GetValue<T>(this IConfiguration configuration, string key)
        {
            return configuration.GetValue<T>(key, default(T));
        }

        public static T GetValue<T>(this IConfiguration configuration, string key, T defaultValue)
        {
            return (T)GetValue(configuration, typeof(T), key, defaultValue);
        }


        public static object GetValue(this IConfiguration configuration, Type type, string key)
        {
            return configuration.GetValue(type, key, defaultValue: null);
        }

        public static object GetValue(this IConfiguration configuration, Type type, string key, object defaultValue)
        {
            var section = configuration.GetSection(key);
            var value = section.Value;
            if (value != null)
                return ConvertValue(type, value, section.Path);
            return defaultValue;
        }


        private static bool TryConvertValue(Type type, string value, string path , out object result, out Exception error)
        {
            error = null;
            result = null;
            if(type == typeof(object))
            {
                result = value;
                return true;
            }

            if(type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (string.IsNullOrEmpty(value))
                {
                    return true;
                }
                return TryConvertValue(Nullable.GetUnderlyingType(type), value, path, out result, out error); //递归
            }

            var converter = TypeDescriptor.GetConverter(type);
            if (converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    result = converter.ConvertFromInvariantString(value);
                }
                catch(Exception e)
                {
                    error = new InvalidOperationException($"Cannot covert \"{path}\" to type {type.FullName}", e);
                }
                return true;
            }
            return false;
        }

        private static object ConvertValue(Type type, string value, string path)
        {
            TryConvertValue(type, value, path, out object result, out Exception error);
            if (error != null)
                throw error;
            return result;
        }
    }
}
