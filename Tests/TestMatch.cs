using CardGame.Client.Game;
using CardGame.Client.Game.Players;
using Godot;


namespace CardGame.Tests
{
    public class TestMatch: Match
    {
        public Player Player;
        public Opponent Opponent;
        public Messenger Messenger;

        public override void _Ready()
        {
            base._Ready();
            Player = GetNode<Player>("Spatial/Table3D/PlayMat/Player");
            Opponent = GetNode<Opponent>("Spatial/Table3D/PlayMat/Opponent");
            Messenger = GetNode<Messenger>("Messenger");
        }
    }
}