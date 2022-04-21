using System;

namespace Csv.Parser;

public sealed record Settings
{
    public char Separator { get; init; } = ',';
    public bool TrimWhitespace { get; init; }
    public Action<int, int> OnError { get; init; }
}