using System;
using UnityEngine;
using UnityEngine.UI;

namespace ConstruyendoMiComunidad
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        GameObject _menu;
        GameObject _instrucciones;
        GameObject _pausa;
        GameObject _hud;
        GameObject _dialogo;
        GameObject _educativo;
        GameObject _asamblea;
        GameObject _emparejamiento;
        GameObject _victoria;
        GameObject _aviso;

        Text _puntos;
        Text _mision;
        Text _prompt;
        Text _avisoTexto;
        Text _dialogoNombre;
        Text _dialogoCuerpo;
        Text _eduTitulo;
        Text _eduCuerpo;
        Text _victoriaCuerpo;
        Action _alCerrarDialogo;
        Action _alCerrarEducativo;

        Toggle _tLimpieza;
        Toggle _tActividades;
        Toggle _tEspacios;
        Toggle _tGrupo;

        readonly int[] _pares = { -1, -1, -1, -1 };
        int _problemaActivo = -1;
        Button[] _btnProblemas;
        Button[] _btnSoluciones;
        Text _matchFeedback;

        static readonly string[] Problemas =
        {
            "Parque abandonado",
            "Calles con residuos",
            "Poca comunicación entre vecinos",
            "Falta de actividades para jóvenes"
        };

        static readonly string[] Soluciones =
        {
            "Jornadas de limpieza organizadas por la comunidad",
            "Asambleas y un grupo comunitario de comunicación",
            "Actividades culturales y deportivas conjuntas",
            "Recuperar el espacio público con quienes lo habitan"
        };

        // pares correctos: problema -> indice de solución
        static readonly int[] Correctos = { 3, 0, 1, 2 };

        float _avisoTiempo;

        void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            if (_aviso != null && _aviso.activeSelf)
            {
                _avisoTiempo -= Time.unscaledDeltaTime;
                if (_avisoTiempo <= 0f)
                    _aviso.SetActive(false);
            }
        }

        public void Construir()
        {
            var canvas = UiFactory.CrearCanvas("UIJuego");
            canvas.transform.SetParent(transform, false);

            ConstruirMenu(canvas.transform);
            ConstruirInstrucciones(canvas.transform);
            ConstruirPausa(canvas.transform);
            ConstruirHud(canvas.transform);
            ConstruirDialogo(canvas.transform);
            ConstruirEducativo(canvas.transform);
            ConstruirAsamblea(canvas.transform);
            ConstruirEmparejamiento(canvas.transform);
            ConstruirVictoria(canvas.transform);
            ConstruirAviso(canvas.transform);

            QuizManager.Instance.Construir(canvas.transform);

            _menu.SetActive(true);
            _hud.SetActive(false);
        }

        void ConstruirMenu(Transform canvas)
        {
            var fondo = UiFactory.Panel(canvas, Vector2.zero, Vector2.one, new Color(0.05f, 0.12f, 0.14f, 0.55f));
            _menu = fondo.gameObject;
            _menu.name = "MenuPrincipal";

            var caja = UiFactory.Caja(_menu.transform, new Vector2(820, 620), new Color(0.96f, 0.95f, 0.9f, 0.95f));
            UiFactory.Texto(caja.transform, GameContent.Titulo, 48, new Color(0.12f, 0.38f, 0.34f)).rectTransform.anchoredPosition = new Vector2(0, 200);
            var sub = UiFactory.Texto(caja.transform, GameContent.Subtitulo, 24, new Color(0.25f, 0.32f, 0.3f), TextAnchor.MiddleCenter, new Vector2(700, 80));
            sub.rectTransform.anchoredPosition = new Vector2(0, 130);

            var b1 = UiFactory.Boton(caja.transform, "Iniciar partida", new Vector2(360, 64), () => GameManager.Instance.IniciarPartida());
            b1.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 30);
            var b2 = UiFactory.Boton(caja.transform, "Instrucciones", new Vector2(360, 64), () => GameManager.Instance.AbrirInstrucciones(), new Color(0.22f, 0.45f, 0.58f));
            b2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50);
            var b3 = UiFactory.Boton(caja.transform, "Salir", new Vector2(360, 64), () => GameManager.Instance.Salir(), new Color(0.55f, 0.28f, 0.25f));
            b3.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -130);
        }

        void ConstruirInstrucciones(Transform canvas)
        {
            var fondo = UiFactory.Panel(canvas, Vector2.zero, Vector2.one, new Color(0.05f, 0.1f, 0.12f, 0.7f));
            _instrucciones = fondo.gameObject;
            _instrucciones.name = "Instrucciones";
            _instrucciones.SetActive(false);
            var caja = UiFactory.Caja(_instrucciones.transform, new Vector2(980, 760), new Color(0.97f, 0.96f, 0.92f));
            UiFactory.Texto(caja.transform, "Instrucciones", 40, new Color(0.12f, 0.35f, 0.32f)).rectTransform.anchoredPosition = new Vector2(0, 330);
            var cuerpo = UiFactory.Texto(caja.transform, GameContent.Instrucciones, 22, new Color(0.15f, 0.15f, 0.15f), TextAnchor.UpperLeft, new Vector2(860, 560));
            cuerpo.rectTransform.anchoredPosition = new Vector2(0, -20);
            var btn = UiFactory.Boton(caja.transform, "Volver", new Vector2(240, 56), () => GameManager.Instance.CerrarInstrucciones());
            btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -330);
        }

        void ConstruirPausa(Transform canvas)
        {
            var fondo = UiFactory.Panel(canvas, Vector2.zero, Vector2.one, new Color(0f, 0f, 0f, 0.55f));
            _pausa = fondo.gameObject;
            _pausa.name = "Pausa";
            _pausa.SetActive(false);
            var caja = UiFactory.Caja(_pausa.transform, new Vector2(520, 420), new Color(0.96f, 0.95f, 0.9f));
            UiFactory.Texto(caja.transform, "Pausa", 42, new Color(0.12f, 0.35f, 0.32f)).rectTransform.anchoredPosition = new Vector2(0, 140);
            var r = UiFactory.Boton(caja.transform, "Continuar", new Vector2(300, 58), () => GameManager.Instance.Reanudar());
            r.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
            var m = UiFactory.Boton(caja.transform, "Menú principal", new Vector2(300, 58), () => GameManager.Instance.VolverAlMenu(), new Color(0.22f, 0.45f, 0.58f));
            m.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -40);
            var s = UiFactory.Boton(caja.transform, "Salir", new Vector2(300, 58), () => GameManager.Instance.Salir(), new Color(0.55f, 0.28f, 0.25f));
            s.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -120);
        }

        void ConstruirHud(Transform canvas)
        {
            var hud = new GameObject("HUD", typeof(RectTransform));
            hud.transform.SetParent(canvas, false);
            UiFactory.Estirar(hud.GetComponent<RectTransform>());
            _hud = hud;

            var barra = UiFactory.Panel(hud.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Color(0.08f, 0.18f, 0.18f, 0.82f));
            barra.rectTransform.pivot = new Vector2(0.5f, 1f);
            barra.rectTransform.anchoredPosition = Vector2.zero;
            barra.rectTransform.sizeDelta = new Vector2(0f, 110f);

            _puntos = UiFactory.Texto(barra.transform, "Puntos de participación: 0", 26, Color.white, TextAnchor.MiddleLeft, new Vector2(700, 40));
            _puntos.rectTransform.anchorMin = new Vector2(0f, 0.55f);
            _puntos.rectTransform.anchorMax = new Vector2(0f, 0.55f);
            _puntos.rectTransform.pivot = new Vector2(0f, 0.5f);
            _puntos.rectTransform.anchoredPosition = new Vector2(30, 0);

            _mision = UiFactory.Texto(barra.transform, "", 22, new Color(0.9f, 0.95f, 0.85f), TextAnchor.MiddleLeft, new Vector2(1400, 50));
            _mision.rectTransform.anchorMin = new Vector2(0f, 0.2f);
            _mision.rectTransform.anchorMax = new Vector2(0f, 0.2f);
            _mision.rectTransform.pivot = new Vector2(0f, 0.5f);
            _mision.rectTransform.anchoredPosition = new Vector2(30, 0);

            _prompt = UiFactory.Texto(hud.transform, "", 28, Color.white, TextAnchor.MiddleCenter, new Vector2(900, 50));
            _prompt.rectTransform.anchorMin = new Vector2(0.5f, 0f);
            _prompt.rectTransform.anchorMax = new Vector2(0.5f, 0f);
            _prompt.rectTransform.pivot = new Vector2(0.5f, 0f);
            _prompt.rectTransform.anchoredPosition = new Vector2(0, 40);
        }

        void ConstruirDialogo(Transform canvas)
        {
            var fondo = UiFactory.Panel(canvas, new Vector2(0.12f, 0.04f), new Vector2(0.88f, 0.34f), new Color(0.1f, 0.16f, 0.18f, 0.92f));
            _dialogo = fondo.gameObject;
            _dialogo.name = "Dialogo";
            _dialogo.SetActive(false);
            _dialogoNombre = UiFactory.Texto(fondo.transform, "Nombre", 28, new Color(0.55f, 0.9f, 0.75f), TextAnchor.UpperLeft, new Vector2(1200, 40));
            _dialogoNombre.rectTransform.anchoredPosition = new Vector2(0, 70);
            _dialogoCuerpo = UiFactory.Texto(fondo.transform, "Texto", 24, Color.white, TextAnchor.UpperLeft, new Vector2(1300, 110));
            _dialogoCuerpo.rectTransform.anchoredPosition = new Vector2(0, -10);
            var btn = UiFactory.Boton(fondo.transform, "Continuar", new Vector2(220, 48), CerrarDialogo);
            btn.GetComponent<RectTransform>().anchorMin = btn.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0f);
            btn.GetComponent<RectTransform>().pivot = new Vector2(1f, 0f);
            btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-20, 16);
        }

        void ConstruirEducativo(Transform canvas)
        {
            var fondo = UiFactory.Panel(canvas, Vector2.zero, Vector2.one, new Color(0.05f, 0.1f, 0.12f, 0.72f));
            _educativo = fondo.gameObject;
            _educativo.name = "Educativo";
            _educativo.SetActive(false);
            var caja = UiFactory.Caja(_educativo.transform, new Vector2(980, 560), new Color(0.97f, 0.96f, 0.9f));
            _eduTitulo = UiFactory.Texto(caja.transform, "Título", 34, new Color(0.12f, 0.38f, 0.32f));
            _eduTitulo.rectTransform.anchoredPosition = new Vector2(0, 210);
            _eduCuerpo = UiFactory.Texto(caja.transform, "", 24, new Color(0.15f, 0.15f, 0.15f), TextAnchor.UpperLeft, new Vector2(860, 280));
            _eduCuerpo.rectTransform.anchoredPosition = new Vector2(0, -20);
            var btn = UiFactory.Boton(caja.transform, "Continuar", new Vector2(260, 56), CerrarEducativo);
            btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -220);
        }

        void ConstruirAsamblea(Transform canvas)
        {
            var fondo = UiFactory.Panel(canvas, Vector2.zero, Vector2.one, new Color(0.05f, 0.1f, 0.12f, 0.75f));
            _asamblea = fondo.gameObject;
            _asamblea.name = "Asamblea";
            _asamblea.SetActive(false);
            var caja = UiFactory.Caja(_asamblea.transform, new Vector2(980, 720), new Color(0.97f, 0.96f, 0.9f));
            UiFactory.Texto(caja.transform, "Asamblea de vecinos", 36, new Color(0.12f, 0.35f, 0.32f)).rectTransform.anchoredPosition = new Vector2(0, 310);
            UiFactory.Texto(caja.transform, "Selecciona las soluciones que la comunidad pondrá en marcha. Cada acuerdo tendrá un efecto visible en el barrio.", 22, new Color(0.2f, 0.2f, 0.2f), TextAnchor.MiddleCenter, new Vector2(860, 80)).rectTransform.anchoredPosition = new Vector2(0, 240);

            _tLimpieza = ToggleOpcion(caja.transform, "Organizar una jornada de limpieza con los vecinos", 140);
            _tActividades = ToggleOpcion(caja.transform, "Proponer actividades culturales y deportivas", 60);
            _tEspacios = ToggleOpcion(caja.transform, "Recuperar los espacios públicos del barrio", -20);
            _tGrupo = ToggleOpcion(caja.transform, "Crear un grupo comunitario para identificar necesidades", -100);

            var btn = UiFactory.Boton(caja.transform, "Aprobar acuerdos", new Vector2(320, 58), ConfirmarAsamblea);
            btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -280);
        }

        Toggle ToggleOpcion(Transform parent, string etiqueta, float y)
        {
            var go = new GameObject("Opcion", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(820, 64);
            rt.anchoredPosition = new Vector2(0, y);
            var bg = go.AddComponent<Image>();
            bg.sprite = UiFactory.Blanco;
            bg.color = new Color(0.88f, 0.93f, 0.9f);

            var toggleGo = new GameObject("Toggle", typeof(RectTransform), typeof(Toggle), typeof(Image));
            toggleGo.transform.SetParent(go.transform, false);
            var trt = toggleGo.GetComponent<RectTransform>();
            trt.anchorMin = new Vector2(0f, 0.5f);
            trt.anchorMax = new Vector2(0f, 0.5f);
            trt.pivot = new Vector2(0f, 0.5f);
            trt.anchoredPosition = new Vector2(16, 0);
            trt.sizeDelta = new Vector2(36, 36);
            var img = toggleGo.GetComponent<Image>();
            img.sprite = UiFactory.Blanco;
            img.color = Color.white;
            var toggle = toggleGo.GetComponent<Toggle>();
            toggle.targetGraphic = img;
            toggle.isOn = true;
            var check = new GameObject("Check", typeof(RectTransform), typeof(Image));
            check.transform.SetParent(toggleGo.transform, false);
            UiFactory.Estirar(check.GetComponent<RectTransform>());
            check.GetComponent<RectTransform>().offsetMin = new Vector2(6, 6);
            check.GetComponent<RectTransform>().offsetMax = new Vector2(-6, -6);
            var cimg = check.GetComponent<Image>();
            cimg.sprite = UiFactory.Blanco;
            cimg.color = new Color(0.16f, 0.55f, 0.42f);
            toggle.graphic = cimg;

            var t = UiFactory.Texto(go.transform, etiqueta, 22, new Color(0.12f, 0.12f, 0.12f), TextAnchor.MiddleLeft, new Vector2(740, 50));
            t.rectTransform.anchoredPosition = new Vector2(40, 0);
            return toggle;
        }

        void ConstruirEmparejamiento(Transform canvas)
        {
            var fondo = UiFactory.Panel(canvas, Vector2.zero, Vector2.one, new Color(0.05f, 0.1f, 0.14f, 0.78f));
            _emparejamiento = fondo.gameObject;
            _emparejamiento.name = "Emparejamiento";
            _emparejamiento.SetActive(false);
            var caja = UiFactory.Caja(_emparejamiento.transform, new Vector2(1200, 780), new Color(0.97f, 0.96f, 0.92f));
            UiFactory.Texto(caja.transform, "El caso de Medellín (2004-2011)", 34, new Color(0.12f, 0.32f, 0.4f)).rectTransform.anchoredPosition = new Vector2(0, 340);
            UiFactory.Texto(caja.transform,
                "El Proyecto Urbano Integral articuló lo físico, lo social y lo institucional, con la comunidad como protagonista. Relaciona cada problema ficticio del barrio con una solución construida en conjunto. Esto es un ejercicio educativo; no forma parte del proyecto histórico.",
                20, new Color(0.2f, 0.2f, 0.2f), TextAnchor.UpperCenter, new Vector2(1080, 90)).rectTransform.anchoredPosition = new Vector2(0, 265);

            _btnProblemas = new Button[4];
            _btnSoluciones = new Button[4];
            for (int i = 0; i < 4; i++)
            {
                int pi = i;
                var bp = UiFactory.Boton(caja.transform, Problemas[i], new Vector2(480, 64), () => ElegirProblema(pi), new Color(0.25f, 0.45f, 0.55f));
                bp.GetComponent<RectTransform>().anchoredPosition = new Vector2(-280, 160 - i * 80);
                _btnProblemas[i] = bp;

                int si = i;
                var bs = UiFactory.Boton(caja.transform, Soluciones[i], new Vector2(520, 64), () => ElegirSolucion(si), new Color(0.3f, 0.52f, 0.4f));
                bs.GetComponent<RectTransform>().anchoredPosition = new Vector2(300, 160 - i * 80);
                _btnSoluciones[i] = bs;
            }

            _matchFeedback = UiFactory.Texto(caja.transform, "Elige un problema y luego la solución que le corresponde.", 22, new Color(0.2f, 0.2f, 0.2f), TextAnchor.MiddleCenter, new Vector2(1000, 50));
            _matchFeedback.rectTransform.anchoredPosition = new Vector2(0, -200);

            var comprobar = UiFactory.Boton(caja.transform, "Comprobar relaciones", new Vector2(360, 58), ComprobarEmparejamiento);
            comprobar.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -280);
        }

        void ConstruirVictoria(Transform canvas)
        {
            var fondo = UiFactory.Panel(canvas, Vector2.zero, Vector2.one, new Color(0.05f, 0.14f, 0.12f, 0.82f));
            _victoria = fondo.gameObject;
            _victoria.name = "Victoria";
            _victoria.SetActive(false);
            var caja = UiFactory.Caja(_victoria.transform, new Vector2(980, 720), new Color(0.96f, 0.97f, 0.9f));
            UiFactory.Texto(caja.transform, "¡Comunidad en marcha!", 42, new Color(0.12f, 0.42f, 0.3f)).rectTransform.anchoredPosition = new Vector2(0, 300);
            _victoriaCuerpo = UiFactory.Texto(caja.transform, "", 22, new Color(0.15f, 0.15f, 0.15f), TextAnchor.UpperLeft, new Vector2(860, 460));
            _victoriaCuerpo.rectTransform.anchoredPosition = new Vector2(0, -20);
            var b1 = UiFactory.Boton(caja.transform, "Jugar de nuevo", new Vector2(280, 56), () => GameManager.Instance.VolverAlMenu());
            b1.GetComponent<RectTransform>().anchoredPosition = new Vector2(-170, -300);
            var b2 = UiFactory.Boton(caja.transform, "Salir", new Vector2(220, 56), () => GameManager.Instance.Salir(), new Color(0.55f, 0.28f, 0.25f));
            b2.GetComponent<RectTransform>().anchoredPosition = new Vector2(170, -300);
        }

        void ConstruirAviso(Transform canvas)
        {
            var img = UiFactory.Panel(canvas, new Vector2(0.2f, 0.12f), new Vector2(0.8f, 0.2f), new Color(0.1f, 0.18f, 0.16f, 0.9f));
            _aviso = img.gameObject;
            _aviso.name = "Aviso";
            _aviso.SetActive(false);
            _avisoTexto = UiFactory.Texto(_aviso.transform, "", 22, Color.white, TextAnchor.MiddleCenter, new Vector2(1000, 70));
        }

        public void MostrarJuego()
        {
            _menu.SetActive(false);
            _instrucciones.SetActive(false);
            _hud.SetActive(true);
            ActualizarHud();
        }

        public void MostrarInstrucciones(bool visible)
        {
            _instrucciones.SetActive(visible);
        }

        public void MostrarPausa(bool visible)
        {
            _pausa.SetActive(visible);
        }

        public void MostrarPrompt(string texto)
        {
            if (_prompt != null)
                _prompt.text = texto;
        }

        public void MostrarAviso(string texto)
        {
            _avisoTexto.text = texto;
            _aviso.SetActive(true);
            _avisoTiempo = 3.2f;
        }

        public void MostrarDialogo(string nombre, string texto, Action alCerrar)
        {
            _alCerrarDialogo = alCerrar;
            _dialogoNombre.text = nombre;
            _dialogoCuerpo.text = texto;
            _dialogo.SetActive(true);
        }

        void CerrarDialogo()
        {
            _dialogo.SetActive(false);
            _alCerrarDialogo?.Invoke();
        }

        public void MostrarEducativo(string titulo, string texto, Action alCerrar)
        {
            _alCerrarEducativo = alCerrar;
            _eduTitulo.text = titulo;
            _eduCuerpo.text = texto;
            _educativo.SetActive(true);
        }

        void CerrarEducativo()
        {
            _educativo.SetActive(false);
            _alCerrarEducativo?.Invoke();
        }

        public void MostrarAsamblea()
        {
            _asamblea.SetActive(true);
        }

        void ConfirmarAsamblea()
        {
            if (!_tLimpieza.isOn && !_tActividades.isOn && !_tEspacios.isOn && !_tGrupo.isOn)
            {
                MostrarAviso("La asamblea necesita al menos un acuerdo.");
                return;
            }

            _asamblea.SetActive(false);
            MissionManager.Instance.CompletarAsamblea(_tLimpieza.isOn, _tActividades.isOn, _tEspacios.isOn, _tGrupo.isOn);
        }

        public void MostrarEmparejamiento()
        {
            for (int i = 0; i < 4; i++)
                _pares[i] = -1;
            _problemaActivo = -1;
            _emparejamiento.SetActive(true);
            _matchFeedback.text = "Elige un problema y luego la solución que le corresponde.";
        }

        void ElegirProblema(int i)
        {
            _problemaActivo = i;
            _matchFeedback.text = "Problema elegido: " + Problemas[i] + ". Ahora elige una solución.";
        }

        void ElegirSolucion(int i)
        {
            if (_problemaActivo < 0)
            {
                _matchFeedback.text = "Primero elige un problema a la izquierda.";
                return;
            }

            _pares[_problemaActivo] = i;
            _matchFeedback.text = Problemas[_problemaActivo] + " → " + Soluciones[i];
            _problemaActivo = -1;
        }

        void ComprobarEmparejamiento()
        {
            int bien = 0;
            for (int i = 0; i < 4; i++)
            {
                if (_pares[i] == Correctos[i])
                    bien++;
            }

            if (bien < 4)
            {
                _matchFeedback.text = $"Llevas {bien} de 4 relaciones correctas. Piensa cómo la comunidad construye cada solución. Inténtalo de nuevo.";
                return;
            }

            _emparejamiento.SetActive(false);
            MissionManager.Instance.CompletarEmparejamiento();
        }

        public void MostrarVictoria()
        {
            CerrarPanelesModales();
            _hud.SetActive(true);
            var m = MissionManager.Instance;
            float pct = m.QuizTotal > 0 ? 100f * m.QuizCorrectas / m.QuizTotal : 0f;
            _victoriaCuerpo.text =
                $"Puntos de participación comunitaria: {CommunityPoints.Instance.Total}\n" +
                $"Cuestionario: {m.QuizCorrectas} de {m.QuizTotal} ({pct:0} %)\n\n" +
                GameContent.ResumenAprendizajes;
            _victoria.SetActive(true);
        }

        public void CerrarPanelesModales()
        {
            _dialogo.SetActive(false);
            _educativo.SetActive(false);
            _asamblea.SetActive(false);
            _emparejamiento.SetActive(false);
        }

        public void OcultarHudParaModal(bool ocultar)
        {
            if (_hud != null)
                _hud.SetActive(!ocultar);
        }

        public void ActualizarHud()
        {
            if (_puntos != null && CommunityPoints.Instance != null)
                _puntos.text = "Puntos de participación: " + CommunityPoints.Instance.Total;
            if (_mision != null && MissionManager.Instance != null)
                _mision.text = MissionManager.Instance.TextoHud();
        }

        void Start()
        {
            if (CommunityPoints.Instance != null)
                CommunityPoints.Instance.Changed += OnPuntos;
            if (MissionManager.Instance != null)
                MissionManager.Instance.ProgresoCambiado += ActualizarHud;
        }

        void OnDestroy()
        {
            if (CommunityPoints.Instance != null)
                CommunityPoints.Instance.Changed -= OnPuntos;
            if (MissionManager.Instance != null)
                MissionManager.Instance.ProgresoCambiado -= ActualizarHud;
        }

        void OnPuntos(int _)
        {
            ActualizarHud();
        }
    }
}
