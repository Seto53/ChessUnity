using System;
using System.Collections.Generic;

[Serializable]
public class ChessGame
{
    private ChessPiece[,] board = new ChessPiece[8, 8];
    private string currentPlayer;
    private bool gameOver;
    private GameState state;

    private bool whiteCastleKing;
    private bool whiteCastleQueen;
    private bool blackCastleKing;
    private bool blackCastleQueen;
    private int enPassant;
    private bool enPassantMoved;

    private float score;

    public ChessGame()
    {
        gameOver = false;
        currentPlayer = "White";
        state = GameState.Playing;
        score = 0f;
        whiteCastleKing = true;
        whiteCastleQueen = true;
        blackCastleKing = true;
        blackCastleQueen = true;
        enPassant = -1;
        enPassantMoved = false;

        board[0, 0] = ChessPiece.WhiteRook;
        board[1, 0] = ChessPiece.WhiteKnight;
        board[2, 0] = ChessPiece.WhiteBishop;
        board[3, 0] = ChessPiece.WhiteQueen;
        board[4, 0] = ChessPiece.WhiteKing;
        board[5, 0] = ChessPiece.WhiteBishop;
        board[6, 0] = ChessPiece.WhiteKnight;
        board[7, 0] = ChessPiece.WhiteRook;
        for (var i = 0; i <= 7; i++)
        {
            board[i, 1] = ChessPiece.WhitePawn;
        }

        board[0, 7] = ChessPiece.BlackRook;
        board[1, 7] = ChessPiece.BlackKnight;
        board[2, 7] = ChessPiece.BlackBishop;
        board[3, 7] = ChessPiece.BlackQueen;
        board[4, 7] = ChessPiece.BlackKing;
        board[5, 7] = ChessPiece.BlackBishop;
        board[6, 7] = ChessPiece.BlackKnight;
        board[7, 7] = ChessPiece.BlackRook;
        for (var i = 0; i <= 7; i++)
        {
            board[i, 6] = ChessPiece.BlackPawn;
        }
    }

