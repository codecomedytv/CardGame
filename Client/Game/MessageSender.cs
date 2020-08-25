using Godot;

namespace CardGame.Client.Game
{
    public class MessageSender: Object
    {
        // Controls Messages Being Sent To The Server
        private const int ServerId = 1;
        private readonly Messenger Messenger;
        private int Id => Messenger.Multiplayer.GetNetworkUniqueId();

        public MessageSender(Messenger messenger)
        {
            Messenger = messenger;
        }

        public void DeclareDeploy(int cardId)
        {
            Messenger.RpcId(ServerId, "OnDeployDeclared", Id, cardId);
        }

        public void DeclareSet(int cardId)
        {
            Messenger.RpcId(ServerId, "OnSetFaceDownDeclared", Id, cardId);
        }

        public void DeclareActivation(int cardId, int targetId)
        {
            Messenger.RpcId(ServerId, "OnActivationDeclared", Id, cardId, targetId);
        }
    }
}