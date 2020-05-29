using CardGame.Client.Library.Card;
using CardGame.Client.Match;
using Godot;
using Godot.Collections;

namespace CardGameSharp.Client.Game
{
    public class Opponent: Reference
    {
        public Array<Card> Field = new Array<Card>();
        public Visual Visual;
    }
}