    public void Play(int toX, int toY, int fromX, int fromY, bool check)
    {
        ChessPiece piece = GetPieceAt(fromX, fromY);
        switch (piece)
        {
            case ChessPiece.BlackPawn:
                if (toY == 0)
                {
                    UpdateCastling(toX, toY);
                    board[toX, toY] = ChessPiece.BlackQueen;
                    score -= 7;
                }
                else if (fromY - toY == 2)
                {
                    enPassant = toX;
                    enPassantMoved = true;
                    board[toX, toY] = piece;
                }
                else
                    switch (board[toX, toY])
                    {
                        case ChessPiece.None when fromX - 1 == toX && toX == enPassant:
                            enPassantMoved = false;
                            enPassant = -1;
                            score -= 1;
                            board[toX, toY] = piece;
                            board[fromX - 1, fromY] = ChessPiece.None;
                            break;
                        case ChessPiece.None when fromX + 1 == toX && toX == enPassant:
                            enPassantMoved = false;
                            enPassant = -1;
                            score -= 1;
                            board[toX, toY] = piece;
                            board[fromX + 1, fromY] = ChessPiece.None;
                            break;
                        default:
                            UpdateScore(toX, toY);
                            board[toX, toY] = piece;
                            break;
                    }

                break;
            case ChessPiece.WhitePawn:
                if (toY == 7)
                {
                    UpdateCastling(toX, toY);
                    board[toX, toY] = ChessPiece.WhiteQueen;
                    score += 7;
                }
                else if (toY - fromY == 2)
                {
                    enPassant = toX;
                    enPassantMoved = true;
                    board[toX, toY] = piece;
                }
                else
                    switch (board[toX, toY])
                    {
                        case ChessPiece.None when fromX - 1 == toX && toX == enPassant:
                            enPassantMoved = false;
                            enPassant = -1;
                            score += 1;
                            board[toX, toY] = piece;
                            board[fromX - 1, fromY] = ChessPiece.None;
                            break;
                        case ChessPiece.None when fromX + 1 == toX && toX == enPassant:
                            enPassantMoved = false;
                            enPassant = -1;
                            score += 1;
                            board[toX, toY] = piece;
                            board[fromX + 1, fromY] = ChessPiece.None;
                            break;
                        default:
                            UpdateScore(toX, toY);
                            board[toX, toY] = piece;
                            break;
                    }

                break;
            case ChessPiece.BlackKing when fromY == 7:
                blackCastleKing = false;
                blackCastleQueen = false;
                if (!IsKingInDanger())
                {
                    if (fromX == 4)
                    {
                        switch (toX)
                        {
                            case 2:
                                board[0, 7] = ChessPiece.None;
                                board[3, 7] = ChessPiece.BlackRook;
                                break;
                            case 6:
                                board[7, 7] = ChessPiece.None;
                                board[5, 7] = ChessPiece.BlackRook;
                                break;
                        }
                    }
                    else
                    {
                        UpdateScore(toX, toY);
                    }

                    UpdateCastling(toX, toY);
                    board[toX, toY] = piece;
                }
                else
                {
                    UpdateScore(toX, toY);
                }

                break;
            case ChessPiece.WhiteKing when fromY == 0:
                whiteCastleKing = false;
                whiteCastleQueen = false;
                if (!IsKingInDanger())
                {
                    if (fromX == 4)
                    {
                        switch (toX)
                        {
                            case 2:
                                board[0, 0] = ChessPiece.None;
                                board[3, 0] = ChessPiece.WhiteRook;
                                break;
                            case 6:
                                board[7, 0] = ChessPiece.None;
                                board[5, 0] = ChessPiece.WhiteRook;
                                break;
                        }
                    }
                    else
                    {
                        UpdateScore(toX, toY);
                    }

                    UpdateCastling(toX, toY);
                    board[toX, toY] = piece;
                }
                else
                {
                    UpdateScore(toX, toY);
                }


                break;
            case ChessPiece.BlackRook when fromY == 7:
                switch (fromX)
                {
                    case 0:
                        blackCastleQueen = false;
                        break;
                    case 7:
                        blackCastleKing = false;
                        break;
                }

                UpdateScore(toX, toY);
                UpdateCastling(toX, toY);
                board[toX, toY] = piece;
                break;
            case ChessPiece.WhiteRook when fromY == 0:
                switch (fromX)
                {
                    case 0:
                        whiteCastleQueen = false;
                        break;
                    case 7:
                        whiteCastleKing = false;
                        break;
                }

                UpdateScore(toX, toY);
                UpdateCastling(toX, toY);
                board[toX, toY] = piece;
                break;
            default:
                UpdateScore(toX, toY);
                UpdateCastling(toX, toY);
                board[toX, toY] = piece;
                break;
        }

        board[fromX, fromY] = ChessPiece.None;
        NextTurn();

        if (enPassantMoved)
        {
            enPassantMoved = false;
        }
        else
        {
            enPassant = -1;
        }

        List<Tuple<int, int>> playerPiecesPos = GetPlayerPiecesPos();
        foreach (var t in playerPiecesPos)
        {
            if (GetLegalMoves(t.Item1, t.Item2, check).Count != 0)
            {
                if (IsDrawMaterial())
                {
                    SetGameState(GameState.DrawNoMaterial);
                }

                return;
            }
        }

        ChessGame clone = this.DeepClone();
        clone.NextTurn();
        if (clone.IsKingInDanger())
        {
            SetGameState(currentPlayer == "Black" ? GameState.WhiteWinCheckmate : GameState.BlackWinCheckmate);
        }
        else
        {
            SetGameState(GameState.DrawStalemate);
        }
    }

    private void UpdateCastling(int toX, int toY)
    {
        switch (toX)
        {
            case 0 when toY == 7 && board[toX, toY] == ChessPiece.BlackRook:
                blackCastleQueen = false;
                break;
            case 7 when toY == 7 && board[toX, toY] == ChessPiece.BlackRook:
                blackCastleKing = false;
                break;
            case 0 when toY == 0 && board[toX, toY] == ChessPiece.WhiteRook:
                whiteCastleQueen = false;
                break;
            case 7 when toY == 0 && board[toX, toY] == ChessPiece.WhiteRook:
                whiteCastleKing = false;
                break;
        }
    }

