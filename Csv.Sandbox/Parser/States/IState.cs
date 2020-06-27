namespace Csv.Parser.States
{
    public interface IState
    {
        IState Process(
            char ch,
            Settings settings,
            Context context);
    }
}
