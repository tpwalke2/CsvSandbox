using System;
using System.Reflection;

namespace Csv.Plumbing.Reflection
{
    public class PropertyAccessor : ValueAccessor
    {
        private readonly PropertyInfo _propertyInfo;

        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public override string Name => _propertyInfo.Name;
        public override Type Type => _propertyInfo.PropertyType;

        protected override void SetValue(
            object obj,
            object value) {_propertyInfo.SetValue(obj, value);}

        protected override object GetValue(
            object obj) {return _propertyInfo.GetValue(obj);}
    }
}