    private void UpdateScore(int toX, int toY)
    {
        switch (board[toX, toY])
        {
            case ChessPiece.None:
                break;
            case ChessPiece.BlackRook:
                score += 5f;
                break;
            case ChessPiece.BlackBishop:
                score += 3f;
                break;
            case ChessPiece.BlackQueen:
                score += 9f;
                break;
            case ChessPiece.BlackKnight:
                score += 3f;
                break;
            case ChessPiece.BlackPawn:
                score += 1f;
                break;
            case ChessPiece.WhiteRook:
                score -= 5f;
                break;
            case ChessPiece.WhiteBishop:
                score -= 3f;
                break;
            case ChessPiece.WhiteQueen:
                score -= 9f;
                break;
            case ChessPiece.WhiteKnight:
                score -= 3f;
                break;
            case ChessPiece.WhitePawn:
                score -= 1f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private bool IsDrawMaterial()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board[i, j] != ChessPiece.BlackKing && board[i, j] != ChessPiece.WhiteKing &&
                    board[i, j] != ChessPiece.None)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool IsKingInDanger()
    {
        List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(this, false);
        foreach (Move m in legalMoves)
        {
            if (GetCurrentPlayer() == "Black" &&
                GetPieceAt(m.GetTo().Item1, m.GetTo().Item2) == ChessPiece.WhiteKing ||
                GetCurrentPlayer() == "White" &&
                GetPieceAt(m.GetTo().Item1, m.GetTo().Item2) == ChessPiece.BlackKing)
            {
                return true;
            }
        }

        return false;
    }

    public List<Tuple<int, int>> GetLegalMoves(int selectedX, int selectedY, bool check)
    {
        ChessPiece selectedPiece = board[selectedX, selectedY];

        List<Tuple<int, int>> legalMoves = new();

        switch (selectedPiece)
        {
            case ChessPiece.BlackQueen:
            case ChessPiece.WhiteQueen:
                legalMoves.AddRange(LineMove(1, 0, selectedX, selectedY));
                legalMoves.AddRange(LineMove(0, 1, selectedX, selectedY));
                legalMoves.AddRange(LineMove(1, 1, selectedX, selectedY));
                legalMoves.AddRange(LineMove(-1, 0, selectedX, selectedY));
                legalMoves.AddRange(LineMove(0, -1, selectedX, selectedY));
                legalMoves.AddRange(LineMove(-1, -1, selectedX, selectedY));
                legalMoves.AddRange(LineMove(-1, 1, selectedX, selectedY));
                legalMoves.AddRange(LineMove(1, -1, selectedX, selectedY));
                break;
            case ChessPiece.BlackKnight:
            case ChessPiece.WhiteKnight:
                legalMoves.AddRange(LMove(selectedX, selectedY));
                break;
            case ChessPiece.BlackBishop:
            case ChessPiece.WhiteBishop:
                legalMoves.AddRange(LineMove(1, 1, selectedX, selectedY));
                legalMoves.AddRange(LineMove(1, -1, selectedX, selectedY));
                legalMoves.AddRange(LineMove(-1, 1, selectedX, selectedY));
                legalMoves.AddRange(LineMove(-1, -1, selectedX, selectedY));
                break;
            case ChessPiece.BlackKing:
                if (blackCastleKing)
                {
                    if (board[5, 7] == ChessPiece.None && board[6, 7] == ChessPiece.None)
                    {
                        legalMoves.Add(new Tuple<int, int>(6, 7));
                    }
                }

                if (blackCastleQueen)
                {
                    if (board[1, 7] == ChessPiece.None && board[2, 7] == ChessPiece.None &&
                        board[3, 7] == ChessPiece.None)
                    {
                        legalMoves.Add(new Tuple<int, int>(2, 7));
                    }
                }

                legalMoves.AddRange(SurroundMove(selectedX, selectedY));
                break;
            case ChessPiece.WhiteKing:
                if (whiteCastleKing)
                {
                    if (board[5, 0] == ChessPiece.None && board[6, 0] == ChessPiece.None)
                    {
                        legalMoves.Add(new Tuple<int, int>(6, 0));
                    }
                }

                if (whiteCastleQueen)
                {
                    if (board[1, 0] == ChessPiece.None && board[2, 0] == ChessPiece.None &&
                        board[3, 0] == ChessPiece.None)
                    {
                        legalMoves.Add(new Tuple<int, int>(2, 0));
                    }
                }

                legalMoves.AddRange(SurroundMove(selectedX, selectedY));
                break;
            case ChessPiece.BlackRook:
            case ChessPiece.WhiteRook:
                legalMoves.AddRange(LineMove(1, 0, selectedX, selectedY));
                legalMoves.AddRange(LineMove(0, 1, selectedX, selectedY));
                legalMoves.AddRange(LineMove(-1, 0, selectedX, selectedY));
                legalMoves.AddRange(LineMove(0, -1, selectedX, selectedY));
                break;
            case ChessPiece.BlackPawn:
                switch (selectedY)
                {
                    case 6 when GetPieceAt(selectedX, selectedY - 1) == ChessPiece.None &&
                                GetPieceAt(selectedX, selectedY - 2) == ChessPiece.None:
                        legalMoves.Add(new Tuple<int, int>(selectedX, selectedY - 2));
                        break;
                    case 3:
                    {
                        if (IsPositionOnBoard(selectedX + 1, selectedY) &&
                            IsPositionOnBoard(selectedX + 1, selectedY - 1) && enPassant == selectedX + 1)
                        {
                            if (GetPieceAt(selectedX + 1, selectedY) != ChessPiece.None &&
                                GetPlayerColor(GetPieceAt(selectedX + 1, selectedY)) != currentPlayer &&
                                GetPieceAt(selectedX + 1, selectedY - 1) == ChessPiece.None)
                            {
                                legalMoves.Add(new Tuple<int, int>(selectedX + 1, selectedY - 1));
                            }
                        }

                        if (IsPositionOnBoard(selectedX - 1, selectedY) &&
                            IsPositionOnBoard(selectedX - 1, selectedY - 1) && enPassant == selectedX - 1)
                        {
                            if (GetPieceAt(selectedX - 1, selectedY) != ChessPiece.None &&
                                GetPlayerColor(GetPieceAt(selectedX - 1, selectedY)) != currentPlayer &&
                                GetPieceAt(selectedX - 1, selectedY - 1) == ChessPiece.None)
                            {
                                legalMoves.Add(new Tuple<int, int>(selectedX - 1, selectedY - 1));
                            }
                        }

                        break;
                    }
                }

                legalMoves.AddRange(PawnMove(selectedX, selectedY - 1));
                break;
            case ChessPiece.WhitePawn:
                switch (selectedY)
                {
                    case 1 when GetPieceAt(selectedX, selectedY + 1) == ChessPiece.None &&
                                GetPieceAt(selectedX, selectedY + 2) == ChessPiece.None:
                        legalMoves.Add(new Tuple<int, int>(selectedX, selectedY + 2));
                        break;
                    case 4:
                    {
                        if (IsPositionOnBoard(selectedX + 1, selectedY) &&
                            IsPositionOnBoard(selectedX - 1, selectedY + 1) && enPassant == selectedX + 1)
                        {
                            if (GetPieceAt(selectedX + 1, selectedY) != ChessPiece.None &&
                                GetPlayerColor(GetPieceAt(selectedX + 1, selectedY)) != currentPlayer &&
                                GetPieceAt(selectedX + 1, selectedY + 1) == ChessPiece.None)
                            {
                                legalMoves.Add(new Tuple<int, int>(selectedX + 1, selectedY + 1));
                            }
                        }

                        if (IsPositionOnBoard(selectedX - 1, selectedY) &&
                            IsPositionOnBoard(selectedX - 1, selectedY + 1) && enPassant == selectedX - 1)
                        {
                            if (GetPieceAt(selectedX - 1, selectedY) != ChessPiece.None &&
                                GetPlayerColor(GetPieceAt(selectedX - 1, selectedY)) != currentPlayer &&
                                GetPieceAt(selectedX - 1, selectedY + 1) == ChessPiece.None)
                            {
                                legalMoves.Add(new Tuple<int, int>(selectedX - 1, selectedY + 1));
                            }
                        }

                        break;
                    }
                }

                legalMoves.AddRange(PawnMove(selectedX, selectedY + 1));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(selectedPiece),
                    "Selected None piece for LegalMove checking");
        }

        if (!check) return legalMoves;
        List<Tuple<int, int>> legalMoves2 = new();
        foreach (var t in legalMoves)
        {
            ChessGame clone = this.DeepClone();
            clone.Play(t.Item1, t.Item2, selectedX, selectedY, false);
            if (!clone.IsKingInDanger())
            {
                legalMoves2.Add(t);
            }
        }

        return legalMoves2;
    }

