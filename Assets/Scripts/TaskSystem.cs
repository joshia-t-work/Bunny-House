using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

namespace BunnyHouse.Core
{
    public class TaskSystem : MonoSingleton
    {
        static CancellationTokenSource Abort = new CancellationTokenSource();
        static Queue<System.Exception> Exceptions = new Queue<System.Exception>();
        static TaskCompletionSource<bool> FinishedTasks = new TaskCompletionSource<bool>();
        static int RunningTasks = 0;
        public static CancellationToken CancellationToken => Abort.Token;
        public static void Check()
        {
            Abort.Token.ThrowIfCancellationRequested();
        }
        public static Task StopAll()
        {
            Abort.Cancel();
            FinishedTasks = new TaskCompletionSource<bool>();
            if (RunningTasks == 0)
            {
                FinishedTasks.TrySetResult(true);
            }
            Abort = new CancellationTokenSource();
            return FinishedTasks.Task;
        }
        public static async void Cancellable(System.Func<Task> func)
        {
            try
            {
                await func();
            }
            catch (TaskCanceledException)
            {
            }
        }
        //public override void MonoAwake()
        //{
        //    _ = Run(async () =>
        //    {
        //        await Task.Delay(1000);
        //        throw new System.Exception();
        //    });
        //}
        public override void MonoUpdate()
        {
            while (Exceptions.Count > 0)
            {
                throw Exceptions.Dequeue();
            }
            while (Main.Actions.Count > 0)
            {
                Main.Actions.Dequeue().Invoke();
            }
        }
        public async static Task Run(System.Func<Task> func)
        {
            try
            {
                RunningTasks += 1;
                await func();
                RunningTasks -= 1;
                if (RunningTasks == 0)
                {
                    FinishedTasks.TrySetResult(true);
                }
            }
            catch (System.Exception e)
            {
                Exceptions.Enqueue(e);
            }
        }
        public async static Task Run(System.Func<System.Action, Task> func)
        {
            try
            {
                RunningTasks += 1;
                await func(Check);
                RunningTasks -= 1;
                if (RunningTasks == 0)
                {
                    FinishedTasks.TrySetResult(true);
                }
            }
            catch (System.Exception e)
            {
                Exceptions.Enqueue(e);
            }
        }
        public static class Main
        {
            public static Queue<System.Action> Actions = new Queue<System.Action>();
            public static void Run(System.Action action)
            {
                Actions.Enqueue(action);
            }
        }

        //static void Main()
        //{
        //    CancellationToken ct = Abort.Token;
        //    Task.Factory.StartNew(() =>
        //    {
        //        while (true)
        //        {
        //            // do some heavy work here
        //            Thread.Sleep(100);
        //            if (ct.IsCancellationRequested)
        //            {
        //                // another thread decided to cancel
        //                Console.WriteLine("task canceled");
        //                break;
        //            }
        //        }
        //    }, ct);

        //    // Simulate waiting 3s for the task to complete
        //    Thread.Sleep(3000);

        //    // Can't wait anymore => cancel this task 
        //    ts.Cancel();
        //    Console.ReadLine();
        //}
    }
}