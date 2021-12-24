using System;
using System.Collections.Generic;
using System.Threading;
using Core;

namespace Player
{
    public class EnginePlayer : IPlayer
    {
        private readonly int engineDepth;

        public EnginePlayer(int engineDepth)
        {
            this.engineDepth = engineDepth;
        }

        public Move GetMove(ChessGame game)
        {
            return GetMoveAtDepth(engineDepth, game);
        }

        private Move GetMoveAtDepth(int depth, ChessGame game)
        {
            List<Thread> myThreads = new List<Thread>();
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(game, true);
            List<Move> moves = new List<Move>();

            foreach (Move m in legalMoves)
            {
                ChessGame clone = game.DeepClone();
                clone.Play(m.GetTo().Item1, m.GetTo().Item2, m.GetFrom().Item1, m.GetFrom().Item2, false);
                Thread thread = new Thread(() =>
                {
                    m.SetScore(game.GetCurrentPlayer() == "Black"
                        ? GetScoreAtDepth(depth - 1, false, clone, float.PositiveInfinity, float.NegativeInfinity)
                        : GetScoreAtDepth(depth - 1, true, clone, float.NegativeInfinity, float.PositiveInfinity));

                    lock (moves)
                    {
                        moves.Add(m);
                    }
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

            return game.GetCurrentPlayer() == "Black" ? Move.GetMinMove(moves) : Move.GetMaxMove(moves);
        }

        private float GetScoreAtDepth(int depth, bool player, ChessGame game, float alpha, float beta)
        {
            if (depth == 0 || game.IsGameOver())
            {
                return game.GetScore();
            }

            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(game, true);

            if (player)
            {
                float maxEval = float.NegativeInfinity;
                foreach (Move m in legalMoves)
                {
                    ChessGame clone = game.DeepClone();
                    clone.Play(m.GetTo().Item1, m.GetTo().Item2, m.GetFrom().Item1, m.GetFrom().Item2, false);
                    float depthScore = GetScoreAtDepth(depth - 1, false, clone, alpha, beta);
                    maxEval = Math.Max(maxEval, depthScore);
                    alpha = Math.Max(alpha, depthScore);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return maxEval;
            }
            else
            {
                float minEval = float.PositiveInfinity;
                foreach (Move m in legalMoves)
                {
                    ChessGame clone = game.DeepClone();
                    clone.Play(m.GetTo().Item1, m.GetTo().Item2, m.GetFrom().Item1, m.GetFrom().Item2, false);
                    float depthScore = GetScoreAtDepth(depth - 1, true, clone, beta, alpha);
                    minEval = Math.Min(minEval, depthScore);
                    beta = Math.Min(beta, depthScore);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return minEval;
            }
        }
    }
}