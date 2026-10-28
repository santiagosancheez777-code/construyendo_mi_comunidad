using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ConstruyendoMiComunidad.Editor
{
    /// <summary>
    /// Prepara Build Settings y ofrece un menú para reconstruir el barrio en la escena.
    /// El juego también se genera solo al pulsar Play, gracias a GameInstaller.
    /// </summary>
    [InitializeOnLoad]
    public static class GameSceneSetup
    {
        static GameSceneSetup()
        {
            EditorApplication.delayCall += EnsureBuildSettings;
        }

        [MenuItem("Construyendo mi comunidad/Preparar proyecto (Build Settings)")]
        public static void EnsureBuildSettings()
        {
            var escena = "Assets/Scenes/SampleScene.unity";
            var escenas = new[] { new EditorBuildSettingsScene(escena, true) };
            EditorBuildSettings.scenes = escenas;

            PlayerSettings.companyName = "ConstruyendoMiComunidad";
            PlayerSettings.productName = "Construyendo mi comunidad";
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        }

        [MenuItem("Construyendo mi comunidad/Generar barrio en la escena abierta")]
        public static void GenerarBarrio()
        {
            var existente = GameObject.Find("NeighborhoodRoot");
            if (existente != null)
                Object.DestroyImmediate(existente);

            NeighborhoodBuilder.Build();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("Barrio generado. Pulsa Play para jugar. El menú, el jugador y la UI se crean al iniciar.");
        }
    }
}
