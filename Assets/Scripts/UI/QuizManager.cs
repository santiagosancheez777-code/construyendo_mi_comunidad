using UnityEngine;
using UnityEngine.UI;

namespace ConstruyendoMiComunidad
{
    public class QuizManager : MonoBehaviour
    {
        public static QuizManager Instance { get; private set; }

        struct Pregunta
        {
            public string Enunciado;
            public string[] Opciones;
            public int Correcta;
            public string Explicacion;
        }

        readonly Pregunta[] _preguntas =
        {
            new Pregunta
            {
                Enunciado = "¿Qué es la participación comunitaria?",
                Opciones = new[]
                {
                    "Dejar que solo las instituciones decidan por el barrio.",
                    "El proceso en el que las personas se involucran de forma activa en los asuntos de su territorio.",
                    "Una actividad obligatoria de un día al año.",
                    "Un trámite para pedir un favor personal."
                },
                Correcta = 1,
                Explicacion = "Participar es involucrarse de manera activa y organizada en lo que afecta a la comunidad."
            },
            new Pregunta
            {
                Enunciado = "¿Cuál es un beneficio de la colaboración comunitaria?",
                Opciones = new[]
                {
                    "Que una sola persona reciba todos los recursos.",
                    "Evitar cualquier reunión entre vecinos.",
                    "Mejor convivencia, cuidado de lo público y sentido de pertenencia.",
                    "Eliminar las diferencias de opinión."
                },
                Correcta = 2,
                Explicacion = "Colaborar mejora la convivencia, el cuidado de los espacios y el orgullo de pertenecer al barrio."
            },
            new Pregunta
            {
                Enunciado = "¿Cuál es una forma de participación comunitaria?",
                Opciones = new[]
                {
                    "Ignorar los problemas del espacio público.",
                    "Asambleas, jornadas de trabajo, grupos vecinales y actividades culturales.",
                    "Decidir en secreto sin informar a nadie.",
                    "Esperar a que el problema desaparezca solo."
                },
                Correcta = 1,
                Explicacion = "Hay muchas formas de participar: reunirse, trabajar en conjunto, organizarse y proponer actividades."
            },
            new Pregunta
            {
                Enunciado = "¿Por qué es importante la toma de decisiones colectivas?",
                Opciones = new[]
                {
                    "Porque hace más lentos todos los procesos y eso es lo deseable.",
                    "Porque evita escuchar a las personas afectadas.",
                    "Porque permite soluciones más adecuadas y sostenibles, pensadas con quienes viven el problema.",
                    "Porque elimina la necesidad de instituciones."
                },
                Correcta = 2,
                Explicacion = "Decidir juntas y juntos produce acuerdos más justos y duraderos."
            },
            new Pregunta
            {
                Enunciado = "¿Qué buscó el Proyecto Urbano Integral de Medellín (2004-2011)?",
                Opciones = new[]
                {
                    "Construir solo vías rápidas sin consultar a nadie.",
                    "Articular dimensiones físicas, sociales e institucionales, involucrando a las comunidades.",
                    "Reemplazar a los barrios por centros comerciales.",
                    "Ser un videojuego educativo."
                },
                Correcta = 1,
                Explicacion = "El PUI es un ejemplo real de intervención integral con participación comunitaria. Las misiones de este juego son ficticias."
            }
        };

        GameObject _panel;
        Text _enunciado;
        Text _feedback;
        Text _progreso;
        Button[] _opciones;
        Button _continuar;
        int _indice;
        int _correctas;
        bool _respondida;

        void Awake()
        {
            Instance = this;
        }

