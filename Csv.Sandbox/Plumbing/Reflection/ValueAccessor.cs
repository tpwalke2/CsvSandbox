using System;
using System.Reflection;
using Csv.Attributes;

namespace Csv.Plumbing.Reflection
{
    public abstract class ValueAccessor
    {
        public abstract string Name { get; }
        public abstract Type Type { get; }
        public IndexedProperty<object, object> Value => new(GetValue, SetValue);
        public abstract bool HasAttribute<TAttr>() where TAttr : Attribute;
        protected abstract object GetValue(object obj);
        protected abstract void SetValue(object obj, object value);
    }

    public abstract class ValueAccessor<TInfo> : ValueAccessor where TInfo : MemberInfo
    {
        protected TInfo InnerInfo { get; init; }
        public override string Name => GetName();
        public override bool HasAttribute<TAttr>()
        {
            return InnerInfo.GetCustomAttribute<TAttr>() != null;
        }

        private string GetName()
        {
            var propertyAttr = InnerInfo.GetCustomAttribute<CsvPropertyAttribute>();
            return string.IsNullOrEmpty(propertyAttr?.PropertyName)
                ? InnerInfo.Name 
                : propertyAttr.PropertyName;
        }
    }
}
