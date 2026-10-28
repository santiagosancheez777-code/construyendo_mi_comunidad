using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConstruyendoMiComunidad
{
    /// <summary>
    /// Controla las 6 misiones, el progreso y los puntos por completarlas.
    /// </summary>
    public class MissionManager : MonoBehaviour
    {
        public const int TotalMisiones = 6;

        public static MissionManager Instance { get; private set; }

        public int MisionActual { get; private set; }
        public int NpcsEscuchados { get; private set; }
        public int ResiduosDepositados { get; private set; }
        public int ResiduosTotales { get; private set; } = 8;
        public int Reparaciones { get; private set; }
        public int ReparacionesTotales { get; private set; } = 3;
        public int QuizCorrectas { get; private set; }
        public int QuizTotal { get; private set; }
        public bool AsambleaCompletada { get; private set; }
        public bool EmparejamientoCompletado { get; private set; }

        public bool LimpiezaElegida { get; private set; }
        public bool ActividadesElegidas { get; private set; }
        public bool EspaciosElegidos { get; private set; }
        public bool GrupoElegido { get; private set; }

        readonly HashSet<string> _npcs = new HashSet<string>();
        public event Action ProgresoCambiado;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            MisionActual = 0;
        }

        public string TextoHud()
        {
            if (MisionActual >= TotalMisiones)
                return "Has completado todas las misiones.";

            string extra = MisionActual switch
            {
                0 => $"  ({NpcsEscuchados}/3)",
                1 => $"  ({ResiduosDepositados}/{ResiduosTotales})",
                3 => $"  ({Reparaciones}/{ReparacionesTotales})",
                _ => string.Empty
            };

            return GameContent.MisionTitulos[MisionActual] + "\n" +
                   GameContent.MisionObjetivos[MisionActual] + extra;
        }

        public void RegistrarNpc(string id)
        {
            if (!_npcs.Add(id))
                return;

            NpcsEscuchados++;
            CommunityPoints.Instance.TryAward("npc_" + id, 10);
            ProgresoCambiado?.Invoke();

            if (MisionActual == 0 && NpcsEscuchados >= 3)
                CompletarMisionActual();
        }

        public void ConfigurarResiduos(int total)
        {
            ResiduosTotales = total;
        }

        public void RegistrarDeposito()
        {
            ResiduosDepositados++;
            ProgresoCambiado?.Invoke();

            if (MisionActual == 1 && ResiduosDepositados >= ResiduosTotales)
            {
                CommunityPoints.Instance.TryAward("actividad_parque", 20);
                CompletarMisionActual();
            }
        }

        public void CompletarAsamblea(bool limpieza, bool actividades, bool espacios, bool grupo)
        {
            if (AsambleaCompletada)
                return;

            LimpiezaElegida = limpieza;
            ActividadesElegidas = actividades;
            EspaciosElegidos = espacios;
            GrupoElegido = grupo;
            AsambleaCompletada = true;
            CommunityPoints.Instance.TryAward("actividad_asamblea", 20);
            ProgresoCambiado?.Invoke();

            if (MisionActual == 2)
                CompletarMisionActual();
        }

        public void RegistrarReparacion()
        {
            Reparaciones++;
            ProgresoCambiado?.Invoke();

            if (MisionActual == 3 && Reparaciones >= ReparacionesTotales)
            {
                CommunityPoints.Instance.TryAward("actividad_reparacion", 20);
                CompletarMisionActual();
            }
        }

        public void CompletarEmparejamiento()
        {
            if (EmparejamientoCompletado)
                return;

            EmparejamientoCompletado = true;
            CommunityPoints.Instance.TryAward("actividad_medellin", 20);
            ProgresoCambiado?.Invoke();

            if (MisionActual == 4)
                CompletarMisionActual();
        }

        public void CompletarCuestionario(int correctas, int total)
        {
            QuizCorrectas = correctas;
            QuizTotal = total;
            ProgresoCambiado?.Invoke();

            if (MisionActual == 5)
                CompletarMisionActual();
        }

        void CompletarMisionActual()
        {
            int indice = MisionActual;
            CommunityPoints.Instance.TryAward("mision_" + indice, 30);

            string mensaje = GameContent.MensajesEducativos[Mathf.Clamp(indice, 0, GameContent.MensajesEducativos.Length - 1)];
            MisionActual = Mathf.Min(indice + 1, TotalMisiones);
            ProgresoCambiado?.Invoke();

            VisualProgress.Instance?.Aplicar(this);
            GameManager.Instance.MostrarEducativo(GameContent.MisionTitulos[indice], mensaje, indice == TotalMisiones - 1);
        }
    }
}
