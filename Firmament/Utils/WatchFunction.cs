using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firmament.Utils
{
    public class WatchFunction
    {
        public static void MeasureExecutionTime(Action action)
        {
            Stopwatch stopwatch = new Stopwatch();
            // 开始计时
            stopwatch.Start();
            // 执行函数
            action.Invoke();
            // 停止计时
            stopwatch.Stop();
            // 输出执行时间
            Console.WriteLine($"Execution time: {stopwatch.Elapsed}");
        }

        public static TResult MeasureExecutionTime<T, TResult>(Func<T, TResult> function, T parameter)
        {
            Stopwatch stopwatch = new Stopwatch();
            // 开始计时
            stopwatch.Start();
            // 执行函数
            TResult result = function.Invoke(parameter);
            // 停止计时
            stopwatch.Stop();
            // 输出执行时间
            Console.WriteLine($"Execution time: {stopwatch.Elapsed}");
            // 返回函数结果
            return result;
        }

        private static void Main()
        {
            //// 调用需要测量执行时间的函数
            //int result = MeasureExecutionTime(function, 100);
            //// 输出函数的执行结果
            //Console.WriteLine($"Function result: {result}");
        }
    }
}
