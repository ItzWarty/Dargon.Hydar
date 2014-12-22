using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;
using ItzWarty.Collections;
using System;

namespace Dargon.Hydar.Utilities {
   public abstract class MessageProcessorBase<TMessage, THandler>
      where TMessage : class, HydarMessage {
      protected readonly AuditEventBus auditEventBus;
      protected readonly HydarContext context;
      protected readonly NetworkNode node;
      protected readonly Network network;
      private readonly MultiValueDictionary<Type, THandler> messageHandlersByPayloadType = new MultiValueDictionary<Type, THandler>();

      protected MessageProcessorBase(AuditEventBus auditEventBus, HydarContext context) {
         this.auditEventBus = auditEventBus;
         this.context = context;
         this.node = context.Node;
         this.network = context.Network;
      }

      protected void RegisterHandler<TPayload>(THandler handler) {
         messageHandlersByPayloadType.Add(typeof(TPayload), handler);
      }

      public bool Process(IRemoteIdentity sender, TMessage message) {
         message.ThrowIfNull("message");
         message.Header.ThrowIfNull("header");
         message.Payload.ThrowIfNull("payload");

         var payloadType = message.Payload.GetType();

         var handlers = messageHandlersByPayloadType.GetValueOrDefault(payloadType);
         if (handlers == null) {
            return false;
         } else {
            handlers.ForEach(handler => Invoke(handler, sender, message));
            return handlers.Count > 0;
         }
      }

      protected abstract void Invoke(THandler handler, IRemoteIdentity sender, TMessage message);

      public void Log(string s) {
         context.Log(s);
      }
   }
}