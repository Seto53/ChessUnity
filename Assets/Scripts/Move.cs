using System;
using System.Collections.Generic;

public class Move
{
    private readonly int fromX;
    private readonly int fromY;
    private readonly int toX;
    private readonly int toY;
    private float score;

    public Move(int fromX, int fromY, int toX, int toY)
    {
        this.fromX = fromX;
        this.fromY = fromY;
        this.toX = toX;
        this.toY = toY;
    }

    public static Move GetMaxMove(List<Move> moves)
    {
        Random rnd = new Random();
        int range = rnd.Next(0, moves.Count);
        Tuple<float, int> max = new(moves[range].score, range);
        for (int i = 0; i < moves.Count; i++)
        {
            if (moves[i].score > max.Item1)
            {
                max = new Tuple<float, int>(moves[i].score, i);
            }
        }

        return moves[max.Item2];
    }

    public static Move GetMinMove(List<Move> moves)
    {
        Random rnd = new Random();
        int range = rnd.Next(0, moves.Count);
        Tuple<float, int> min = new(moves[range].score, range);
        for (int i = 0; i < moves.Count; i++)
        {
            if (moves[i].score < min.Item1)
            {
                min = new Tuple<float, int>(moves[i].score, i);
            }
        }

        return moves[min.Item2];
    }


    public void SetScore(float score2)
    {
        this.score = score2;
    }

    public float GetScore()
    {
        return score;
    }

    public Tuple<int, int> GetFrom()
    {
        return new Tuple<int, int>(fromX, fromY);
    }

    public Tuple<int, int> GetTo()
    {
        return new Tuple<int, int>(toX, toY);
    }
}