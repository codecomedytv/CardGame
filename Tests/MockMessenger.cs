using System.Collections.Generic;
using CardGame.Server;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Network;

namespace CardGame.Tests
{
    public class MockMessenger: Messenger
    {
        
        // These functions will complain about an inactive network if not overridden when dealing..
        // ..with server-side only tests
        public override void OnPlayExecuted(Player player, Command command) { /* Empty Test Implementation */ }

        public override void Update(IEnumerable<Player> enumerable) { /* Empty Test Implementation */ }

        public override void DisqualifyPlayer(int player, int reason) { /* Empty Test Implementation */}

    }
}