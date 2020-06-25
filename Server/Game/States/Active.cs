using CardGame.Client.Match;
using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server.States
{
    public class Active: State
    {
        public override void OnEnter(Player player)
        {
        }

        public override string ToString()
        {
            return "Active";
        }
    }
}
