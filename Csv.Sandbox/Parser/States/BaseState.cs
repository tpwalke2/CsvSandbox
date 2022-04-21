using System.Linq;

namespace Csv.Parser.States;

public abstract class BaseState : IState
{
    public abstract IState Process(
        char ch,
        Settings settings,
        Context context);

    protected static void AddField(Settings settings, Context context)
    {
        var value                          = context.Buffer.ToString();
        if (settings.TrimWhitespace) value = value.Trim();
        context.CurrentRow.Add(value);
        context.Buffer.Clear();
    }

    protected static void AddRow(Context context)
    {
        context.Rows.Add(context.CurrentRow.ToList());
        context.CurrentRow.Clear();
        context.CurrentColumn = 0;
    }

    protected static void AppendCharacter(char ch, Context context)
    {
        context.Buffer.Append(ch);
        context.CurrentColumn++;
    }
}