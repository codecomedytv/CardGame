using System;
using CardGame.Client.Library.Card;
using Godot;
using Array = Godot.Collections.Array;

namespace CardGameSharp.Client.Game
{
    public class History: RichTextLabel
    {
	    private enum Who
	    {
		    Global,
		    Player,
		    Opponent
	    };

	    private int Line = 0;

	    public void PlayerDraw(Card card)
	    {
		    BbcodeText += $"{Line} : Your drew {card}\n";
		    Line++;
	    }

	    public void OpponentDraw()
	    {
		    BbcodeText += $"{Line}: Enemy drew a card(s)\n";
		    Line++;
	    }

	    public void Deploy(int player, Card card)
	    {
		    var p = player == (int) Who.Player ? "You" : "Enemy";
		    BbcodeText += $"{Line} : {p} Deployed {card}\n";
		    Line++;
	    }

	    public void SetFaceDown(int player, Card card)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += $"{Line} : Your set {card}\n";
			    Line++;
		    }
		    else
		    {
			    BbcodeText += $"{Line} : Enemy set a card FaceDown\n";
			    Line++;
		    }
	    }

	    public void Activate(int player, Card card)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += $"{Line} : You activated {card}\n";
			    Line++;
		    }
		    else
		    {
			    BbcodeText += $"{Line} : Enemy Activated {card}";
			    Line++;
		    }
	    }
	    
	    public void Target(int player, Array targets)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += $"{Line} : \tTargeting%s\n{targets}";
			    Line++;
		    }
		    else
		    {
			    BbcodeText += $"{Line} : \tTargeting: {targets}\n";
			    Line++;
		    }
	    }

	    public void Bounce(int player, Card card)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += $"{Line} : {card} was returned to your hand\n";
			    Line++;
		    }
		    else
		    {
			    BbcodeText += $"{Line} : {card} was returned to Enemy's Hand\n";
			    Line++;
		    }
	    }

	    public void Attack(int player, Card attacker, Card defender)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += $"{Line} : Your {attacker} attacked:\n\t   Enemy's {defender}\n";
			    Line++;
		    }
		    else
		    {
			    BbcodeText += $"{Line} : Enemy's {attacker} Attacked:\n\t Your {defender}\n";
			    Line++;
		    }
	    }
	    
	    public void DirectAttack(int player, Card attacker)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += $"{Line} : Your {attacker} attacked directly\n";
			    Line++;
		    }
		    else
		    {
			    BbcodeText += $"{Line} : Enemy's {attacker} attacked directly\n";
			    Line++;
		    }
	    }

	    public void DestroyUnit(int player, Card unit)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += $"{Line} : Your {unit} was destroyed\n";
			    Line++;
		    }
		    else
		    {
			    BbcodeText += $"{Line} : Enemy's {unit} was destroyed\n";
			    Line++;
		    }
		    
	    }

	    public void LoseLife(int player, int amount)
	    {
		    if (player == (int) Who.Player)
		    {
			    BbcodeText += $"{Line} : You took {amount} damage\n";
			    Line++;
		    }
		    else
		    {
			    BbcodeText += $"{Line} : Enemy Took {amount} damage\n";
			    Line++;
		    }

	    }

	    public void BeginTurn()
	    {
		    if (Line > 2)
		    {
			    // Line 0 & 1 are taken by starting hands;
			    BbcodeText += $"{Line} : Enemy's turn had ended\n";
			    Line++;
		    }

		    BbcodeText += $"{Line} : Your turn has started\n";
		    Line++;
	    }

	    public void EndTurn()
	    {
		    BbcodeText += $"{Line} : Your turn has ended\n";
		    Line++;
		    BbcodeText += $"{Line} : Enemy's turn has started\n";
		    Line++;
	    }
    }
}

