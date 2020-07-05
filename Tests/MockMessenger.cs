using System.Collections.Generic;
using CardGame.Server;
using CardGame.Server.Game;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;

namespace CardGame.Tests
{
    public class MockMessenger: Messenger
    {
        
        // These functions will complain about an inactive network if not overridden when dealing..
        // ..with server-side only tests
        public override void OnPlayExecuted(Event Event) { /* Empty Test Implementation */ }

        public override void Update(IEnumerable<Card> cards, IEnumerable<Player> enumerable) { /* Empty Test Implementation */ }

        public override void DisqualifyPlayer(int player) { /* Empty Test Implementation */}

        public new void Activate(int id, int card, int target = 0) => base.Activate(id, card, target);

    }
}