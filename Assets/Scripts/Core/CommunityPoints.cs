using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConstruyendoMiComunidad
{
    /// <summary>
    /// Puntos de participación. Cada clave se otorga una sola vez.
    /// </summary>
    public class CommunityPoints : MonoBehaviour
    {
        public static CommunityPoints Instance { get; private set; }

        public int Total { get; private set; }
        public event Action<int> Changed;

        readonly HashSet<string> _otorgados = new HashSet<string>();

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public bool TryAward(string clave, int cantidad)
        {
            if (string.IsNullOrEmpty(clave) || cantidad <= 0)
                return false;

            if (!_otorgados.Add(clave))
                return false;

            Total += cantidad;
            Changed?.Invoke(Total);
            return true;
        }

        public bool YaOtorgado(string clave) => _otorgados.Contains(clave);
    }
}
