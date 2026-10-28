using UnityEngine;
using UnityEngine.InputSystem;

namespace ConstruyendoMiComunidad
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform Objetivo;
        public Vector3 DesfaseMenu = new Vector3(16f, 11f, -18f);
        public Vector3 MirarMenu = new Vector3(0f, 1.5f, 0f);

        [SerializeField] float distancia = 6.2f;
        [SerializeField] float altura = 1.7f;
        [SerializeField] float sensibilidad = 0.12f;
        [SerializeField] float pitchMin = -12f;
        [SerializeField] float pitchMax = 55f;

        float _yaw = 180f;
        float _pitch = 12f;

        void LateUpdate()
        {
            if (Objetivo == null)
                return;

            var gm = GameManager.Instance;
            if (gm == null || gm.Estado == GameState.Menu || gm.Estado == GameState.Victory)
            {
                transform.position = DesfaseMenu;
                transform.LookAt(MirarMenu);
                return;
            }

            if (gm.Estado == GameState.Playing)
            {
                var raton = Mouse.current;
                if (raton != null)
                {
                    Vector2 delta = raton.delta.ReadValue();
                    _yaw += delta.x * sensibilidad;
                    _pitch -= delta.y * sensibilidad;
                    _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);
                }
            }

            Quaternion rot = Quaternion.Euler(_pitch, _yaw, 0f);
            Vector3 pivote = Objetivo.position + Vector3.up * altura;
            Vector3 deseada = pivote - rot * Vector3.forward * distancia;

            if (Physics.SphereCast(pivote, 0.25f, (deseada - pivote).normalized, out RaycastHit hit, distancia, ~0, QueryTriggerInteraction.Ignore))
                deseada = pivote + (deseada - pivote).normalized * Mathf.Max(0.6f, hit.distance - 0.3f);

            transform.position = deseada;
            transform.LookAt(pivote);
        }
    }
}
