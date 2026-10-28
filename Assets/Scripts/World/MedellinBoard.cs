using UnityEngine;

namespace ConstruyendoMiComunidad
{
    public class MedellinBoard : MonoBehaviour, IInteractable
    {
        public string Prompt => "Pulsa E para estudiar el caso de Medellín";

        public bool CanInteract =>
            MissionManager.Instance != null &&
            MissionManager.Instance.MisionActual == 4 &&
            !MissionManager.Instance.EmparejamientoCompletado &&
            GameManager.Instance.Estado == GameState.Playing;

        public void Interact(PlayerController player)
        {
            GameManager.Instance.AbrirEmparejamiento();
        }
    }
}
