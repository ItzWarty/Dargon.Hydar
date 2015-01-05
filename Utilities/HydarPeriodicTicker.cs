using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dargon.Hydar.Clustering;
using ItzWarty.Threading;

namespace Dargon.Hydar.Utilities {
   public interface HydarPeriodicTicker {
      event EventHandler Tick;
   }

   public class HydarPeriodicTickerImpl : HydarPeriodicTicker, IDisposable {
      private readonly IThreadingProxy threadingProxy;
      private readonly HydarConfiguration hydarConfiguration;
      private readonly IThread thread;
      private readonly ICancellationTokenSource cancellationTokenSource;
      public event EventHandler Tick;

      public HydarPeriodicTickerImpl(IThreadingProxy threadingProxy, HydarConfiguration hydarConfiguration) {
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
