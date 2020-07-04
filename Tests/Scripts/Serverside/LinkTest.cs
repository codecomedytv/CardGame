
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
            deckList.Add(SetCodes.Debug500500);
            deckList.Add(SetCodes.Debug500500);
            deckList.Add(SetCodes.Debug500500);
            deckList.Add(SetCodes.DebugDestroyAttackingUnit);
            deckList.Add(SetCodes.DebugDestroyAttackingUnit);
            deckList.Add(SetCodes.DebugDestroyAttackingUnit);
            deckList.Add(SetCodes.Debug15001000);
            deckList.Add(SetCodes.Debug15001000);
            deckList.Add(SetCodes.Debug15001000);
            StartGame(deckList);
            var attacker = Player.Hand[6];
            var chainMine = Opponent.Hand[3];
            Play.Deploy(Player.Id, attacker.Id);
            Play.PassPlay(Opponent.Id);
            Play.PassPlay(Player.Id);
            Play.EndTurn(Player.Id);
            Play.SetFaceDown(Opponent.Id, chainMine.Id);
            Play.EndTurn(Opponent.Id);
            Play.AttackDirectly(Player.Id, attacker.Id);
            Play.Activate(Opponent.Id, chainMine.Id);
            Play.PassPlay(Player.Id);
            Play.PassPlay(Opponent.Id);
            
            // Link Should Resolve Here
            Assert.Has(attacker, Player.Graveyard, attacker + "is in Player 1's Discard");
            Assert.Has(chainMine, Opponent.Graveyard, chainMine + " is in Player 0's Discard");
        }
    }
}