using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;
using ItzWarty.Collections;
using System;

namespace Dargon.Hydar.Utilities {
   public abstract class EnvelopeProcessorBase<TEnvelope, TMessageHandler> where TEnvelope : class, InboundEnvelope {
      private readonly MultiValueDictionary<Type, TMessageHandler> messageHandlersByMessageType = new MultiValueDictionary<Type, TMessageHandler>();

      protected void RegisterHandler<TMessage>(TMessageHandler handler) {
         messageHandlersByMessageType.Add(typeof(TMessage), handler);
      }

      public virtual bool Process(TEnvelope envelope) {
         envelope.ThrowIfNull("envelope");
         
         var messageType = GetMessage(envelope).GetType();

         var messageHandler = messageHandlersByMessageType.GetValueOrDefault(messageType);
         if (messageHandler == null) {
            return false;
         } else {
            messageHandler.ForEach(handler => Invoke(handler, envelope));
            return messageHandler.Count > 0;
         }
      }

      protected virtual object GetMessage(TEnvelope envelope) {
         return envelope.Message;
      }

      protected abstract void Invoke(TMessageHandler handler, TEnvelope envelope);
   }
}