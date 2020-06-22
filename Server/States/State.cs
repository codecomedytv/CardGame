using System.Security.Policy;
using CardGame.Server.Room;
using CardGame.Server.Room.Cards;
using Godot.Collections;

namespace CardGame.Server.States
{
    public class State: Godot.Object
    {
        protected const bool Ok = false;
        protected const bool DisqualifyPlayer = true; 
        protected Player Player;
        public Link Link => Player.Game.Link;
        public Battle Battle => Player.Game.Battle;

        public virtual void OnEnter(Player player)
        {
            Player = player;
        }

        public virtual bool OnDeploy(Unit unit)
        {
            return DisqualifyPlayer;
        }

        public virtual bool OnAttack(Unit unit, Unit defender)
        {
            return DisqualifyPlayer;
        }
        
        public virtual bool OnDirectAttack(Unit attacker)
        {
            return DisqualifyPlayer;
        }
        
        public virtual bool OnActivation(Support card, Card target)
        {
            return DisqualifyPlayer;
        }

        public virtual bool OnSetFaceDown(Support card)
        {
            return DisqualifyPlayer;
        }

        public virtual bool OnPassPlay()
        {
            return DisqualifyPlayer;
        }

        public virtual bool OnEndTurn()
        {
            return DisqualifyPlayer;
        }

        public override string ToString()
        {
            return "State";
        }
 

    }
}
