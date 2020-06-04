
using System.Collections.Generic;
using Godot.Collections;

namespace CardGame.Tests.Scripts.Serverside
{
    public class LinkTest: GameFixture
    {
        public override string Title()
        {
            return "Triggered Event Cards";
        }

        [Test]
        public void OnAttack()
        {
            
        // This test allows us to check that Chain Mines destroyed the attacking monster
        // However it doesn't absolutely confirm that this activation was legal (we know it is but the game doesn't know for sure)
        // We likely need to create a dedicated error handler system with err_codes, signals etc.
            var deckList = new List<SetCodes>();
            deckList.Add(SetCodes.Debug500_500);
            deckList.Add(SetCodes.Debug500_500);
            deckList.Add(SetCodes.Debug500_500);
            deckList.Add(SetCodes.DebugDestroyAttackingUnit);
            deckList.Add(SetCodes.DebugDestroyAttackingUnit);
            deckList.Add(SetCodes.DebugDestroyAttackingUnit);
            deckList.Add(SetCodes.Debug1500_1000);
            deckList.Add(SetCodes.Debug1500_1000);
            deckList.Add(SetCodes.Debug1500_1000);
            StartGame(deckList);
            var attacker = Players[1].Hand[6];
            var chainMine = Players[0].Hand[3];
            Play.Deploy(Players[1].Id, attacker.Id);
            Play.PassPlay(Players[1].Id);
            Play.PassPlay(Players[0].Id);
            Play.EndTurn(Players[1].Id);
            Play.SetFaceDown(Players[0].Id, chainMine.Id);
            Play.EndTurn(Players[0].Id);
            const int targetDirectAttack = -1;
            Play.Attack(Players[1].Id, attacker.Id, targetDirectAttack);
            Play.Activate(Players[0].Id, chainMine.Id, new Array<int>());
            Play.PassPlay(Players[1].Id);
            Play.PassPlay(Players[0].Id);
            
            // Link Should Resolve Here
            Assert.Has(attacker, Players[1].Graveyard, attacker + "is in Player 1's Discard");
            Assert.Has(chainMine, Players[0].Graveyard, chainMine + " is in Player 0's Discard");
        }
    }
}