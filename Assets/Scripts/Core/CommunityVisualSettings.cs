using UnityEngine;

namespace ConstruyendoMiComunidad
{
    /// <summary>Visual replacements only. Gameplay components stay on the generated roots.</summary>
    [CreateAssetMenu(menuName = "Construyendo mi comunidad/Configuracion visual", fileName = "CommunityVisualSettings")]
    public sealed class CommunityVisualSettings : ScriptableObject
    {
        [Header("Personajes (modelos como hijos; deja vacío para usar el modelo incluido)")]
        public GameObject playerModel;
        public Vector3 playerModelOffset = Vector3.zero;
        public Vector3 playerModelRotation = Vector3.zero;
        public Vector3 playerModelScale = Vector3.one;
        public GameObject[] residentModels = new GameObject[5];
        public Vector3 residentModelOffset = Vector3.zero;
        public Vector3 residentModelRotation = Vector3.zero;
        public Vector3 residentModelScale = Vector3.one;

        [Header("Apariencia del barrio")]
        public Color grass = new Color(.45f, .72f, .32f);
        public Color road = new Color(.35f, .36f, .38f);
        public Color sidewalk = new Color(.72f, .71f, .68f);
        public Color houseWallA = new Color(.95f, .78f, .42f);
        public Color houseWallB = new Color(.93f, .55f, .45f);
        public Color houseWallC = new Color(.55f, .72f, .88f);
        public Color roof = new Color(.75f, .28f, .22f);
        public Color treeCanopy = new Color(.22f, .55f, .28f);
        public Color communityCenter = new Color(.95f, .92f, .82f);

        public GameObject GetResidentModel(int index) => residentModels != null && index >= 0 && index < residentModels.Length ? residentModels[index] : null;
    }
}
