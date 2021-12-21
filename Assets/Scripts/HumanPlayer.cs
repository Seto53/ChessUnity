public class HumanPlayer : Player
{
    private Move move;

    public Move GetMove(ChessGame game)
    {
        return move;
    }

    public void SetMove(Move m)
    {
        move = m;
    }

    public void SetNull()
    {
        move = null;
    }
}