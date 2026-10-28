using UnityEngine;

namespace ConstruyendoMiComunidad
{
    /// <summary>
    /// Activa mejoras visuales del barrio según las misiones y decisiones.
    /// </summary>
    public class VisualProgress : MonoBehaviour
    {
        public static VisualProgress Instance { get; private set; }

        public GameObject ParqueSucio;
        public GameObject ParqueLimpio;
        public GameObject ExtraLimpieza;
        public GameObject ExtraActividades;
        public GameObject ExtraEspacios;
        public GameObject ExtraGrupo;
        public GameObject ZonaDeteriorada;
        public GameObject ZonaRestaurada;

        void Awake()
        {
            Instance = this;
        }

        public void Aplicar(MissionManager m)
        {
            if (ParqueSucio != null)
                ParqueSucio.SetActive(m.MisionActual < 2);
            if (ParqueLimpio != null)
                ParqueLimpio.SetActive(m.MisionActual >= 2);
            if (ExtraLimpieza != null)
                ExtraLimpieza.SetActive(m.LimpiezaElegida);
            if (ExtraActividades != null)
                ExtraActividades.SetActive(m.ActividadesElegidas);
            if (ExtraEspacios != null)
                ExtraEspacios.SetActive(m.EspaciosElegidos);
            if (ExtraGrupo != null)
                ExtraGrupo.SetActive(m.GrupoElegido);
            if (ZonaDeteriorada != null)
                ZonaDeteriorada.SetActive(m.MisionActual < 4);
            if (ZonaRestaurada != null)
                ZonaRestaurada.SetActive(m.MisionActual >= 4);
        }

        public void MarcarReparacion(Transform spot)
        {
            foreach (var r in spot.GetComponentsInChildren<Renderer>())
                r.material = WorldPalette.Color(new Color(0.45f, 0.78f, 0.42f));
        }
    }
}