    private List<Tuple<int, int>> LineMove(int xIncrement, int yIncrement, int selectedX, int selectedY)
    {
        List<Tuple<int, int>> legalMoves = new();

        var x = selectedX + xIncrement;
        var y = selectedY + yIncrement;

        while (IsPositionOnBoard(x, y) && GetPieceAt(x, y) == ChessPiece.None)
        {
            legalMoves.Add(new Tuple<int, int>(x, y));
            x += xIncrement;
            y += yIncrement;
        }

        if (IsPositionOnBoard(x, y) && GetPlayerColor(GetPieceAt(x, y)) != currentPlayer)
        {
            legalMoves.Add(new Tuple<int, int>(x, y));
        }

        return legalMoves;
    }

    private List<Tuple<int, int>> LMove(int selectedX, int selectedY)
    {
        List<Tuple<int, int>> temp = new()
        {
            PointMove(selectedX + 1, selectedY + 2),
            PointMove(selectedX - 1, selectedY + 2),
            PointMove(selectedX + 2, selectedY + 1),
            PointMove(selectedX + 2, selectedY - 1),
            PointMove(selectedX + 1, selectedY - 2),
            PointMove(selectedX - 1, selectedY - 2),
            PointMove(selectedX - 2, selectedY + 1),
            PointMove(selectedX - 2, selectedY - 1)
        };

        List<Tuple<int, int>> list = new List<Tuple<int, int>>();
        foreach (var t in temp)
        {
            if (t != null) list.Add(t);
        }

        return list;
    }

