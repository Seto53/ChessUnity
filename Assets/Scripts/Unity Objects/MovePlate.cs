using Core;
using Player;
using UnityEngine;

namespace Unity_Objects
{
    public class MovePlate : MonoBehaviour
    {
        //Some functions will need reference to the controller
        private GameObject controller;

        //The Chesspiece that was tapped to create this MovePlate
        private GameObject reference = null;

        private int matrixX;
        private int matrixY;

        private bool attack;

        public void Start()
        {
            if (attack)
            {
                //Set to red
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
        }

        public void OnMouseUp()
        {
            controller = GameObject.FindGameObjectWithTag("GameController");
            if (controller.GetComponent<Game>().GetCurrentPlayer() == "White")
            {
                ChessObject cm = reference.GetComponent<ChessObject>();
                ((HumanPlayer)controller.GetComponent<Game>().WhitePlayer).SetMove(new Move(cm.GetXBoard(), cm.GetYBoard(),
                    matrixX, matrixY));
            }
            else
            {
                ChessObject cm = reference.GetComponent<ChessObject>();
                ((HumanPlayer)controller.GetComponent<Game>().BlackPlayer).SetMove(new Move(
                    cm.GetXBoard(), cm.GetYBoard(), matrixX, matrixY));
            }
        }

        //Setters/Getters
        public void SetCoords(int x, int y)
        {
            matrixX = x;
            matrixY = y;
        }

        public void SetReference(GameObject obj)
        {
            reference = obj;
        }

        public void SetAttack()
        {
            attack = true;
        }
    }
}