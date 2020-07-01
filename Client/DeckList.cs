using System.Collections;
using System.Collections.Generic;

namespace CardGame.Client
{
    public class DeckList: IEnumerable<SetCodes>
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

        public IEnumerator<SetCodes> GetEnumerator()
        {
            if (CardsInDeckList.Count == 0)
            {
                SetDefault();
            }
            return CardsInDeckList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

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
    }
}