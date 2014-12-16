using System;
using System.Collections.Generic;
using Dargon.Audits;
using Dargon.Hydar.Grid.ClusterPhases;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;

namespace Dargon.Hydar.Grid {
   public class MessageProcessorBase {
      protected readonly AuditEventBus auditEventBus;
      protected readonly NodePhaseFactory phaseFactory;
      protected readonly HydarContext context;
      protected readonly GridConfiguration configuration;
      protected readonly HydarNode node;
      protected readonly Network network;
      private readonly Dictionary<Type, Action<IRemoteIdentity, HydarMessageHeader, object>> messageHandlersByPayloadType = new Dictionary<Type, Action<IRemoteIdentity, HydarMessageHeader, object>>();

      public MessageProcessorBase(AuditEventBus auditEventBus, NodePhaseFactory phaseFactory, HydarContext context) {
         this.auditEventBus = auditEventBus;
         this.phaseFactory = phaseFactory;
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

         var handler = messageHandlersByPayloadType.GetValueOrDefault(payloadType);
         if (handler == null) {
            return false;
         } else {
            handler(sender, message.Header, message.Payload);
            return true;
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