using System;
using Dargon.Audits;
using Dargon.Hydar.Clustering;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar.Utilities {
   public class MessageProcessorBase {
      protected readonly AuditEventBus auditEventBus;
      protected readonly HydarContext context;
      protected readonly ClusteringConfiguration configuration;
      protected readonly NetworkNode node;
      protected readonly Network network;
      private readonly MultiValueDictionary<Type, Action<IRemoteIdentity, HydarMessageHeader, object>> messageHandlersByPayloadType = new MultiValueDictionary<Type, Action<IRemoteIdentity, HydarMessageHeader, object>>();

      public MessageProcessorBase(AuditEventBus auditEventBus, HydarContext context) {
         this.auditEventBus = auditEventBus;
         this.context = context;
         this.configuration = context.Configuration;
         this.node = context.Node;
         this.network = context.Network;
      }

      protected void RegisterHandler<TPayload>(Action<IRemoteIdentity, HydarMessageHeader, TPayload> handler) {
         messageHandlersByPayloadType.Add(typeof(TPayload), (identity, header, payload) => handler(identity, header, (TPayload)payload));
      }

      public bool Process(IRemoteIdentity sender, HydarMessage message) {
         message.ThrowIfNull("message");
         message.Header.ThrowIfNull("header");
         message.Payload.ThrowIfNull("payload");

         var payloadType = message.Payload.GetType();

         var handlers = messageHandlersByPayloadType.GetValueOrDefault(payloadType);
         if (handlers == null) {
            return false;
         } else {
            handlers.ForEach(handler => handler(sender, message.Header, message.Payload));
            return handlers.Count > 0;
         }
      }

      public void Send<TPayload>(TPayload payload) {
         var header = new HydarMessageHeaderImpl(node.Identifier);
         network.Broadcast(node, new HydarMessageImpl<TPayload>(header, payload));
      }

      public void Log(string s) {
         context.Log(s);
      }
   }
}