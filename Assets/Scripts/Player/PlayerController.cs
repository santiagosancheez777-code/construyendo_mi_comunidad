using UnityEngine;
using UnityEngine.InputSystem;

namespace ConstruyendoMiComunidad
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float velocidad = 5.5f;
        [SerializeField] float velocidadCarrera = 8.5f;
        [SerializeField] float gravedad = -24f;
        [SerializeField] float salto = 6.5f;

        CharacterController _cc;
        float _velocidadY;
        Transform _camara;

        public TrashItem ResiduoLlevado { get; private set; }

        void Awake()
        {
            _cc = GetComponent<CharacterController>();
        }

        public void AsignarCamara(Transform camara)
        {
            _camara = camara;
        }

        void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.BloqueaJugador)
                return;

            var teclado = Keyboard.current;
            if (teclado == null)
                return;

            Vector2 input = Vector2.zero;
            if (teclado.wKey.isPressed || teclado.upArrowKey.isPressed) input.y += 1f;
            if (teclado.sKey.isPressed || teclado.downArrowKey.isPressed) input.y -= 1f;
            if (teclado.dKey.isPressed || teclado.rightArrowKey.isPressed) input.x += 1f;
            if (teclado.aKey.isPressed || teclado.leftArrowKey.isPressed) input.x -= 1f;
            input = Vector2.ClampMagnitude(input, 1f);

            Transform referencia = _camara != null ? _camara : transform;
            Vector3 adelante = referencia.forward;
            Vector3 derecha = referencia.right;
            adelante.y = 0f;
            derecha.y = 0f;
            adelante.Normalize();
            derecha.Normalize();

            bool correr = teclado.leftShiftKey.isPressed || teclado.rightShiftKey.isPressed;
            float vel = correr ? velocidadCarrera : velocidad;
            Vector3 movimiento = (adelante * input.y + derecha * input.x) * vel;

            if (movimiento.sqrMagnitude > 0.01f)
            {
                Quaternion rot = Quaternion.LookRotation(movimiento.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 12f * Time.deltaTime);
            }

            if (_cc.isGrounded)
            {
                _velocidadY = -1.5f;
                if (teclado.spaceKey.wasPressedThisFrame)
                    _velocidadY = salto;
            }
            else
            {
                _velocidadY += gravedad * Time.deltaTime;
            }

            movimiento.y = _velocidadY;
            _cc.Move(movimiento * Time.deltaTime);

            if (ResiduoLlevado != null)
                ResiduoLlevado.transform.position = transform.position + Vector3.up * 1.85f + transform.forward * 0.4f;
        }

        public void Recoger(TrashItem item)
        {
            ResiduoLlevado = item;
            item.AlRecoger();
        }

        public void SoltarEnCaneca()
        {
            if (ResiduoLlevado == null)
                return;

            Destroy(ResiduoLlevado.gameObject);
            ResiduoLlevado = null;
        }
    }
}
