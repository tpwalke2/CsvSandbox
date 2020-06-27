using System;
using System.Reflection;

namespace Csv.Plumbing.Reflection
{
    public class FieldAccessor : ValueAccessor
    {
        private readonly FieldInfo _fieldInfo;

        public FieldAccessor(FieldInfo fieldInfo)
        {
            _fieldInfo = fieldInfo;
        }

        public override string Name => _fieldInfo.Name;
        public override Type Type => _fieldInfo.FieldType;
        
        protected override void SetValue(
            object obj,
            object value) {_fieldInfo.SetValue(obj, value);}
        protected override object GetValue(
            object obj) {return _fieldInfo.GetValue(obj);}
    }
}
