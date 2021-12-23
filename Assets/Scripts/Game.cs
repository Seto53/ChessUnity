using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject chessPiece;
    public ChessGame chessGame;
    private GameObject[,] chessBoard = new GameObject[8, 8];
    private readonly Stack<ChessGame> gamesHistory = new();

    private float blackPoints;
    private float whitePoints;
    private int gameMode;
    public Player WhitePlayer;
    public Player BlackPlayer;
    private const int EngineDepth = 2;


    private void Start()
    {
        gameMode = 0;

        chessGame = new ChessGame();
        gamesHistory.Push(chessGame.DeepClone());
        DestroyAll();
        chessBoard = new GameObject[8, 8];
        UpdateChessBoard();
        switch (gameMode)
        {
            case 0:
                WhitePlayer = new HumanPlayer();
                BlackPlayer = new EnginePlayer(EngineDepth, true);
                break;
            case 1:
                WhitePlayer = new HumanPlayer();
                BlackPlayer = new EnginePlayer(EngineDepth, false);
                break;
            case 2:
                WhitePlayer = new HumanPlayer();
                BlackPlayer = new HumanPlayer();
                break;
            case 3:
                WhitePlayer = new EnginePlayer(EngineDepth, false);
                BlackPlayer = new EnginePlayer(EngineDepth, false);
                break;
            case 4:
                WhitePlayer = new RandomPlayer();
                BlackPlayer = new RandomPlayer();
                break;
            case 5:
                WhitePlayer = new HumanPlayer();
                BlackPlayer = new RandomPlayer();
                break;
        }
    }

    private void CheckForMove()
    {
        if (chessGame.GetCurrentPlayer() == "Black")
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Move move = BlackPlayer.GetMove(chessGame);
            watch.Stop();
            Debug.Log($"Execution Time: {watch.ElapsedMilliseconds} ms");
            if (move == null) return;
            Play(move.GetTo().Item1, move.GetTo().Item2, chessBoard[move.GetFrom().Item1, move.GetFrom().Item2]);
            if (BlackPlayer is HumanPlayer player)
            {
                player.SetNull();
            }
        }
        else
        {
            Move move = WhitePlayer.GetMove(chessGame);
            if (move == null) return;
            Play(move.GetTo().Item1, move.GetTo().Item2, chessBoard[move.GetFrom().Item1, move.GetFrom().Item2]);
            if (WhitePlayer is HumanPlayer player)
            {
                player.SetNull();
            }
        }
    }

    public void Update()
    {
        if (!IsGameOver())
        {
            CheckForMove();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("Game");
            Debug.Log("Blackpoints:" + blackPoints);
            Debug.Log("Whitepoints:" + whitePoints);
            Start();
        }
    }

    private void UpdateChessBoard()
    {
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                if (chessGame.GetBoard()[i, j] == GameObjectToPiece(chessBoard[i, j])) continue;
                DestroyAt(i, j);
                CreateChessObject(chessGame.GetBoard()[i, j], i, j);
            }
        }
    }


    private void CreateChessObject(ChessPiece piece, int x, int y)
    {
        string toName = PieceToName(piece);
        if (toName == "none") return;
        GameObject obj = Instantiate(chessPiece, new Vector3(0, 0, -1), Quaternion.identity);
        ChessObject cm = obj.GetComponent<ChessObject>();
        cm.name = toName;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();
        chessBoard[x, y] = obj;
    }

    private void Play(int x, int y, GameObject obj)
    {
        ChessObject cm = obj.GetComponent<ChessObject>();
        gamesHistory.Push(chessGame.DeepClone());
        chessGame.Play(x, y, cm.GetXBoard(), cm.GetYBoard(), true);
        cm.DestroyMovePlates();
        UpdateChessBoard();
        if (IsGameOver())
        {
            SetWinner(chessGame.GetState());
        }

        GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>().text = chessGame.GetScore() + "";
    }

    public void UndoMove()
    {
        if (gamesHistory.Count <= 1) return;
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        foreach (var t in movePlates)
        {
            Destroy(t);
        }

        chessGame = gamesHistory.Pop();
        GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>().text = chessGame.GetScore() + "";
        UpdateChessBoard();
    }

    private void DestroyAll()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                DestroyAt(i, j);
            }
        }
    }

    private void DestroyAt(int x, int y)
    {
        if (chessBoard[x, y] != null)
        {
            Destroy(chessBoard[x, y]);
        }
    }

    //Setters/Getters
    private void SetWinner(GameState playerWinner)
    {
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>().enabled = false;
        string message = String.Empty;
        switch (playerWinner)
        {
            case GameState.DrawNoMaterial:
                blackPoints += 0.5f;
                whitePoints += 0.5f;
                message = "Draw by Insufficient Material!";
                break;
            case GameState.DrawStalemate:
                blackPoints += 0.5f;
                whitePoints += 0.5f;
                message = "Draw by Stalemate!";
                break;
            case GameState.BlackWinCheckmate:
                blackPoints += 1f;
                message = "Black wins by Checkmate!";
                break;
            case GameState.WhiteWinCheckmate:
                whitePoints += 1f;
                message = "White wins by Checkmate!";
                break;
        }

        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = message;
        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }

    public string GetCurrentPlayer()
    {
        return chessGame.GetCurrentPlayer();
    }

    public bool IsGameOver()
    {
        return chessGame.IsGameOver();
    }

    public ChessPiece GetPieceAt(int x, int y)
    {
        return chessGame.GetPieceAt(x, y);
    }

    //Static
    private static string PieceToName(ChessPiece piece)
    {
        return piece switch
        {
            ChessPiece.BlackQueen => "b_queen",
            ChessPiece.WhiteQueen => "w_queen",
            ChessPiece.BlackKnight => "b_knight",
            ChessPiece.WhiteKnight => "w_knight",
            ChessPiece.BlackBishop => "b_bishop",
            ChessPiece.WhiteBishop => "w_bishop",
            ChessPiece.BlackKing => "b_king",
            ChessPiece.WhiteKing => "w_king",
            ChessPiece.BlackRook => "b_rook",
            ChessPiece.WhiteRook => "w_rook",
            ChessPiece.BlackPawn => "b_pawn",
            ChessPiece.WhitePawn => "w_pawn",
            _ => "none"
        };
    }

    private static ChessPiece GameObjectToPiece(GameObject obj)
    {
        if (obj == null)
        {
            return ChessPiece.None;
        }

        ChessObject cm = obj.GetComponent<ChessObject>();
        return cm.name switch
        {
            "b_queen" => ChessPiece.BlackQueen,
            "w_queen" => ChessPiece.WhiteQueen,
            "b_knight" => ChessPiece.BlackKnight,
            "w_knight" => ChessPiece.WhiteKnight,
            "b_bishop" => ChessPiece.BlackBishop,
            "w_bishop" => ChessPiece.WhiteBishop,
            "b_king" => ChessPiece.BlackKing,
            "w_king" => ChessPiece.WhiteKing,
            "b_rook" => ChessPiece.BlackRook,
            "w_rook" => ChessPiece.WhiteRook,
            "b_pawn" => ChessPiece.BlackPawn,
            "w_pawn" => ChessPiece.WhitePawn,
            _ => ChessPiece.None
        };
    }
}