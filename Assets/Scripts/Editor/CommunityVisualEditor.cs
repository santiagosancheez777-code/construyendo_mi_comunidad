using System.IO;
using System.Collections.Generic;
using ConstruyendoMiComunidad;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ConstruyendoMiComunidad.Editor
{
    public sealed class CommunityVisualEditor : EditorWindow
    {
        const string SettingsPath = "Assets/Resources/CommunityVisualSettings.asset";
        CommunityVisualSettings settings;
        SerializedObject serializedSettings;

        [MenuItem("Construyendo mi comunidad/Personalizar barrio y personajes")]
        static void Open() => GetWindow<CommunityVisualEditor>("Personalizar barrio");

        void OnEnable() => CargarConfiguracion();

        [InitializeOnLoadMethod]
        static void AsegurarCarpetasYConfiguracion()
        {
            Directory.CreateDirectory("Assets/Resources");
            Directory.CreateDirectory("Assets/Art/Characters/Player");
            Directory.CreateDirectory("Assets/Art/Characters/Residents");
            if (AssetDatabase.LoadAssetAtPath<CommunityVisualSettings>(SettingsPath) == null)
            {
                var settings = CreateInstance<CommunityVisualSettings>();
                AssetDatabase.CreateAsset(settings, SettingsPath);
                AssetDatabase.SaveAssets();
            }
        }

        void CargarConfiguracion()
        {
            settings = AssetDatabase.LoadAssetAtPath<CommunityVisualSettings>(SettingsPath);
            serializedSettings = settings != null ? new SerializedObject(settings) : null;
        }

        void OnGUI()
        {
            EditorGUILayout.HelpBox("Los modelos se añaden como hijos visuales: controles, misiones e interacciones permanecen en el personaje raíz.", MessageType.Info);
            if (settings == null)
            {
                if (GUILayout.Button("Crear configuración visual"))
                {
                    Directory.CreateDirectory("Assets/Resources");
                    settings = CreateInstance<CommunityVisualSettings>();
                    AssetDatabase.CreateAsset(settings, SettingsPath);
                    AssetDatabase.SaveAssets();
                    serializedSettings = new SerializedObject(settings);
                }
                return;
            }

            serializedSettings.Update();
            EditorGUILayout.PropertyField(serializedSettings.FindProperty("playerModel"));
            EditorGUILayout.PropertyField(serializedSettings.FindProperty("playerModelOffset"));
            EditorGUILayout.PropertyField(serializedSettings.FindProperty("playerModelRotation"));
            EditorGUILayout.PropertyField(serializedSettings.FindProperty("playerModelScale"));
            EditorGUILayout.PropertyField(serializedSettings.FindProperty("residentModels"), true);
            EditorGUILayout.PropertyField(serializedSettings.FindProperty("residentModelOffset"));
            EditorGUILayout.PropertyField(serializedSettings.FindProperty("residentModelRotation"));
            EditorGUILayout.PropertyField(serializedSettings.FindProperty("residentModelScale"));
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Colores principales", EditorStyles.boldLabel);
            string[] colors = { "grass", "road", "sidewalk", "houseWallA", "houseWallB", "houseWallC", "roof", "treeCanopy", "communityCenter" };
            foreach (var property in colors) EditorGUILayout.PropertyField(serializedSettings.FindProperty(property));
            if (serializedSettings.ApplyModifiedProperties()) EditorUtility.SetDirty(settings);

            EditorGUILayout.Space();
            if (GUILayout.Button("Generar barrio editable en la escena")) GenerarBarrioPersistente();
            var pipeline = GraphicsSettings.defaultRenderPipeline;
            var urp = pipeline as UniversalRenderPipelineAsset;
            EditorGUILayout.HelpBox(urp != null
                ? "URP activo: " + urp.name + ". Los shaders de los materiales del barrio son compatibles."
                : "No hay un Universal Render Pipeline activo. Abre Project Settings > Graphics y asigna Assets/Settings/PC_RPAsset.asset; los materiales URP se verán magenta hasta hacerlo.",
                urp != null ? MessageType.Info : MessageType.Error);
            EditorGUILayout.HelpBox("Carpetas para modelos: Assets/Art/Characters/Player y Assets/Art/Characters/Residents. Importa modelos y materiales allí, y arrástralos a esta ventana.", MessageType.None);
        }

        [MenuItem("Construyendo mi comunidad/Generar y guardar barrio editable")]
        public static void GenerarBarrioPersistente()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorUtility.DisplayDialog("Detén Play", "Sal del modo Play antes de generar objetos permanentes en la escena.", "Entendido");
                return;
            }

            var existing = GameObject.Find("NeighborhoodRoot");
            if (existing != null) DestroyImmediate(existing);
            var root = NeighborhoodBuilder.Build();
            GuardarMateriales(root.gameObject);
            CrearPrefabsReutilizables(root.gameObject);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            AssetDatabase.SaveAssets();
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            Selection.activeTransform = root;
            Debug.Log("Barrio guardado en la escena. Los materiales y prefabs reutilizables están en Assets/Resources/Materials y Assets/Prefabs/Neighborhood.");
        }

        public static void GenerarEnEscenaPrincipal()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity", OpenSceneMode.Single);
            GenerarBarrioPersistente();
        }

        static void GuardarMateriales(GameObject root)
        {
            const string folder = "Assets/Resources/Materials";
            Directory.CreateDirectory(folder);
            AssetDatabase.Refresh();
            var creados = new Dictionary<Material, Material>();
            var renderers = root.GetComponentsInChildren<Renderer>(true);
            foreach (var renderer in renderers)
            {
                var materials = renderer.sharedMaterials;
                bool changed = false;
                for (int i = 0; i < materials.Length; i++)
                {
                    var source = materials[i];
                    if (source == null || AssetDatabase.Contains(source)) continue;
                    if (creados.TryGetValue(source, out var materialGuardado))
                    {
                        materials[i] = materialGuardado;
                        changed = true;
                        continue;
                    }
                    var name = SafeName(source.name);
                    var path = folder + "/" + name + ".mat";
                    var saved = AssetDatabase.LoadAssetAtPath<Material>(path);
                    if (saved == null)
                    {
                        saved = new Material(source) { name = name };
                        AssetDatabase.CreateAsset(saved, path);
                    }
                    else
                    {
                        EditorUtility.CopySerialized(source, saved);
                        saved.name = name;
                        EditorUtility.SetDirty(saved);
                    }
                    creados[source] = saved;
                    materials[i] = saved;
                    changed = true;
                }
                if (changed) renderer.sharedMaterials = materials;
            }
        }

        static void CrearPrefabsReutilizables(GameObject root)
        {
            const string folder = "Assets/Prefabs/Neighborhood";
            Directory.CreateDirectory(folder);
            AssetDatabase.Refresh();
            var hijos = new Transform[root.transform.childCount];
            for (int i = 0; i < hijos.Length; i++) hijos[i] = root.transform.GetChild(i);
            foreach (var child in hijos)
            {
                GuardarPrefab(child, folder);
                foreach (Transform descendant in child.GetComponentsInChildren<Transform>(true))
                {
                    if (descendant == child) continue;
                    string n = descendant.name;
                    if (n == "Casa" || n == "Arbol" || n == "Farola" || n == "Banca" || n.StartsWith("NPC_") || n.StartsWith("Caneca") || n.StartsWith("Residuo_") || n == "ParqueSucio" || n == "ParqueLimpio")
                        GuardarPrefab(descendant, folder);
                }
            }
        }

        static void GuardarPrefab(Transform item, string folder)
        {
            string path = folder + "/" + SafeName(item.name) + "_" + item.GetSiblingIndex() + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(item.gameObject, path);
        }

        static string SafeName(string value)
        {
            foreach (char invalid in Path.GetInvalidFileNameChars()) value = value.Replace(invalid, '_');
            return value.Replace('/', '_').Replace('\\', '_').Trim();
        }
    }
}
