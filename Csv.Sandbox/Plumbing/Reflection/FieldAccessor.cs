using System;
using System.Reflection;

namespace Csv.Plumbing.Reflection;

public class FieldAccessor : ValueAccessor<FieldInfo>
{
    public FieldAccessor(FieldInfo fieldInfo)
    {
        InnerInfo = fieldInfo;
    }

    public override Type Type => InnerInfo.FieldType;

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