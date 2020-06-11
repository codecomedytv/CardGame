using System.Security.Policy;
using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server.States
{
    public class State: Godot.Object
    {
        protected const bool Ok = false;
        protected const bool DisqualifyPlayer = true; 
        protected Player Player;

        public virtual void OnEnter(Player player)
        {
            Player = player;
        }

        public virtual bool OnDeploy(Unit unit)
        {
            return DisqualifyPlayer;
        }

        public virtual bool OnAttack(Unit unit, object defender, bool isDirectAttack)
        {
            return DisqualifyPlayer;
        }

        public virtual bool OnActivation(Support card, Array<int> targets)
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