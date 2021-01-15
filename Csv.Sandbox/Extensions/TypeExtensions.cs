using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Csv.Attributes;
using Csv.Plumbing.Reflection;

namespace Csv.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<ValueAccessor> GetAccessors(this Type t, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance)
        {
            return t.GetProperties(bindingAttr)
                    .Select(pi => new PropertyAccessor(pi) as ValueAccessor)
                    .Concat(t.GetFields(bindingAttr)
                             .Select(fi => new FieldAccessor(fi) as ValueAccessor))
                    .Where(gs => !gs.HasAttribute<CsvIgnoreAttribute>())
                    .GroupBy(gs => gs.Name)
                    .Select(group => group.First());
        }

        public static object Convert(this Type t, string input)
        {
            try
            {
                return TypeDescriptor.GetConverter(t)
                                     .ConvertFrom(input);
            }
            catch (NotSupportedException)
            {
                return null;
            }
        }
    }
}
