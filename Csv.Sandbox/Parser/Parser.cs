using System.Collections.Generic;
using System.Linq;
using Csv.Parser.States;

namespace Csv.Parser
{
    public static class Parser
    {
        public static IList<IList<string>> Parse(string input, Settings settings = null)
        {
            return DoParse(
                input,
                settings ?? new Settings(),
                new Context());
        }

        private static IList<IList<string>> DoParse(
            string input,
            Settings settings,
            Context context)
        {
            input.Aggregate(
                ParseStates.Start,
                (current, ch) => current.Process(ch, settings, context));

            ParseStates.Cleanup.Process(default(char), settings, context);

            return context.Rows;
        }
    }
}
