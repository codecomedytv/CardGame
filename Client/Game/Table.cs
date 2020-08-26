using System.Collections.Generic;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class Table: Spatial
    {
        public IPlayer PlayerView { get; }
        public IPlayer OpponentView { get; }
        
        public Table()
        {
            
            var scene = (PackedScene) GD.Load("res://Client/Game/Table.tscn");
            var instance = (Spatial) scene.Instance();
            AddChild(instance);
            PlayerView = (IPlayer) instance.GetNode("PlayMat/Player");
            OpponentView = (IPlayer) instance.GetNode("PlayMat/Opponent");

            var p = (Player) PlayerView;
            p.EnergyIcon = (Sprite) instance.GetNode("HUD/PlayerActive");
            p.OpponentEnergyIcon = (Sprite) instance.GetNode("HUD/OpponentActive");
            PlayerView.DefendingIcon = (Sprite) instance.GetNode("HUD/PlayerDefending");
            OpponentView.DefendingIcon = (Sprite) instance.GetNode("HUD/OpponentDefending");
            PlayerView.LifeBar = (TextureProgress) instance.GetNode("HUD/PlayerLife/Bar");
            PlayerView.LifeCount = (Label) instance.GetNode("HUD/PlayerLife/Count");
            PlayerView.LifeChange = (Label) instance.GetNode("HUD/PlayerLife/Change");
            OpponentView.LifeBar = (TextureProgress) instance.GetNode("HUD/OpponentLife/Bar");
            OpponentView.LifeCount = (Label) instance.GetNode("HUD/OpponentLife/Count");
            OpponentView.LifeChange = (Label) instance.GetNode("HUD/OpponentLife/Change");
        }
    }
}