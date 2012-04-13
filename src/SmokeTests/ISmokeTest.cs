namespace CrossroadsIO.SmokeTests
{
    internal interface ISmokeTest
    {
        string Name { get; }

        void Run();
    }
}
