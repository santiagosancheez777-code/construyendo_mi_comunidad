using UnityEngine;

namespace ConstruyendoMiComunidad
{
    public class RepairSpot : MonoBehaviour, IInteractable
    {
        public string Accion = "recuperar este espacio";
        bool _hecho;

        public string Prompt => _hecho ? string.Empty : "Pulsa E para " + Accion;
        public bool CanInteract =>
            !_hecho &&
            MissionManager.Instance != null &&
            MissionManager.Instance.MisionActual == 3 &&
            GameManager.Instance.Estado == GameState.Playing;

        public void Interact(PlayerController player)
        {
            _hecho = true;
            VisualProgress.Instance?.MarcarReparacion(transform);
            MissionManager.Instance.RegistrarReparacion();
        }
    }
}
