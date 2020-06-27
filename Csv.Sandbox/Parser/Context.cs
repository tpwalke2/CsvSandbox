using System.Collections.Generic;
using System.Text;

namespace Csv.Parser
{
    public class Context
    {
        public StringBuilder Buffer { get; } = new StringBuilder();
        public IList<IList<string>> Rows { get; } = new List<IList<string>>();
        public IList<string> CurrentRow { get; } = new List<string>();
        public int CurrentColumn { get; set; } = 0;
    }
}
