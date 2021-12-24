using System;
using System.Collections.Generic;
using System.Threading;

namespace Core
{
    public static class MoveGenerator
    {
        public static List<Move> GenerateLegalMoves(ChessGame game, bool check)
        {
            List<Move> moves = new List<Move>();
            foreach (var piece in game.GetPlayerPiecesPos())
            {
                List<Tuple<int, int>> legalMoves = game.GetLegalMoves(piece.Item1, piece.Item2, check);
                foreach (var t in legalMoves)
                {
                    moves.Add(new Move(piece.Item1, piece.Item2, t.Item1, t.Item2));
                }
            }

            return moves;
        }

        public static int GenerateMovesAtDepth(ChessGame game, int depth)
        {
            List<Thread> myThreads = new List<Thread>();
            List<Move> legalMoves = GenerateLegalMoves(game, true);
            int movesCount = 0;

            foreach (Move m in legalMoves)
            {
                ChessGame clone = game.DeepClone();
                clone.Play(m.GetTo().Item1, m.GetTo().Item2, m.GetFrom().Item1, m.GetFrom().Item2, false);
                Thread thread = new Thread(() =>
                {
                    int count = Search(clone, depth - 1);
                    movesCount += count;
                });
                myThreads.Add(thread);
            }

            foreach (Thread curThread in myThreads)
            {
                curThread.Start();
            }

            foreach (Thread curThread in myThreads)
            {
                curThread.Join();
            }

            return movesCount;
        }

        private static int Search(ChessGame game, int depth)
        {
            if (depth == 0)
            {
                return 0;
            }

            List<Move> legalMoves = GenerateLegalMoves(game, true);
            int movesCount = 0;
            foreach (Move m in legalMoves)
            {
                ChessGame clone = game.DeepClone();
                clone.Play(m.GetTo().Item1, m.GetTo().Item2, m.GetFrom().Item1, m.GetFrom().Item2, false);
                movesCount += Search(clone, depth - 1);
            }

            return movesCount;
        }
    }
}