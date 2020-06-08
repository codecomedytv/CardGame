using Godot;

namespace CardGame.Client.Match
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

    }
}

