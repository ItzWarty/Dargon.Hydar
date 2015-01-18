using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dargon.Hydar.Utilities {
   public class LockGuard : IDisposable {
      private object synchronization;

      public LockGuard(object synchronization) {
         this.synchronization = synchronization;

         Monitor.Enter(synchronization);
      }

      ~LockGuard() {
         Release();
      }

      public void Release() {
         lock (this) {
            if (synchronization != null) {
               Monitor.Exit(synchronization);
               synchronization = null;
            }
         }
      }

      void IDisposable.Dispose() {
         Release();
      }
   }
}
