namespace Csv.Parser.States;

public class QuotedField : BaseState
{
    public override IState Process(
        char ch,
        Settings settings,
        Context context)
    {
        if (ch == Constants.DoubleQuote)
        {
            context.CurrentColumn++;
            return ParseStates.PotentialEscape;
        }

        AppendCharacter(ch, context);
        return this;
    }
}