using Core;

namespace Player
{
    public class HumanPlayer : IPlayer
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
}