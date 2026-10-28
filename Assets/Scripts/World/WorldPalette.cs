using System.Collections.Generic;
using UnityEngine;

namespace ConstruyendoMiComunidad
{
    public static class WorldPalette
    {
        static readonly Dictionary<Color, Material> Cache = new Dictionary<Color, Material>();
        static readonly Dictionary<Color, Material> MetalCache = new Dictionary<Color, Material>();
        static Shader _shader;

        public static void Reiniciar()
        {
            Cache.Clear();
            MetalCache.Clear();
            _shader = null;
        }

        public static Material Color(Color color) => Crear(color, false);

        public static Material Metal(Color color) => Crear(color, true);

        static Material Crear(Color color, bool metal)
        {
            if (_shader == null)
            {
                // Este proyecto está configurado para URP; no uses Standard, que se muestra magenta en URP.
                _shader = Shader.Find("Universal Render Pipeline/Lit");
                if (_shader == null)
                    _shader = Shader.Find("Universal Render Pipeline/Simple Lit");
                if (_shader == null)
                    _shader = Shader.Find("Universal Render Pipeline/Unlit");
            }

            if (_shader == null)
                throw new System.InvalidOperationException("No se encontró un shader URP. Verifica que Universal Render Pipeline esté instalado.");

            var cache = metal ? MetalCache : Cache;
            if (cache.TryGetValue(color, out var existente))
                return existente;

            var mat = new Material(_shader) { name = (metal ? "CMC_Metal_" : "CMC_") + color.ToString() };
            if (mat.HasProperty("_BaseColor"))
                mat.SetColor("_BaseColor", color);
            if (mat.HasProperty("_Color"))
                mat.SetColor("_Color", color);
            // Acabado mate uniforme: evita el aspecto plástico de los materiales por defecto.
            if (mat.HasProperty("_Metallic"))
                mat.SetFloat("_Metallic", metal ? 0.72f : 0f);
            if (mat.HasProperty("_Smoothness"))
                mat.SetFloat("_Smoothness", metal ? 0.46f : 0.18f);
            if (mat.HasProperty("_Glossiness"))
                mat.SetFloat("_Glossiness", metal ? 0.46f : 0.18f);
            mat.enableInstancing = true;
            mat.color = color;
            cache[color] = mat;
            return mat;
        }
    }
}
