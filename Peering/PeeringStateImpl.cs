using System;
using Dargon.Hydar.Peering.Messages;
using ItzWarty.Collections;

namespace Dargon.Hydar.Peering {
   public class PeeringStateImpl : ManageablePeeringState {
      private readonly PeerStatusFactory peerStatusFactory;
      private readonly ConcurrentDictionary<Guid, ManageablePeerStatus> peerStatusesByPeerId = new ConcurrentDictionary<Guid, ManageablePeerStatus>();

      public PeeringStateImpl(PeerStatusFactory peerStatusFactory) {
         this.peerStatusFactory = peerStatusFactory;
      }

      public IReadOnlySet<Guid> EnumeratePeerGuids() {
         return new HashSet<Guid>(peerStatusesByPeerId.Keys);
      }

      public void HandlePeeringAnnounce(Guid senderId, uint senderAddress, PeeringAnnounce peeringAnnounce) {
         var peerStatus = GetPeerStatusInternal(senderId);
         peerStatus.Update(senderAddress, peeringAnnounce);
      }

      public PeerStatus GetPeerStatus(Guid peerId) {
         return GetPeerStatusInternal(peerId);
      }

      private ManageablePeerStatus GetPeerStatusInternal(Guid peerId) {
         return peerStatusesByPeerId.GetOrAdd(peerId, peerStatusFactory.Create);
      }
   }
}