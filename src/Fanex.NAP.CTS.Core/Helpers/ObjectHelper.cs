namespace Fanex.NAP.CTS.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;

    public static class ObjectHelper
    {
        public static T Pick<T>(this T? target, T defaultValue) where T : struct => target.HasValue ? target.Value : defaultValue;

        public static string Pick(this string target, string defaultValue = "-") => string.IsNullOrWhiteSpace(target) ? defaultValue : target;

        public static T ConvertObject<T>(object source)
        {
            var target = Activator.CreateInstance<T>();
            var sourceType = source.GetType();
            var targetFields = target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
          
            foreach (var field in targetFields)
            {               
                var sourceProp = sourceType.GetProperty(field.Name, BindingFlags.Instance | BindingFlags.Public);

                if (sourceProp == null)
                {
                    continue;
                }                    
                var value = sourceProp.GetValue(source);

                if (field.CanWrite)
                {
                    field.SetValue(target, value);
                }
            }

            return target;
        }

        public static string SerializeObj<T>(T obj)
        {           
            return JsonSerializer.Serialize(obj);
        }

        public static IEnumerable<T> DeserializeObj<T>(string obj)
        {
            return JsonSerializer.Deserialize<IEnumerable<T>>(obj) ?? Enumerable.Empty<T>();
        }

    }
}