using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardGame.Client.Cards;
using CardGame.Tests.Scripts;
using Godot;
using Object = Godot.Object;

namespace CardGame.Client.Room.Commands
{
    public class CommandQueue: Object
    {
	    private delegate Card Fetch(int id = 0, SetCodes setCode = SetCodes.NullCard);
	    private readonly Fetch GetCard;
        private readonly Player Player;
        private readonly Player Opponent;
        private readonly Tween Gfx;

        private readonly Queue<Command> Commands = new Queue<Command>();


        public CommandQueue(CardCatalog cardCatalog, Player player, Player opponent, Tween gfx)
        {
	        GetCard = cardCatalog.Fetch;
            Player = player;
            Opponent = opponent;
            Gfx = gfx;
        }

        public void SubscribeTo(Messenger messenger)
        {
	        // We have to subscribe directly like this otherwise params don't unpack for some reason
	        messenger.Connect(nameof(Messenger.RevealCard), this, nameof(RevealCard));
	        messenger.Connect(nameof(Messenger.UpdateCard), this, nameof(OnCardUpdated));
	        messenger.Connect(nameof(Messenger.LoadDeck), this, nameof(OnDeckLoaded));
	        messenger.Connect(nameof(Messenger.Draw), this, nameof(OnDrawQueued));
	        messenger.Connect(nameof(Messenger.Deploy), this, nameof(OnDeployQueued));
	        messenger.Connect(nameof(Messenger.SetFaceDown), this, nameof(OnSetFaceDownQueued));
	        messenger.Connect(nameof(Messenger.Activate), this, nameof(OnActivationQueued));
	        messenger.Connect(nameof(Messenger.Trigger), this, nameof(OnTriggeredQueued));
	        messenger.Connect(nameof(Messenger.BattleUnit), this, nameof(OnUnitBattled));
	        messenger.Connect(nameof(Messenger.SendCardToZone), this, nameof(OnCardSentToZone));
	        messenger.Connect(nameof(Messenger.LoseLife), this, nameof(OnLifeLost));
        }

        public async Task Execute()
        {
            foreach (var command in Commands)
            {
                await command.Execute(Gfx);
                Gfx.RemoveAll();
            }
            
            Commands.Clear();
            
        }
        
        private void RevealCard(int id, SetCodes setCode, ZoneIds zoneId)
        {
            var card = GetCard(id, setCode);
            Commands.Enqueue(new RevealCard(Opponent, card, zoneId));
        }

        private void OnCardUpdated(int id, CardStates state, IEnumerable<int> attackTargets, IEnumerable<int> targets)
        {
            var card = GetCard(id);
            card.State = state;
            card.ValidTargets.Clear();
            foreach (var target in targets)
            {
	            card.ValidTargets.Add(GetCard(target));
            }

            card.ValidAttackTargets.Clear();

            foreach (var defender in attackTargets)
            {
	            card.ValidAttackTargets.Add(GetCard(defender));
            }
        }
        
        private void OnDeckLoaded(Dictionary<int, SetCodes> deck)
        {
        	foreach (var card in deck.Select(serial => GetCard(serial.Key, serial.Value)))
        	{
        		card.Player = Player;
        	}
        }

        private void OnDrawQueued(int id = 0, bool isOpponent = false)
        {
        	Commands.Enqueue(new Draw(GetPlayer(isOpponent), GetCard(id)));
        }

        private void OnDeployQueued(int id, SetCodes setCode, bool isOpponent)
        {
        	Commands.Enqueue(new Deploy(GetPlayer(isOpponent), GetCard(id)));
        }
        
        private void OnSetFaceDownQueued(int id, bool isOpponent)
        {
        	Commands.Enqueue(new SetFaceDown(GetPlayer(isOpponent), GetCard(id), isOpponent));
        }
        
        private void OnActivationQueued(int id, SetCodes setCode, int positionInLink, bool isOpponent)
        {
        	Commands.Enqueue(new Activate(GetCard(id), positionInLink));
        }
        
        private void OnTriggeredQueued(int id, int positionInLink)
        {
        	Commands.Enqueue(new Trigger(GetCard(id), positionInLink));
        }
        
        private void OnUnitBattled(int attackerId, int defenderId, bool isOpponent)
        {
        	var attacker = GetCard(attackerId);
        	var defender = GetCard(defenderId);
        	Commands.Enqueue(new Battle(attacker, defender, isOpponent));
        }

        private void OnCardSentToZone(int cardId, ZoneIds zoneId, bool isOpponent)
        {
        	var card = GetCard(cardId);
            Commands.Enqueue(new SendCardToZone(GetPlayer(isOpponent), card, zoneId));
        }

        private void OnLifeLost(int lifeLost, bool isOpponent)
        {
        	Commands.Enqueue(new LoseLife(GetPlayer(isOpponent), lifeLost));
        }

        private Player GetPlayer(bool isOpponent = false) => isOpponent ? Opponent : Player;
    }
}
