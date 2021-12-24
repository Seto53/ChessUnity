using Core;

namespace Player
{
    public interface IPlayer
    {
        public Move GetMove(ChessGame game);
    }
}