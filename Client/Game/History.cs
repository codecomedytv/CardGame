using System;
using CardGame.Client.Library.Card;
using Godot;
using Array = Godot.Collections.Array;

namespace CardGameSharp.Client.Game
{
    public class History: RichTextLabel
    {
	    public enum Who
	    {
		    Global,
		    Player,
		    Opponent
	    };

	    private int Line = 0;

	    public void PlayerDraw(Card card)
	    {
		    BbcodeText += String.Format("{0} : Your drew {1}\n", Line, card.ToString());
		    Line++;
	    }

	    public void OpponentDraw(int count)
	    {
		    BbcodeText += String.Format("{0}: Enemy drew a card(s)\n", Line, count);
		    Line++;
	    }

	    public void Deploy(int player, Card card)
	    {
		    var p = player == (int) Who.Player ? "You" : "Enemy";
		    BbcodeText += String.Format("{0} : {1} Deployed {2}\n", Line, p, card);
		    Line++;
	    }

	    public void SetFaceDown(int player, Card card)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += String.Format("{0} : Your set {1}\n", Line, card);
			    Line++;
		    }
		    else
		    {
			    BbcodeText += String.Format("{0} : Enemy set a card facedown\n", Line);
			    Line++;
		    }
	    }

	    public void Target(int player, Array targets)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += String.Format("{0} : \tTargeting%s\n{1}", Line, targets);
			    Line++;
		    }
		    else
		    {
			    BbcodeText += String.Format("{0} : \tTargeting: %s\n", Line, targets);
			    Line++;
		    }
	    }

	    public void Bounce(int player, Card card)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += String.Format("{0} : {1} was returned to your hand\n", Line, card);
			    Line++;
		    }
		    else
		    {
			    BbcodeText += String.Format("{0} : {1} was returned to Enemy's Hand\n", Line, card);
			    Line++;
		    }
	    }

	    public void Attack(int player, Card attacker, Card defender)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += String.Format("{0} : Your {1} attacked:\n\t   Enemy's {2}\n", Line, attacker, defender);
			    Line++;
		    }
		    else
		    {
			    BbcodeText += String.Format("{0} : Enemy's {1} Attacked:\n\t Your {2}\n", Line, attacker, defender);
			    Line++;
		    }
	    }

	    public void Activate(int player, Card card)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += String.Format("{0} : You activated {1}\n", Line, card);
			    Line++;
		    }
		    else
		    {
			    BbcodeText += String.Format("{0} : Enemy Activated {1}", Line, card);
			    Line++;
		    }
	    }

	    public void DirectAttack(int player, Card attacker)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += String.Format("{0} : Your {1} attacked directly\n", Line, attacker);
			    Line++;
		    }
		    else
		    {
			    BbcodeText += String.Format("{0} : Enemy's {1} attacked directly\n", Line, attacker);
			    Line++;
		    }
	    }

	    public void DestroyUnit(int player, Card unit)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += String.Format("{0} : Your {1} was destroyed\n", Line, unit);
			    Line++;
		    }
		    else
		    {
			    BbcodeText += String.Format("{0} : Enemy's {1} was destroyed\n", Line, unit);
			    Line++;
		    }
		    
	    }

	    public void LoseLife(int player, int amount)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += String.Format("{0} : You took {1} damage\n", Line, amount);
			    Line++;
		    }
		    else
		    {
			    BbcodeText += String.Format("{0} : Enemy Took {1} damage\n", Line, amount);
			    Line++;
		    }

	    }

	    public void BeginTurn()
	    {
		    if (Line > 2)
		    {
			    // Line 0 & 1 are taken by starting hands;
			    BbcodeText += String.Format("{0} : Enemy's turn had ended\n", Line);
			    Line++;
		    }

		    BbcodeText += String.Format("{0} : Your turn has started\n", Line);
		    Line++;
	    }

	    public void EndTurn()
	    {
		    BbcodeText += String.Format("{0} : Your turn has ended\n", Line);
		    Line++;
		    BbcodeText += String.Format("{0} : Enemy's turn has started\n", Line);
		    Line++;
	    }
    }
}

