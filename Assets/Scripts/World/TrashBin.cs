using UnityEngine;

namespace ConstruyendoMiComunidad
{
    public class TrashBin : MonoBehaviour, IInteractable
    {
        public TrashKind TipoAceptado;

        public string Prompt =>
            TipoAceptado == TrashKind.Reciclable
                ? "Pulsa E para depositar en la caneca de reciclaje"
                : "Pulsa E para depositar en la caneca orgánica";

        public bool CanInteract =>
            MissionManager.Instance != null &&
            MissionManager.Instance.MisionActual == 1 &&
            GameManager.Instance.Estado == GameState.Playing;

        public void Interact(PlayerController player)
        {
            if (player.ResiduoLlevado == null)
            {
                UIManager.Instance.MostrarAviso("Primero recoge un residuo en el parque.");
                return;
            }

            if (player.ResiduoLlevado.Tipo != TipoAceptado)
            {
                UIManager.Instance.MostrarAviso("Este residuo no corresponde a esta caneca. Observa el color.");
                return;
            }

            player.SoltarEnCaneca();
            MissionManager.Instance.RegistrarDeposito();
            UIManager.Instance.MostrarAviso("¡Bien! Clasificar residuos es una forma concreta de cuidar lo público.");
        }
    }
}
