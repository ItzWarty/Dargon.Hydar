using System;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Clustering.Phases {
   public abstract class PhaseBase : MessageProcessorBase<HydarMessage, Action<IRemoteIdentity, HydarMessageHeader, object>>, IPhase {
      protected readonly NodePhaseFactory phaseFactory;
      protected readonly ManageableClusterContext clusterContext;
      protected readonly ClusteringConfiguration configuration;

      protected PhaseBase(AuditEventBus auditEventBus, HydarContext context, ManageableClusterContext manageableClusterContext, NodePhaseFactory phaseFactory) 
         : base (auditEventBus, context) {
         this.phaseFactory = phaseFactory;
         this.clusterContext = manageableClusterContext;
         this.configuration = context.Configuration;
      }

      public virtual void Initialize() { }
      public virtual void Enter() { }
      public abstract void Tick();

      protected void RegisterHandler<TPayload>(Action<IRemoteIdentity, HydarMessageHeader, TPayload> handler) {
         base.RegisterHandler<TPayload>((identity, header, payload) => handler(identity, header, (TPayload)payload));
      }

      protected void RegisterNullHandler<TPayload>() {
         base.RegisterHandler<TPayload>((x, y, z) => { });
      }

      protected override void Invoke(Action<IRemoteIdentity, HydarMessageHeader, object> handler, IRemoteIdentity sender, HydarMessage message) {
         handler(sender, message.Header, message.Payload);
      }

      protected void Send<TPayload>(TPayload payload) {
         var header = new HydarMessageHeaderImpl(node.Identifier);
         network.Broadcast(node, new HydarMessageImpl<TPayload>(header, payload));
      }
   }
}
