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

	    public void AddLine(string gameEvent)
	    {
		    BbcodeText += $"{Line} : {gameEvent}\n";
		    Line++;
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
    }
}

