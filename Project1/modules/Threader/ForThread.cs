using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Threading;
using System.Threading.Tasks;
using Galaxy.modules;

namespace Debuger.modules.Threader
{
   
    class ForThread
    {
        public static async void  For( int start , int limit , int adder , Action<int> action)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
             Task task;
                task = Task.Run(() =>
                {
                    for (int i = start; i < limit; i += adder)
                    {
                        if (!cts.Token.IsCancellationRequested)
                            action(i);
                        else
                            break;
                    }
                    cts.Cancel();
                    cts.Dispose();
                }, cts.Token);
            await task;
            task.Dispose();
            task = null;
            cts =null;
        }
       
    public static async void  ForN( int start , int limit , int adder , Action<int> action)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
             Task task;
                task = Task.Run(() =>
                {
                    for (int i = start; i > limit; i -= adder)
                    {
                        if (!cts.Token.IsCancellationRequested)
                            action(i);
                        else
                            break;
                    }
                    cts.Cancel();
                    cts.Dispose();
                }, cts.Token);
            await task;
            task.Dispose();
            task = null;
            cts =null;
        }
       

        public static void Foreach<T>( List<T> list, Action<T> action)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Task task = Task.Run(() => {
                foreach (T item in list)
                {
                    if (!cts.Token.IsCancellationRequested)
                        action(item);
                    else
                        break;
                }
                cts.Cancel();
                task = null;
                task.Dispose();
                cts.Dispose();
                cts = null;
            }, cts.Token);
        }
    }
}
