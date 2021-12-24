using System;
using System.Collections.Generic;
using Core;

namespace Player
{
    public class RandomPlayer : IPlayer
    {
        private readonly Random rnd = new();

        public Move GetMove(ChessGame game)
        {
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(game, true);
            return legalMoves[rnd.Next(0, legalMoves.Count)];
        }
    }
}