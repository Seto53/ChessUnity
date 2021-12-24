using UnityEngine;

namespace Unity_Objects
{
    public class ButtonHandler : MonoBehaviour
    {
        public void UndoMove()
        {
            GameObject controller = GameObject.FindGameObjectWithTag("GameController");
            controller.GetComponent<Game>().UndoMove();
        }
    }
}
