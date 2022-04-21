namespace Csv.Parser.States;

public class Start : BaseState
{
    public override IState Process(
        char ch,
        Settings settings,
        Context context)
    {
        if (ch == settings.Separator)
        {
            context.CurrentColumn++;
            AddField(settings, context);
            return this;
        }

        switch (ch)
        {
            case Constants.CarriageReturn:
                return this;
            case Constants.LineFeed:
                if (context.Buffer.Length > 0) AddField(settings, context);
                if (context.CurrentRow.Count > 0) AddRow(context);
                return this;
            case Constants.DoubleQuote:
                context.CurrentColumn++;
                return ParseStates.QuotedField;
            default:
                AppendCharacter(ch, context);
                return ParseStates.UnquotedField;
        }
    }
}