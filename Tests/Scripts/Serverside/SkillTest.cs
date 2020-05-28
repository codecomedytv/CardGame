using System.Collections.Generic;
using Godot.Collections;

namespace CardGame.Tests.Scripts.Serverside
{
    public class SkillTest: GameFixture
    {
        private List<SetCodes> DeckList = new List<SetCodes>();

        public override string Title()
        {
            return "When using a Skill";
        }

        public override void Pre()
        {
            DeckList.Clear();
            DeckList.Add(SetCodes.Debug1000_1000);
            DeckList.Add(SetCodes.Debug1000_1000);
            DeckList.Add(SetCodes.Debug1000_1000);
            DeckList.Add(SetCodes.Debug500_500);
            DeckList.Add(SetCodes.Debug500_500);
            DeckList.Add(SetCodes.Debug500_500);
            DeckList.Add(SetCodes.Debug1500_1000);
            DeckList.Add(SetCodes.Debug1500_1000);
            DeckList.Add(SetCodes.Debug1500_1000);
 
        }

        [Test]
        public void That_Destroys_Target_Opponent_Unit()
        {
            DeckList.Add(SetCodes.DebugDestroyOpponentUnit);
            StartGame(DeckList);
            
            var support = Players[1].Hand[0].Id;
            Play.SetFaceDown(Players[1].Id, support);
            Play.EndTurn(Players[1].Id);
            var unitA = Players[0].Hand[1];
            var unitB = Players[0].Hand[6];
            Play.Deploy(Players[0].Id, unitA.Id);
            Play.PassPlay(Players[1].Id);
            Play.PassPlay(Players[0].Id);
            Play.Deploy(Players[0].Id, unitB.Id);
            Play.PassPlay(Players[1].Id);
            Play.PassPlay(Players[0].Id);
            Play.EndTurn(Players[0].Id);
            var targets = new Array<int>();
            targets.Add(unitA.Id);
            Assert.IsEqual(targets.Count, 1, "Target Count is 1");
            Play.Activate(Players[1].Id, support, 0, targets);
            Play.PassPlay(Players[0].Id);
            Play.PassPlay(Players[1].Id);
            
            Assert.Has(unitA, Players[0].Graveyard, "Then That Unit Is Destroyed");

        }
    }
}
