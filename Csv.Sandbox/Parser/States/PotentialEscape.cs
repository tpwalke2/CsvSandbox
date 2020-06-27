namespace Csv.Parser.States
{
    public class PotentialEscape : BaseState
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
                return ParseStates.Start;
            }

            switch (ch)
            {
                case Constants.CarriageReturn:
                    return this;
                case Constants.LineFeed:
                    AddField(settings, context);
                    AddRow(context);
                    return ParseStates.Start;
                case Constants.DoubleQuote:
                    AppendCharacter(ch, context);
                    return ParseStates.QuotedField;
                default:
                    return ParseStates.Error;
            }
        }
    }
}
