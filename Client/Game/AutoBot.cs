using System;
using CardGame.Client.Library.Card;
using CardGame.Server;
using Godot;

namespace CardGameSharp.Client.Game
{
    public class AutoBot: GameInput
    {
	    public bool Attacked = false;
	    public bool Deployed = false;

	    public void Command()
	    {
		    if (GameOver() || !IsOurTurn())
		    {
			    return;
		    }

		    GD.Randomize();

		    if (!Deployed)
		    {
			    Deploy();
			    Deployed = true;
		    }

		    else if (!Attacked)
		    {
			    Attack();
			    Attacked = true;
		    }

		    else
		    {
			    End();
		    }
	    }

	    public bool GameOver()
	    {
		    return Player.Lost || Player.Won;
	    }

	    public bool IsOurTurn()
	    {
		    return Player.IsTurnPlayer;
	    }

	    public void Attack()
	    {
		    if (Player.Field.Count > 0 && Opponent.Field.Count > 0)
		    {
			    var attacker = Player.Field[(int) (GD.Randi() % Player.Field.Count)];
			    var defender = Opponent.Field[(int) (GD.Randi() % Opponent.Field.Count)];
			    if (!attacker.IsReady)
			    {
				    CallDeferred(nameof(CommandRequested));
				    return;
			    }
			    OnAttack(attacker, defender.Id);
			    return;
		    }
		    else
		    {
			    End();
		    }
	    }

	    public void Deploy()
	    {
		    if (Player.Hand.Count > 0 && Player.Field.Count < 7)
		    {
			    var card = Player.Hand[(int) GD.Randi() % Player.Hand.Count];
			    if (card.CardType == CardTypes.Support)
			    {
				    GD.PushWarning("Attempting To Deploy Support Card: " + card.Title);
				    End();
				    return;
			    }
			    else
			    {
				    OnDeploy(card);
				    return;
			    }
		    }
		    else
		    {
			    End();
		    }
	    }

	    public void End()
	    {
		    Deployed = false;
		    Attacked = false;
		    EndTurn();
	    }
	    
	    

    }
}
