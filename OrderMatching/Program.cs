namespace OrderMatching
{
    class Program
    {
        static void RunDemo()
        {
            var demo = new DemoTest();
            demo.RunDemo();
        }
        static void Main(string[] args)
        {
            PerformanceTest.StartBenchmark();
            //RunDemo();
        }
    }
}
