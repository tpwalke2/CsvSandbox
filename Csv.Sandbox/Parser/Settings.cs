using System;

namespace Csv.Parser
{
    public class Settings
    {
        public char Separator { get; set; } = ',';
        public bool TrimWhitespace { get; set; }
        public Action<int, int> OnError { get; set; }
    }
}
