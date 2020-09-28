using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace CardGame.Client
{
    public class DeckList //IEnumerable<SetCodes>
    {
        private readonly IList<SetCodes> CardsInDeckList = new List<SetCodes>();

        public void Add(SetCodes setCode, int copies = 1)
        {
            for (var i = 0; i < copies; i++)
            {
                CardsInDeckList.Add(setCode);
            }
        }

        public void Clear() => CardsInDeckList.Clear();

        public int Count => CardsInDeckList.Count;

        // public IEnumerator<SetCodes> GetEnumerator()
        // {
        //     Console.WriteLine($"There are {CardsInDeckList.Count} in the DeckList");
        //     if (CardsInDeckList.Count == 0)
        //     {
        //         SetDefault();
        //     }
        //     return CardsInDeckList.GetEnumerator();
        // }

        // IEnumerator IEnumerable.GetEnumerator()
        // {
        //     return GetEnumerator();
        // }

        private void SetDefault()
        {
            Add(SetCodes.AlphaDungeonGuide, 34);
            Add(SetCodes.AlphaGuardPuppy);
            Add(SetCodes.AlphaWrongWay);
            Add(SetCodes.AlphaCounterAttack);
            Add(SetCodes.AlphaQuestReward);
            Add(SetCodes.AlphaNoviceArcher);
            Add(SetCodes.AlphaTrainingTrainer);
        }

        public Array<SetCodes> ToArray()
        {
            if (CardsInDeckList.Count == 0)
            {
                // We can't pass the decklist via RPC as IEnumerable in 3.2.3 to marshalling issues..
                // ..so we have to move the default setting down here
                SetDefault();
            }
            var deckList = new Array<SetCodes>();
            foreach (var setCode in CardsInDeckList)
            {
                deckList.Add(setCode);
            }

            return deckList;
        }
    }
}