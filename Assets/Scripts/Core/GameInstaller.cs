using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace ConstruyendoMiComunidad
{
    /// <summary>
    /// Arranca el juego en cualquier escena: crea el barrio, el jugador, la UI y los sistemas.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public class GameInstaller : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Arrancar()
        {
            if (FindFirstObjectByType<GameInstaller>() != null)
                return;

            var root = new GameObject("GameRoot");
            root.AddComponent<GameInstaller>();
        }

        void Awake()
        {
            // Evita reutilizar materiales de una sesión anterior al entrar en Play sin recarga de dominio.
            WorldPalette.Reiniciar();

            if (FindFirstObjectByType<CommunityPoints>() == null)
                gameObject.AddComponent<CommunityPoints>();
            if (FindFirstObjectByType<MissionManager>() == null)
                gameObject.AddComponent<MissionManager>();
            if (FindFirstObjectByType<GameManager>() == null)
                gameObject.AddComponent<GameManager>();
            if (FindFirstObjectByType<QuizManager>() == null)
                gameObject.AddComponent<QuizManager>();
            if (FindFirstObjectByType<UIManager>() == null)
                gameObject.AddComponent<UIManager>();

            AsegurarEventSystem();
            AsegurarBarrio();
            var jugador = AsegurarJugador();
            AsegurarCamara(jugador);
            AjustarIluminacion();

            MissionManager.Instance.ConfigurarResiduos(8);
            UIManager.Instance.Construir();
            VisualProgress.Instance?.Aplicar(MissionManager.Instance);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        static void AsegurarEventSystem()
        {
            if (FindFirstObjectByType<EventSystem>() != null)
                return;

            var go = new GameObject("EventSystem");
            go.AddComponent<EventSystem>();
            go.AddComponent<InputSystemUIInputModule>();
        }

        static void AsegurarBarrio()
        {
            if (GameObject.Find("NeighborhoodRoot") != null)
                return;

            NeighborhoodBuilder.Build();
        }

        static Transform AsegurarJugador()
        {
            var existente = FindFirstObjectByType<PlayerController>();
            if (existente != null)
                return existente.transform;

            var player = new GameObject("Protagonista");
            player.layer = 2;
            player.tag = "Untagged";
            player.transform.position = new Vector3(0f, 0.1f, -16f);

            var cc = player.AddComponent<CharacterController>();
            cc.height = 2.5f;
            cc.radius = 0.34f;
            cc.center = new Vector3(0f, 1.25f, 0f);
            cc.minMoveDistance = 0f;

            var visualSettings = Resources.Load<CommunityVisualSettings>("CommunityVisualSettings");
            if (visualSettings != null && visualSettings.playerModel != null)
            {
                var model = Instantiate(visualSettings.playerModel, player.transform);
                model.name = "ModeloVisual";
                model.transform.localPosition = visualSettings.playerModelOffset;
                model.transform.localRotation = Quaternion.Euler(visualSettings.playerModelRotation);
                model.transform.localScale = visualSettings.playerModelScale;
                SetLayer(model, 0);
                foreach (var visualCollider in model.GetComponentsInChildren<Collider>(true))
                    visualCollider.enabled = false;
            }
            else
            {
                CrearProtagonista(player.transform);
            }

            player.AddComponent<PlayerController>();
            player.AddComponent<InteractionSystem>();
            return player.transform;
        }

        static void CrearProtagonista(Transform raiz)
        {
            Color piel = new Color(0.86f, 0.63f, 0.47f);
            Color chaqueta = new Color(0.68f, 0.055f, 0.13f);
            Color camiseta = new Color(0.96f, 0.81f, 0.39f);
            Color pantalon = new Color(0.075f, 0.13f, 0.23f);
            Color cabello = new Color(0.055f, 0.09f, 0.17f);
            Color botas = new Color(0.075f, 0.07f, 0.065f);

            ParteJugador(raiz, PrimitiveType.Capsule, new Vector3(0f, 1.34f, 0f), new Vector3(0.32f, 0.43f, 0.25f), camiseta, "CamisetaAmarilla");
            ParteJugador(raiz, PrimitiveType.Cube, new Vector3(-0.19f, 1.34f, 0.08f), new Vector3(0.2f, 0.79f, 0.27f), chaqueta, "ChaquetaRoja_Izquierda");
            ParteJugador(raiz, PrimitiveType.Cube, new Vector3(0.19f, 1.34f, 0.08f), new Vector3(0.2f, 0.79f, 0.27f), chaqueta, "ChaquetaRoja_Derecha");
            ParteJugador(raiz, PrimitiveType.Cube, new Vector3(0f, 1.35f, 0.222f), new Vector3(0.025f, 0.69f, 0.025f), new Color(0.92f, 0.62f, 0.39f), "Cremallera");

            var mangaIzq = ParteJugador(raiz, PrimitiveType.Capsule, new Vector3(-0.39f, 1.28f, 0.015f), new Vector3(0.17f, 0.43f, 0.17f), chaqueta, "MangaIzquierda");
            mangaIzq.transform.localRotation = Quaternion.Euler(0f, 0f, -7f);
            var mangaDer = ParteJugador(raiz, PrimitiveType.Capsule, new Vector3(0.39f, 1.28f, 0.015f), new Vector3(0.17f, 0.43f, 0.17f), chaqueta, "MangaDerecha");
            mangaDer.transform.localRotation = Quaternion.Euler(0f, 0f, 7f);
            ParteJugador(raiz, PrimitiveType.Cube, new Vector3(-0.4f, 0.85f, 0.02f), new Vector3(0.17f, 0.09f, 0.18f), new Color(0.83f, 0.1f, 0.17f), "PuñoIzquierdo");
            ParteJugador(raiz, PrimitiveType.Cube, new Vector3(0.4f, 0.85f, 0.02f), new Vector3(0.17f, 0.09f, 0.18f), new Color(0.83f, 0.1f, 0.17f), "PuñoDerecho");
            ParteJugador(raiz, PrimitiveType.Sphere, new Vector3(-0.4f, 0.72f, 0.04f), new Vector3(0.12f, 0.16f, 0.11f), piel, "ManoIzquierda");
            ParteJugador(raiz, PrimitiveType.Sphere, new Vector3(0.4f, 0.72f, 0.04f), new Vector3(0.12f, 0.16f, 0.11f), piel, "ManoDerecha");

            ParteJugador(raiz, PrimitiveType.Capsule, new Vector3(-0.16f, 0.53f, 0f), new Vector3(0.18f, 0.53f, 0.19f), pantalon, "Pantalon_Izquierdo");
            ParteJugador(raiz, PrimitiveType.Capsule, new Vector3(0.16f, 0.53f, 0f), new Vector3(0.18f, 0.53f, 0.19f), pantalon, "Pantalon_Derecho");
            ParteJugador(raiz, PrimitiveType.Cube, new Vector3(-0.16f, 0.1f, 0.075f), new Vector3(0.2f, 0.17f, 0.33f), botas, "BotaIzquierda");
            ParteJugador(raiz, PrimitiveType.Cube, new Vector3(0.16f, 0.1f, 0.075f), new Vector3(0.2f, 0.17f, 0.33f), botas, "BotaDerecha");

            ParteJugador(raiz, PrimitiveType.Cylinder, new Vector3(0f, 1.82f, 0f), new Vector3(0.13f, 0.14f, 0.13f), piel, "Cuello");
            ParteJugador(raiz, PrimitiveType.Sphere, new Vector3(0f, 2.08f, 0f), new Vector3(0.36f, 0.43f, 0.32f), piel, "Cabeza");
            ParteJugador(raiz, PrimitiveType.Sphere, new Vector3(0f, 2.39f, -0.035f), new Vector3(0.39f, 0.23f, 0.35f), cabello, "CabelloSuperior");
            var flequillo = ParteJugador(raiz, PrimitiveType.Sphere, new Vector3(-0.13f, 2.34f, 0.2f), new Vector3(0.31f, 0.16f, 0.19f), cabello, "FlequilloLateral");
            flequillo.transform.localRotation = Quaternion.Euler(0f, 0f, -18f);
            ParteJugador(raiz, PrimitiveType.Sphere, new Vector3(0.27f, 2.23f, 0.015f), new Vector3(0.15f, 0.25f, 0.19f), cabello, "PatillaDerecha");

            ParteJugador(raiz, PrimitiveType.Sphere, new Vector3(-0.35f, 2.06f, 0f), new Vector3(0.09f, 0.13f, 0.09f), piel, "OrejaIzquierda");
            ParteJugador(raiz, PrimitiveType.Sphere, new Vector3(0.35f, 2.06f, 0f), new Vector3(0.09f, 0.13f, 0.09f), piel, "OrejaDerecha");
            Color blancoOjo = new Color(0.96f, 0.93f, 0.84f);
            Color iris = new Color(0.16f, 0.52f, 0.74f);
            foreach (int lado in new[] { -1, 1 })
            {
                float x = lado * 0.12f;
                ParteJugador(raiz, PrimitiveType.Sphere, new Vector3(x, 2.12f, 0.286f), new Vector3(0.095f, 0.1f, 0.045f), blancoOjo, lado < 0 ? "OjoBlanco_I" : "OjoBlanco_D");
                ParteJugador(raiz, PrimitiveType.Sphere, new Vector3(x, 2.12f, 0.328f), new Vector3(0.055f, 0.06f, 0.025f), iris, lado < 0 ? "IrisAzul_I" : "IrisAzul_D");
                ParteJugador(raiz, PrimitiveType.Sphere, new Vector3(x, 2.12f, 0.35f), new Vector3(0.027f, 0.034f, 0.015f), cabello, lado < 0 ? "Pupila_I" : "Pupila_D");
                var ceja = ParteJugador(raiz, PrimitiveType.Cube, new Vector3(x, 2.235f, 0.3f), new Vector3(0.14f, 0.035f, 0.04f), cabello, lado < 0 ? "Ceja_I" : "Ceja_D");
                ceja.transform.localRotation = Quaternion.Euler(0f, 0f, lado < 0 ? -5f : 5f);
            }
            ParteJugador(raiz, PrimitiveType.Sphere, new Vector3(0f, 2.02f, 0.34f), new Vector3(0.075f, 0.1f, 0.08f), piel, "Nariz");
            ParteJugador(raiz, PrimitiveType.Cube, new Vector3(0f, 1.94f, 0.312f), new Vector3(0.1f, 0.018f, 0.018f), new Color(0.48f, 0.19f, 0.17f), "Sonrisa");

            ParteJugador(raiz, PrimitiveType.Cube, new Vector3(-0.16f, 1.57f, 0.235f), new Vector3(0.025f, 0.18f, 0.025f), new Color(0.88f, 0.78f, 0.58f), "CordónCapucha_I");
            ParteJugador(raiz, PrimitiveType.Cube, new Vector3(0.16f, 1.57f, 0.235f), new Vector3(0.025f, 0.18f, 0.025f), new Color(0.88f, 0.78f, 0.58f), "CordónCapucha_D");
            ParteJugador(raiz, PrimitiveType.Sphere, new Vector3(-0.16f, 1.46f, 0.25f), Vector3.one * 0.045f, new Color(0.88f, 0.78f, 0.58f), "TerminalCordón_I");
            ParteJugador(raiz, PrimitiveType.Sphere, new Vector3(0.16f, 1.46f, 0.25f), Vector3.one * 0.045f, new Color(0.88f, 0.78f, 0.58f), "TerminalCordón_D");
        }

        static void AsegurarCamara(Transform jugador)
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                var go = new GameObject("Main Camera");
                go.tag = "MainCamera";
                cam = go.AddComponent<Camera>();
                go.AddComponent<AudioListener>();
                go.AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
            }

            var follow = cam.GetComponent<CameraFollow>();
            if (follow == null)
                follow = cam.gameObject.AddComponent<CameraFollow>();
            follow.Objetivo = jugador;
            jugador.GetComponent<PlayerController>().AsignarCamara(cam.transform);

            // El barrio se construye antes que la cámara; enlazarla aquí permite ver las etiquetas 3D de los vecinos.
            foreach (var canvas in FindObjectsByType<Canvas>(FindObjectsSortMode.None))
                if (canvas.renderMode == RenderMode.WorldSpace)
                    canvas.worldCamera = cam;
        }

        static void AjustarIluminacion()
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.55f, 0.7f, 0.85f);
            RenderSettings.ambientEquatorColor = new Color(0.5f, 0.55f, 0.45f);
            RenderSettings.ambientGroundColor = new Color(0.25f, 0.22f, 0.18f);
        }

        static void SetLayer(GameObject go, int layer)
        {
            go.layer = layer;
            foreach (Transform child in go.transform)
                SetLayer(child.gameObject, layer);
        }

        static GameObject ParteJugador(Transform parent, PrimitiveType tipo, Vector3 posicion, Vector3 escala, Color color, string nombre)
        {
            var parte = GameObject.CreatePrimitive(tipo);
            parte.name = nombre;
            parte.transform.SetParent(parent);
            parte.transform.localPosition = posicion;
            parte.transform.localScale = escala;
            parte.GetComponent<Renderer>().sharedMaterial = WorldPalette.Color(color);
            var collider = parte.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
                Destroy(collider);
            }
        SetLayer(parte, 0);
            return parte;
        }
    }
}
