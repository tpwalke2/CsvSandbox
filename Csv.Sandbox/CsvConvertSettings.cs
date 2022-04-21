using System;

namespace Csv;

public sealed record CsvConvertSettings
{
    public Action<string> OnError { get; init; }
    public char Separator { get; init; } = ',';
    public bool EmitHeader { get; init; } = true;
}