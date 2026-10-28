using UnityEngine;

namespace ConstruyendoMiComunidad
{
    public enum TrashKind
    {
        Reciclable,
        Organico
    }

    public class TrashItem : MonoBehaviour, IInteractable
    {
        public TrashKind Tipo;
        bool _recogido;

        public string Prompt =>
            Tipo == TrashKind.Reciclable
                ? "Pulsa E para recoger residuo reciclable"
                : "Pulsa E para recoger residuo orgánico";

        public bool CanInteract =>
            !_recogido &&
            MissionManager.Instance != null &&
            MissionManager.Instance.MisionActual == 1 &&
            GameManager.Instance.Estado == GameState.Playing;

        public void Interact(PlayerController player)
        {
            if (player.ResiduoLlevado != null)
            {
                UIManager.Instance.MostrarAviso("Ya llevas un residuo. Depósitalo en la caneca del mismo color.");
                return;
            }

            player.Recoger(this);
        }

        public void AlRecoger()
        {
            _recogido = true;
            var col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;
        }
    }
}
