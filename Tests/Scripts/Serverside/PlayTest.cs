using System;
using System.Collections.Generic;

namespace CardGame.Tests.Scripts.Serverside
{
    public class PlayTest : GameFixture
    {
        private readonly List<SetCodes> DeckList = new List<SetCodes>();

        public override string Title()
        {
            return "Given The Idle Turn Player";
        }

        public override void Start()
        {
            DeckList.Add(SetCodes.Debug500_500);
            DeckList.Add(SetCodes.Debug500_500);
            DeckList.Add(SetCodes.Debug500_500);
            DeckList.Add(SetCodes.Debug1500_1000);
            DeckList.Add(SetCodes.Debug1500_1000);
            DeckList.Add(SetCodes.Debug1500_1000);
            DeckList.Add(SetCodes.DebugDraw2Cards);
            DeckList.Add(SetCodes.DebugDraw2Cards);
            DeckList.Add(SetCodes.DebugDraw2Cards);
            DeckList.Add(SetCodes.Debug1000_1000);
            DeckList.Add(SetCodes.Debug1000_1000);
            DeckList.Add(SetCodes.Debug1000_1000);
        }

        [Test]
        public void When_They_Deploy_A_Unit()
        {
            StartGame(DeckList);
            var unit = Players[1].Hand[0].Id;
            Play.Deploy(Players[1].Id, unit);
            
            Assert.IsEqual(Players[1].Field.Count, 1,"Then Their Field has 1 Unit");
        }

        [Test]
        public void When_They_Set_A_Support()
        {
            StartGame(DeckList);
            var support = Players[1].Hand[4].Id;
            Play.SetFaceDown(Players[1].Id, support);
            
            Assert.IsEqual(Players[1].Support.Count, 1, "Then Their Support has 1 Support");
        }

        [Test]
        public void When_They_End_Their_Turn()
        {
            StartGame(DeckList);
            Play.EndTurn(Players[1].Id);
            
            Assert.IsFalse(Players[1] == Game.GameState.GetTurnPlayer(), "Then They Are Not The Turn Player");
        }

    }
}