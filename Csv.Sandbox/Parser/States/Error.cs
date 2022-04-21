using System;

namespace Csv.Parser.States;

public class Error : BaseState
{
    public override IState Process(
        char ch,
        Settings settings,
        Context context)
    {
        if (settings.OnError == null)
            throw new FormatException(
                $"Error parsing CSV at line '{context.Rows.Count + 1}' near column '{context.CurrentColumn}'");

        settings.OnError(context.Rows.Count + 1, context.CurrentColumn);
        return this;
    }
}