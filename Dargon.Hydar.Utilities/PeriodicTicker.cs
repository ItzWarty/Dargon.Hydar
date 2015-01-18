using System;
using ItzWarty.Threading;

namespace Dargon.Hydar.Utilities {
   public interface PeriodicTicker {
      event EventHandler Tick;
   }

   public class PeriodicTickerImpl : PeriodicTicker, IDisposable {
      private readonly IThreadingProxy threadingProxy;
      private readonly PeriodicTickerConfiguration hydarConfiguration;
      private readonly IThread thread;
      private readonly ICancellationTokenSource cancellationTokenSource;
      public event EventHandler Tick;

      public PeriodicTickerImpl(IThreadingProxy threadingProxy, PeriodicTickerConfiguration hydarConfiguration) {
         this.threadingProxy = threadingProxy;
         this.hydarConfiguration = hydarConfiguration;

         thread = threadingProxy.CreateThread(TickerThreadEntryPoint, new ThreadCreationOptions() { IsBackground = true });
         cancellationTokenSource = threadingProxy.CreateCancellationTokenSource();
      }

      public void Initialize() {
         thread.Start();
      }

      internal void TickerThreadEntryPoint() {
         while (!cancellationTokenSource.IsCancellationRequested) {
            var capture = Tick;
            if (capture != null) {
               capture(this, EventArgs.Empty);
            }

            threadingProxy.Sleep(hydarConfiguration.TickIntervalMillis);
         }
      }

      public void Dispose() {
         cancellationTokenSource.Dispose();
         thread.Join();
      }
   }
}
