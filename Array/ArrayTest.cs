using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace CacheMiss.Array
{ 
    public class ArrayTest
    {
        private const int Max = 10240;
        private readonly long[,] _numbers = new long[Max,Max]; 

        [Benchmark]
        public void RowMajor()
        {
            for (var i = 0; i < Max; i++)
            {
                for (var j = 0; j < Max; j++)
                {
                    _numbers[i, j] = i + j;
                }
            }
        }

        [Benchmark]
        public void ColumnMajor()
        {
            for (var i = 0; i < Max; i++)
            {
                for (var j = 0; j < Max; j++)
                {
                    _numbers[j, i] = i + j;
                }
            }
        }

        [Benchmark]
        public void LoopBlocking()
        {
            const int blockSize = 8;
            for (var i = 0; i < Max; i += blockSize)
            {
                for (var j = 0; j < Max; j += blockSize)
                {
                    for (var ii = i; ii < i + blockSize; ii++)
                    {
                        for (var jj = j; jj < j + blockSize; jj++)
                        {
                            _numbers[j, i] = i + j;
                        }
                    }
                }
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ArrayTest>(new DebugInProcessConfig());
        }
    }
}