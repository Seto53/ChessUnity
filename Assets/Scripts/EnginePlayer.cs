using System;
using System.Collections.Generic;
using System.Threading;

public class EnginePlayer : Player
{
    private readonly int engineDepth;
    private readonly bool multithreaded;

    public EnginePlayer(int engineDepth, bool setMultithreaded)
    {
        this.engineDepth = engineDepth;
        this.multithreaded = setMultithreaded;
    }

    public Move GetMove(ChessGame game)
    {
        return multithreaded ? GetMoveAtDepthMultihread(engineDepth, game) : GetMoveAtDepth(engineDepth, game);
    }

    private Move GetMoveAtDepthMultihread(int depth, ChessGame game)
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
                m.SetScore(clone.GetScore() +
                           GetScoreAtDepth(depth - 1, game.GetCurrentPlayer(), clone));
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

    private Move GetMoveAtDepth(int depth, ChessGame game)
    {
        List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(game, true);
        List<Move> moves = new List<Move>();

        foreach (Move m in legalMoves)
        {
            ChessGame clone = game.DeepClone();
            clone.Play(m.GetTo().Item1, m.GetTo().Item2, m.GetFrom().Item1, m.GetFrom().Item2, false);

            m.SetScore(clone.GetScore() +
                       GetScoreAtDepth(depth - 1, game.GetCurrentPlayer(), clone));

            moves.Add(m);
        }

        return game.GetCurrentPlayer() == "Black" ? Move.GetMinMove(moves) : Move.GetMaxMove(moves);
    }


    private float GetScoreAtDepth(int depth, string player, ChessGame game)
    {
        if (depth == 0)
        {
            return 0;
        }

        List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(game, true);
        if (legalMoves.Count == 0)
        {
            if (game.IsKingInDanger())
            {
                return player == "Black" ? float.PositiveInfinity : float.NegativeInfinity;
            }

            return 0;
        }

        float actualScore = 0;
        foreach (Move m in legalMoves)
        {
            ChessGame clone = game.DeepClone();
            clone.Play(m.GetTo().Item1, m.GetTo().Item2, m.GetFrom().Item1, m.GetFrom().Item2, false);
            float depthScore = clone.GetScore() + GetScoreAtDepth(depth - 1, player, clone);
            actualScore = player == "Black"
                ? Math.Max(depthScore, actualScore)
                : Math.Min(depthScore, actualScore);
        }

        return actualScore;
    }
}