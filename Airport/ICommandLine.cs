namespace Airport
{
    public interface ICommandLine
    {
        void Write(string text);

        string Read();
    }
}