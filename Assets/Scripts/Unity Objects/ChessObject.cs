using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Unity_Objects
{
    public class ChessObject : MonoBehaviour
    {
        //References
        public GameObject controller;
        public GameObject movePlate;

        //Positions
        private int xBoard = -1;
        private int yBoard = -1;

        //Player color
        private string player;

        public Sprite b_queen, b_knight, b_bishop, b_king, b_rook, b_pawn;
        public Sprite w_queen, w_knight, w_bishop, w_king, w_rook, w_pawn;

        public void Activate()
        {
            controller = GameObject.FindGameObjectWithTag("GameController");

            SetCoords();

            switch (name)
            {
                case "b_queen":
                    this.GetComponent<SpriteRenderer>().sprite = b_queen;
                    player = "Black";
                    break;
                case "b_knight":
                    this.GetComponent<SpriteRenderer>().sprite = b_knight;
                    player = "Black";
                    break;
                case "b_rook":
                    this.GetComponent<SpriteRenderer>().sprite = b_rook;
                    player = "Black";
                    break;
                case "b_bishop":
                    this.GetComponent<SpriteRenderer>().sprite = b_bishop;
                    player = "Black";
                    break;
                case "b_king":
                    this.GetComponent<SpriteRenderer>().sprite = b_king;
                    player = "Black";
                    break;
                case "b_pawn":
                    this.GetComponent<SpriteRenderer>().sprite = b_pawn;
                    player = "Black";
                    break;
                case "w_queen":
                    this.GetComponent<SpriteRenderer>().sprite = w_queen;
                    player = "White";
                    break;
                case "w_knight":
                    this.GetComponent<SpriteRenderer>().sprite = w_knight;
                    player = "White";
                    break;
                case "w_rook":
                    this.GetComponent<SpriteRenderer>().sprite = w_rook;
                    player = "White";
                    break;
                case "w_bishop":
                    this.GetComponent<SpriteRenderer>().sprite = w_bishop;
                    player = "White";
                    break;
                case "w_king":
                    this.GetComponent<SpriteRenderer>().sprite = w_king;
                    player = "White";
                    break;
                case "w_pawn":
                    this.GetComponent<SpriteRenderer>().sprite = w_pawn;
                    player = "White";
                    break;
            }
        }

        private void OnMouseUp()
        {
            if (controller.GetComponent<Game>().IsGameOver() ||
                controller.GetComponent<Game>().GetCurrentPlayer() != player) return;
            DestroyMovePlates();

            InitiateMovePlates();
        }
    
        public void DestroyMovePlates()
        {
            GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
            foreach (var t in movePlates)
            {
                Destroy(t);
            }
        }

        private void InitiateMovePlates()
        {
            Game sc = controller.GetComponent<Game>();
            List<Tuple<int, int>> legalMoves = sc.chessGame.GetLegalMoves(xBoard, yBoard, true);

            foreach (var t in legalMoves)
            {
                string playerColor = ChessGame.GetPlayerColor(sc.GetPieceAt(t.Item1, t.Item2));
                if (playerColor != player && playerColor != "None")
                {
                    MovePlateSpawn(t.Item1, t.Item2, true);
                }
                else
                {
                    MovePlateSpawn(t.Item1, t.Item2, false);
                }
            }
        }

        private void MovePlateSpawn(int matrixX, int matrixY, bool attack)
        {
            float x = matrixX;
            float y = matrixY;

            x *= 0.70f;
            y *= 0.70f;

            x += -2.45f;
            y += -2.45f;

            GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

            MovePlate mpScript = mp.GetComponent<MovePlate>();
            mpScript.SetReference(gameObject);
            mpScript.SetCoords(matrixX, matrixY);
            if (attack)
            {
                mpScript.SetAttack();
            }
        }

        //Setters/Getters
        private void SetCoords()
        {
            float x = xBoard;
            float y = yBoard;

            x *= 0.70f;
            y *= 0.70f;

            x += -2.45f;
            y += -2.45f;

            this.transform.position = new Vector3(x, y, -1.0f);
        }

        public int GetXBoard()
        {
            return xBoard;
        }

        public int GetYBoard()
        {
            return yBoard;
        }

        public void SetXBoard(int x)
        {
            xBoard = x;
        }

        public void SetYBoard(int y)
        {
            yBoard = y;
        }
    }
}