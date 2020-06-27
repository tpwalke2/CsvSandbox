using System;
using System.Collections.Generic;

namespace Csv.Extensions
{
    public static class ObjectExtensions
    {
        public static TResult Pipe<T, TResult>(this T value, Func<T, TResult> pipeFunction)
        {
            return pipeFunction(value);
        }
        
        public static void Pipe<T>(this T value, Action<T> pipeFunction)
        {
            pipeFunction(value);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> func)
        {
            foreach (var item in enumerable)
            {
                func(item);
            }
        }
    }
}
