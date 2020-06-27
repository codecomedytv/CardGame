using CardGame.Client.Library.Cards;

namespace CardGame.Client.Library.Alpha
{
    public class TrainingTrainer: Unit
    {
        public TrainingTrainer()
        {
            Title = "Training Trainer";
            SetCode = SetCodes.AlphaTrainingTrainer;
            Attack = 1000;
            Defense = 1000;
            Text = "When this card is played: You may play one [Pet] from your Deck";
            Illustration = "res://Assets/CardArt/girl.png";
        }
    }
}