        public void Construir(Transform canvas)
        {
            var fondo = UiFactory.Panel(canvas, Vector2.zero, Vector2.one, new Color(0.07f, 0.12f, 0.14f, 0.78f));
            _panel = fondo.gameObject;
            _panel.name = "PanelCuestionario";
            _panel.SetActive(false);

            var caja = UiFactory.Caja(_panel.transform, new Vector2(1100, 780), new Color(0.97f, 0.96f, 0.92f, 0.98f));
            UiFactory.Texto(caja.transform, "Pon a prueba tus conocimientos", 36, new Color(0.12f, 0.35f, 0.32f)).rectTransform.anchoredPosition = new Vector2(0, 340);
            var tProg = UiFactory.Texto(caja.transform, "Pregunta 1 de 5", 22, new Color(0.25f, 0.25f, 0.25f));
            tProg.rectTransform.anchoredPosition = new Vector2(0, 295);
            _progreso = tProg;

            var tEnc = UiFactory.Texto(caja.transform, "", 28, new Color(0.12f, 0.12f, 0.12f), TextAnchor.UpperCenter);
            tEnc.rectTransform.sizeDelta = new Vector2(980, 110);
            tEnc.rectTransform.anchoredPosition = new Vector2(0, 210);
            _enunciado = tEnc;

            _opciones = new Button[4];
            for (int i = 0; i < 4; i++)
            {
                int idx = i;
                var btn = UiFactory.Boton(caja.transform, "Opción", new Vector2(920, 70), () => Responder(idx), new Color(0.22f, 0.48f, 0.52f));
                btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 90 - i * 85);
                _opciones[i] = btn;
            }

            var tFeed = UiFactory.Texto(caja.transform, "", 22, new Color(0.15f, 0.35f, 0.22f), TextAnchor.MiddleCenter);
            tFeed.rectTransform.sizeDelta = new Vector2(980, 70);
            tFeed.rectTransform.anchoredPosition = new Vector2(0, -250);
            _feedback = tFeed;

            _continuar = UiFactory.Boton(caja.transform, "Siguiente", new Vector2(280, 60), Siguiente);
            _continuar.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -330);
            _continuar.gameObject.SetActive(false);
        }

        public void Iniciar()
        {
            _indice = 0;
            _correctas = 0;
            _panel.SetActive(true);
            UIManager.Instance.OcultarHudParaModal(true);
            MostrarPregunta();
        }

        void MostrarPregunta()
        {
            _respondida = false;
            _feedback.text = string.Empty;
            _continuar.gameObject.SetActive(false);
            var p = _preguntas[_indice];
            _progreso.text = $"Pregunta {_indice + 1} de {_preguntas.Length}";
            _enunciado.text = p.Enunciado;
            for (int i = 0; i < 4; i++)
            {
                _opciones[i].interactable = true;
                _opciones[i].image.color = new Color(0.22f, 0.48f, 0.52f);
                _opciones[i].GetComponentInChildren<Text>().text = p.Opciones[i];
            }
        }

        void Responder(int indice)
        {
            if (_respondida)
                return;

            _respondida = true;
            var p = _preguntas[_indice];
            bool ok = indice == p.Correcta;
            if (ok)
            {
                _correctas++;
                CommunityPoints.Instance.TryAward("quiz_" + _indice, 10);
                _feedback.color = new Color(0.12f, 0.45f, 0.22f);
                _feedback.text = "Correcto. " + p.Explicacion;
            }
            else
            {
                _feedback.color = new Color(0.55f, 0.18f, 0.15f);
                _feedback.text = "No era esa la respuesta. " + p.Explicacion;
            }

            for (int i = 0; i < 4; i++)
            {
                _opciones[i].interactable = false;
                if (i == p.Correcta)
                    _opciones[i].image.color = new Color(0.18f, 0.62f, 0.32f);
                else if (i == indice)
                    _opciones[i].image.color = new Color(0.7f, 0.28f, 0.22f);
            }

            _continuar.GetComponentInChildren<Text>().text = _indice >= _preguntas.Length - 1 ? "Ver resultados" : "Siguiente";
            _continuar.gameObject.SetActive(true);
        }

        void Siguiente()
        {
            if (_indice < _preguntas.Length - 1)
            {
                _indice++;
                MostrarPregunta();
                return;
            }

            _panel.SetActive(false);
            UIManager.Instance.OcultarHudParaModal(false);
            MissionManager.Instance.CompletarCuestionario(_correctas, _preguntas.Length);
        }
    }
}
