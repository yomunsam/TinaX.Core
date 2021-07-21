namespace TinaX.Options
{
    public interface IOptions
    {
        object OptionValue { get; }
    }

    public interface IOptions<TOption> where TOption : class
    {
        TOption Value { get; }
    }
}
