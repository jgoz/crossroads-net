namespace CrossroadsIO.SmokeTests
{
    using System;

    internal class Program
    {
        public static void Main(string[] args)
        {
            RunTests(
                new HelloWorld(),
                new LatencyBenchmark(),
                new ThroughputBenchmark());

            Console.WriteLine("Finished running tests.");
            Console.ReadLine();
        }

        private static void RunTests(params ISmokeTest[] smokeTests)
        {
            foreach (ISmokeTest test in smokeTests)
            {
                Console.WriteLine("Running test {0}...", test.Name);
                Console.WriteLine();
                test.Run();
                Console.WriteLine();
            }
        }
    }
}
