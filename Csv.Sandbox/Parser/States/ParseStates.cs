namespace Csv.Parser.States;

public static class ParseStates
{
    public static readonly IState Start = new Start();
    public static readonly IState UnquotedField = new UnquotedField();
    public static readonly IState QuotedField = new QuotedField();
    public static readonly IState PotentialEscape = new PotentialEscape();
    public static readonly IState Cleanup = new Cleanup();
    public static readonly IState Error = new Error();
}