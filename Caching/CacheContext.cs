using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;
using System;

namespace Dargon.Hydar.Caching {
   public interface CacheContext {
      string Name { get; }
      Guid Id { get; }

      void Dispatch(InboundEnvelope e);
   }

   public class CacheContextImpl : EnvelopeProcessorBase<InboundEnvelope, Action<InboundEnvelope>>, CacheContext {
      private readonly string name;
      private readonly Guid id;

      public CacheContextImpl(string name, Guid id) {
         this.name = name;
         this.id = id;
      }

      public string Name { get { return name; } }
      public Guid Id { get { return id; } }

      public void Initialize() {
      }

      protected override void Invoke(Action<InboundEnvelope> handler, InboundEnvelope envelope) {
         handler(envelope);
      }

      public void Dispatch(InboundEnvelope e) {
         Process(e);
      }
   }
}
