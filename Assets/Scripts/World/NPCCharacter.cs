using UnityEngine;

namespace ConstruyendoMiComunidad
{
    public class NPCCharacter : MonoBehaviour, IInteractable
    {
        public string Id;
        public string Nombre;
        public string Problema;
        public string Dialogo;

        public string Prompt => "Pulsa E para hablar con " + Nombre;
        public bool CanInteract => GameManager.Instance != null && GameManager.Instance.Estado == GameState.Playing;

        public void Interact(PlayerController player)
        {
            GameManager.Instance.EntrarDialogo();
            UIManager.Instance.MostrarDialogo(Nombre, Dialogo, () =>
            {
                MissionManager.Instance.RegistrarNpc(Id);
                if (GameManager.Instance.Estado != GameState.Educational)
                    GameManager.Instance.ContinuarJuego();
            });
        }
    }
}
