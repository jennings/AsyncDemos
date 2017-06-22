using System;
using System.Collections.Concurrent;
using System.Threading;
using static AsyncDemos.Helper;

namespace AsyncDemos
{
    class SimulationSynchronizationContext : SynchronizationContext, IDisposable
    {
        readonly CancellationTokenSource _cts = new CancellationTokenSource();
        readonly BlockingCollection<Tuple<SendOrPostCallback, object>> _tasks = new BlockingCollection<Tuple<SendOrPostCallback, object>>();
        readonly Thread _worker;

        public SimulationSynchronizationContext()
        {
            _worker = new Thread(RunLoop);
            _worker.Start(_cts.Token);

        }

        public override void Post(SendOrPostCallback d, object state)
        {
            Output($"---- Posting a callback to Thread ID {_worker.ManagedThreadId}");
            _tasks.Add(Tuple.Create(d, state));
        }

        void RunLoop(object tokenObj)
        {
            var cancellationToken = (CancellationToken)tokenObj;
            while (!cancellationToken.IsCancellationRequested)
            {
                Tuple<SendOrPostCallback, object> tuple;
                try
                {
                    tuple = _tasks.Take(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (ObjectDisposedException)
                {
                    return;
                }

                var callback = tuple.Item1;
                var state = tuple.Item2;

                SetSynchronizationContext(this);
                callback.Invoke(state);
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
