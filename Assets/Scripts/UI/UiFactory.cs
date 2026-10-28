using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ConstruyendoMiComunidad
{
    public static class UiFactory
    {
        static Font _fuente;
        static Sprite _blanco;

        public static Font Fuente
        {
            get
            {
                if (_fuente == null)
                {
                    _fuente = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                    if (_fuente == null)
                        _fuente = Resources.GetBuiltinResource<Font>("Arial.ttf");
                }

                return _fuente;
            }
        }

        public static Sprite Blanco
        {
            get
            {
                if (_blanco == null)
                {
                    var tex = Texture2D.whiteTexture;
                    _blanco = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 4f);
                }

                return _blanco;
            }
        }

        public static Canvas CrearCanvas(string nombre)
        {
            var go = new GameObject(nombre);
            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10;
            var scaler = go.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            go.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        public static Image Panel(Transform parent, Vector2 anclaMin, Vector2 anclaMax, Color color)
        {
            var go = new GameObject("Panel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = anclaMin;
            rt.anchorMax = anclaMax;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            var img = go.GetComponent<Image>();
            img.sprite = Blanco;
            img.color = color;
            img.raycastTarget = true;
            return img;
        }

        public static Image Caja(Transform parent, Vector2 size, Color color)
        {
            var img = Panel(parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), color);
            img.rectTransform.sizeDelta = size;
            return img;
        }

        public static Text Texto(Transform parent, string contenido, int size, Color color, TextAnchor align = TextAnchor.MiddleCenter, Vector2? tamano = null)
        {
            var go = new GameObject("Texto", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = tamano ?? new Vector2(900, 70);
            var t = go.GetComponent<Text>();
            t.font = Fuente;
            t.text = contenido;
            t.fontSize = size;
            t.color = color;
            t.alignment = align;
            t.horizontalOverflow = HorizontalWrapMode.Wrap;
            t.verticalOverflow = VerticalWrapMode.Overflow;
            t.raycastTarget = false;
            return t;
        }

        public static Button Boton(Transform parent, string etiqueta, Vector2 size, UnityAction accion, Color? fondo = null)
        {
            var go = new GameObject("Boton_" + etiqueta, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = size;
            var img = go.GetComponent<Image>();
            img.sprite = Blanco;
            img.color = fondo ?? new Color(0.16f, 0.55f, 0.48f);
            var btn = go.GetComponent<Button>();
            btn.targetGraphic = img;
            var colors = btn.colors;
            colors.highlightedColor = new Color(0.22f, 0.68f, 0.58f);
            colors.pressedColor = new Color(0.1f, 0.38f, 0.34f);
            btn.colors = colors;
            var t = Texto(go.transform, etiqueta, 26, Color.white);
            Estirar(t.rectTransform);
            t.raycastTarget = false;
            btn.onClick.AddListener(accion);
            return btn;
        }

        public static void Estirar(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    }
}
