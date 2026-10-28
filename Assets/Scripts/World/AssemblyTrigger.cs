using UnityEngine;

namespace ConstruyendoMiComunidad
{
    public class AssemblyTrigger : MonoBehaviour, IInteractable
    {
        public string Prompt => "Pulsa E para entrar a la asamblea de vecinos";

        public bool CanInteract =>
            MissionManager.Instance != null &&
            MissionManager.Instance.MisionActual == 2 &&
            !MissionManager.Instance.AsambleaCompletada &&
            GameManager.Instance.Estado == GameState.Playing;

        public void Interact(PlayerController player)
        {
            GameManager.Instance.AbrirAsamblea();
        }
    }
}
