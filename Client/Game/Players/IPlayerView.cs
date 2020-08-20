using CardGame.Client.Game.Cards;

namespace CardGame.Client.Game.Players
{
    public interface IPlayerView
    {
        void DisplayName(string name);
        void DisplayHealth(int health);

        void Draw(ICardView card);
        void Discard(ICardView card);
        void Deploy(ICardView card);
        void Set(ICardView card);
        void Attack(ICardView attacker, ICardView defender);
        void AttackDirectly(ICardView attacker);
    }
}