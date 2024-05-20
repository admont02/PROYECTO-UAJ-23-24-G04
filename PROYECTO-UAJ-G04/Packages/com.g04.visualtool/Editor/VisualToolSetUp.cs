using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
[InitializeOnLoad]
public class VisualToolSetUp
{
    private const string ConfigFilePath = "Packages/com.g04.visualtool/Editor/VisualToolSetUp.json";
    private static bool executed=false;
    static VisualToolSetUp()
    {
        //Suscribe el metodo Initialize al evento update de EditorApplication
        EditorApplication.update += Initialize;
    }
    private static void Initialize(){

        if (!EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isCompiling)
        {
            LoadConfig();

            if (!executed)
            {

                SetRenderPipeline();
                System.Threading.Thread.Sleep(5000);
                FixGlobalSettings();
                //PENDIENTE DE RESPUESTA DE GUILLE Y PILAR
                // PipeLineConverter();
                executed=true;
                SaveConfig();
            }
            //Elimina el metodo Initialize de la susvripción al evento update de EditorApplication
            EditorApplication.update -= Initialize;

        }
    }
    /// <summary>
    /// Cambia el RenderPipeline automáticamente
    /// </summary>
    private static void SetRenderPipeline()
    {
        //Obtiene la ruta
        string packagePath = "Packages/com.g04.visualtool/Editor/Render/NewUniversalRenderPipelineAsset.asset";
        RenderPipelineAsset pipelineAsset = AssetDatabase.LoadAssetAtPath<RenderPipelineAsset>(packagePath);

        if (pipelineAsset == null)
        {
            Debug.LogError("No se encuentra el Render Pipeline Asset");
            return;
        }
        GraphicsSettings.renderPipelineAsset = pipelineAsset;

    }
    /// <summary>
    /// Abre la ventana de ProjectSettings Grapchics URP GlobalSettings
    /// </summary>
    [MenuItem("Custom/Open Graphics Settings")]
    private static void FixGlobalSettings()
    {
        SettingsService.OpenProjectSettings("Project/Graphics/URP Global Settings");
        System.Threading.Thread.Sleep(1000);
    }
    /// <summary>
    /// Carga el archivo de configuración
    /// </summary>
    private static void LoadConfig() {
        if (File.Exists(ConfigFilePath)) {
            string json = File.ReadAllText(ConfigFilePath);
            executed = JsonUtility.FromJson<ConfigData>(json).Executed;
        } else {
            Debug.LogError("No se encuentra el archivo de configuración");
        }
    }
        private static void SaveConfig() {
        ConfigData data = new ConfigData { Executed = executed };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(ConfigFilePath, json);
    }

    //PENDIENTE DE RESPUESTA DE GUILLE Y PILAR
    // [MenuItem("Window/Rendering/Render Pipeline Converter")]
    //private static void PipeLineConverter()
    //{
    // 
    //    EditorApplication.ExecuteMenuItem("Window/Rendering/Render Pipeline Converter");

    //    // Espera un tiempo para que la ventana se abra completamente y se cargue el contenido
    //    //System.Threading.Thread.Sleep(1000);
    
    //    //Intentar seleccionar las opciones y que pulse el boton automaticamente

    //    //// Espera un tiempo para que se complete el proceso de conversi�n
    //    //System.Threading.Thread.Sleep(5000); // Ajusta el tiempo seg�n sea
    //}
private struct ConfigData
{
    public bool Executed;
}
}
#endif 