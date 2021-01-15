using System;
using System.Reflection;

namespace Csv.Plumbing.Reflection
{
    public class PropertyAccessor : ValueAccessor<PropertyInfo>
    {
        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            InnerInfo = propertyInfo;
        }

        public override Type Type => InnerInfo.PropertyType;

        protected override void SetValue(
            object obj,
            object value)
        {
            InnerInfo.SetValue(obj, value);
        }

        protected override object GetValue(object obj)
        {
            return InnerInfo.GetValue(obj);
        }
    }
}
