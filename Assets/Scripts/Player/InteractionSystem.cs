using UnityEngine;
using UnityEngine.InputSystem;

namespace ConstruyendoMiComunidad
{
    public class InteractionSystem : MonoBehaviour
    {
        [SerializeField] float radio = 2.4f;

        IInteractable _actual;

        void Update()
        {
            if (GameManager.Instance == null || GameManager.Instance.Estado != GameState.Playing)
            {
                UIManager.Instance?.MostrarPrompt(string.Empty);
                return;
            }

            _actual = BuscarCercano();
            UIManager.Instance?.MostrarPrompt(_actual != null && _actual.CanInteract ? _actual.Prompt : string.Empty);

            var teclado = Keyboard.current;
            if (teclado != null && teclado.eKey.wasPressedThisFrame && _actual != null && _actual.CanInteract)
            {
                var player = GetComponent<PlayerController>();
                _actual.Interact(player);
            }
        }

        IInteractable BuscarCercano()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position + Vector3.up, radio, ~0, QueryTriggerInteraction.Collide);
            IInteractable mejor = null;
            float mejorDist = float.MaxValue;

            foreach (var col in hits)
            {
                var interactuable = col.GetComponentInParent<IInteractable>();
                if (interactuable == null || !interactuable.CanInteract)
                    continue;

                float d = (col.transform.position - transform.position).sqrMagnitude;
                if (d < mejorDist)
                {
                    mejorDist = d;
                    mejor = interactuable;
                }
            }

            return mejor;
        }
    }
}
