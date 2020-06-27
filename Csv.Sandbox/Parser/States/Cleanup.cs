namespace Csv.Parser.States
{
    public class Cleanup : BaseState
    {
        public override IState Process(
            char ch,
            Settings settings,
            Context context)
        {
            if (context.Buffer.Length <= 0) return this;
            AddField(settings, context);
            AddRow(context);
            return this;
        }
    }
}