    private List<Tuple<int, int>> SurroundMove(int selectedX, int selectedY)
    {
        List<Tuple<int, int>> temp = new()
        {
            PointMove(selectedX, selectedY + 1),
            PointMove(selectedX, selectedY - 1),
            PointMove(selectedX - 1, selectedY),
            PointMove(selectedX - 1, selectedY - 1),
            PointMove(selectedX - 1, selectedY + 1),
            PointMove(selectedX + 1, selectedY),
            PointMove(selectedX + 1, selectedY - 1),
            PointMove(selectedX + 1, selectedY + 1)
        };

        List<Tuple<int, int>> list = new List<Tuple<int, int>>();
        foreach (var t in temp)
        {
            if (t != null) list.Add(t);
        }

        return list;
    }

    private Tuple<int, int> PointMove(int x, int y)
    {
        if (!IsPositionOnBoard(x, y)) return null;
        return GetPlayerColor(GetPieceAt(x, y)) != currentPlayer ? new Tuple<int, int>(x, y) : null;
    }

    private List<Tuple<int, int>> PawnMove(int x, int y)
    {
        List<Tuple<int, int>> legalMoves = new();

        if (!IsPositionOnBoard(x, y)) return legalMoves;
        if (GetPieceAt(x, y) == ChessPiece.None)
        {
            legalMoves.Add(new Tuple<int, int>(x, y));
        }

        if (IsPositionOnBoard(x + 1, y) && GetPieceAt(x + 1, y) != ChessPiece.None &&
            GetPlayerColor(GetPieceAt(x + 1, y)) != currentPlayer)
        {
            legalMoves.Add(new Tuple<int, int>(x + 1, y));
        }

        if (IsPositionOnBoard(x - 1, y) && GetPieceAt(x - 1, y) != ChessPiece.None &&
            GetPlayerColor(GetPieceAt(x - 1, y)) != currentPlayer)
        {
            legalMoves.Add(new Tuple<int, int>(x - 1, y));
        }

        return legalMoves;
    }

    public List<Tuple<int, int>> GetPlayerPiecesPos()
    {
        List<Tuple<int, int>> playerPiecesPos = new();

        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                if (GetPlayerColor(GetPieceAt(i, j)) == currentPlayer)
                {
                    playerPiecesPos.Add(new Tuple<int, int>(i, j));
                }
            }
        }

        if (playerPiecesPos.Count == 0)
        {
            throw new ArgumentException(GetCurrentPlayer() + " has no pieces.");
        }

        return playerPiecesPos;
    }

    private void NextTurn()
    {
        currentPlayer = currentPlayer == "White" ? "Black" : "White";
    }

    private bool IsPositionOnBoard(int x, int y)
    {
        return x >= 0 && y >= 0 && x < board.GetLength(0) && y < board.GetLength(1);
    }

    //Setters/Getters
    public ChessPiece[,] GetBoard()
    {
        return board;
    }

    public ChessPiece GetPieceAt(int x, int y)
    {
        return board[x, y];
    }

    public float GetScore()
    {
        return score;
    }

    private void SetGameState(GameState gameState)
    {
        gameOver = true;
        state = gameState;
        switch (gameState)
        {
            case GameState.DrawNoMaterial:
            case GameState.DrawStalemate:
                score = 0f;
                break;
            case GameState.WhiteWinCheckmate:
                score = 100f;
                break;
            case GameState.BlackWinCheckmate:
                score = -100f;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
        }
    }

    public GameState GetState()
    {
        return state;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    //Static
    public static string GetPlayerColor(ChessPiece piece)
    {
        return piece switch
        {
            ChessPiece.BlackQueen => "Black",
            ChessPiece.BlackKnight => "Black",
            ChessPiece.BlackRook => "Black",
            ChessPiece.BlackBishop => "Black",
            ChessPiece.BlackKing => "Black",
            ChessPiece.BlackPawn => "Black",
            ChessPiece.WhiteQueen => "White",
            ChessPiece.WhiteKnight => "White",
            ChessPiece.WhiteRook => "White",
            ChessPiece.WhiteBishop => "White",
            ChessPiece.WhiteKing => "White",
            ChessPiece.WhitePawn => "White",
            _ => "None"
        };
    }
}