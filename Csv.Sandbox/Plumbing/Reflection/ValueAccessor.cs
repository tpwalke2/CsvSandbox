using System;

namespace Csv.Plumbing.Reflection
{
    public abstract class ValueAccessor
    {
        public abstract string Name { get; }
        public abstract Type Type { get; }
        public IndexedProperty<object, object> Value => new IndexedProperty<object, object>(GetValue, SetValue);
        protected abstract object GetValue(object obj);
        protected abstract void SetValue(object obj, object value);
    }
}
