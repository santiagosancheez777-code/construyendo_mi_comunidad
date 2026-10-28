using UnityEngine;

namespace ConstruyendoMiComunidad
{
    public class QuizTrigger : MonoBehaviour, IInteractable
    {
        public string Prompt => "Pulsa E para presentar el cuestionario";

        public bool CanInteract =>
            MissionManager.Instance != null &&
            MissionManager.Instance.MisionActual == 5 &&
            GameManager.Instance.Estado == GameState.Playing;

        public void Interact(PlayerController player)
        {
            GameManager.Instance.AbrirCuestionario();
        }
    }
}
