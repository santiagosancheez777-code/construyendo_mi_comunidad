using UnityEngine;
using UnityEngine.InputSystem;

namespace ConstruyendoMiComunidad
{
    /// <summary>
    /// Orquesta estados del juego, inicio, pausa y cierre.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameState Estado { get; private set; } = GameState.Menu;

        public bool BloqueaJugador => Estado != GameState.Playing;

        GameState _estadoAntesDePausa = GameState.Playing;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            Estado = GameState.Menu;
            Time.timeScale = 1f;
        }

        void Update()
        {
            var teclado = Keyboard.current;
            if (teclado == null)
                return;

            if (!teclado.escapeKey.wasPressedThisFrame)
                return;

            if (Estado == GameState.Playing)
                Pausar();
            else if (Estado == GameState.Paused)
                Reanudar();
        }

        public void IniciarPartida()
        {
            Estado = GameState.Playing;
            UIManager.Instance.MostrarJuego();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void AbrirInstrucciones()
        {
            UIManager.Instance.MostrarInstrucciones(true);
        }

        public void CerrarInstrucciones()
        {
            UIManager.Instance.MostrarInstrucciones(false);
        }

        public void Salir()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void Pausar()
        {
            _estadoAntesDePausa = Estado;
            Estado = GameState.Paused;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UIManager.Instance.MostrarPausa(true);
        }

        public void Reanudar()
        {
            Estado = GameState.Playing;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            UIManager.Instance.MostrarPausa(false);
        }

        public void VolverAlMenu()
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        public void EntrarDialogo()
        {
            Estado = GameState.Dialogue;
            LiberarCursor();
        }

        public void MostrarEducativo(string titulo, string texto, bool alFinalizarMostrarVictoria)
        {
            Estado = GameState.Educational;
            LiberarCursor();
            UIManager.Instance.MostrarEducativo(titulo, texto, () =>
            {
                if (alFinalizarMostrarVictoria)
                    MostrarVictoria();
                else
                    ContinuarJuego();
            });
        }

        public void AbrirAsamblea()
        {
            if (MissionManager.Instance.MisionActual != 2 || MissionManager.Instance.AsambleaCompletada)
                return;

            Estado = GameState.Assembly;
            LiberarCursor();
            UIManager.Instance.MostrarAsamblea();
        }

        public void AbrirEmparejamiento()
        {
            if (MissionManager.Instance.MisionActual != 4 || MissionManager.Instance.EmparejamientoCompletado)
                return;

            Estado = GameState.Matching;
            LiberarCursor();
            UIManager.Instance.MostrarEmparejamiento();
        }

        public void AbrirCuestionario()
        {
            if (MissionManager.Instance.MisionActual != 5)
                return;

            Estado = GameState.Quiz;
            LiberarCursor();
            QuizManager.Instance.Iniciar();
        }

        public void ContinuarJuego()
        {
            Estado = GameState.Playing;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            UIManager.Instance.CerrarPanelesModales();
        }

        public void MostrarVictoria()
        {
            Estado = GameState.Victory;
            LiberarCursor();
            UIManager.Instance.MostrarVictoria();
        }

        void LiberarCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
