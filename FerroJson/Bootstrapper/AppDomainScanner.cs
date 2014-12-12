namespace FerroJson.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AppDomainScanner
    {
        public static IEnumerable<Type> Types<T>()
        {
            return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                   where !assembly.IsDynamic
                   from type in assembly.GetExportedTypes()
                   where typeof(T).IsAssignableFrom(type)
                   where !type.IsAbstract
                   where type.IsPublic
                   select type;
        }
    }
}
