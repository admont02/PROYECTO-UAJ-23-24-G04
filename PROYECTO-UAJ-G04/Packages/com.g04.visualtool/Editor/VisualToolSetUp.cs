using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[InitializeOnLoad]
public class VisualToolSetUp
{
    private const string ConfigFilePath = "Packages/com.g04.visualtool/Editor/VisualToolSetUp.json";
    private static bool executed=false;
    static VisualToolSetUp()
    {
        Debug.Log("ME LLAMO PERO NO HAGO NA");
        LoadConfig();
        if (!executed)
        {
            Debug.Log("Hola me he ejecutado al cargarme");
            SetRenderPipeline();
            System.Threading.Thread.Sleep(5000);
            FixGlobalSettings();
            // PipeLineConverter();
            executed=true;
            SaveConfig();
        }

    }
    private static void SetRenderPipeline()
    {
        string packagePath = "Packages/com.g04.visualtool/Editor/Render/NewUniversalRenderPipelineAsset.asset";
        RenderPipelineAsset pipelineAsset = AssetDatabase.LoadAssetAtPath<RenderPipelineAsset>(packagePath);

        if (pipelineAsset == null)
        {
            Debug.LogError("Render Pipeline Asset not found!");
            return;
        }
        GraphicsSettings.renderPipelineAsset = pipelineAsset;
        Debug.Log("Render Pipeline changed successfully!");
    }
    [MenuItem("Custom/Open Graphics Settings")]
    private static void FixGlobalSettings()
    {
        Debug.Log("SE LLAMA");
        SettingsService.OpenProjectSettings("Project/Graphics/URP Global Settings");
        System.Threading.Thread.Sleep(1000);
    }
    private static void LoadConfig() {
        if (File.Exists(ConfigFilePath)) {
            string json = File.ReadAllText(ConfigFilePath);
            executed = JsonUtility.FromJson<ConfigData>(json).Executed;
        } else {
            executed = false;
        }
    }
        private static void SaveConfig() {
        ConfigData data = new ConfigData { Executed = executed };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(ConfigFilePath, json);
    }
    // [MenuItem("Window/Rendering/Render Pipeline Converter")]
    //private static void PipeLineConverter()
    //{
    //    // Haz clic en la opci�n "Render Pipeline Converter" dentro de "Rendering"
    //    EditorApplication.ExecuteMenuItem("Window/Rendering/Render Pipeline Converter");

    //    // Espera un tiempo para que la ventana se abra completamente y se cargue el contenido
    //    //System.Threading.Thread.Sleep(1000);

    //    // Selecciona todas las opciones
    //    // Aqu� necesitar�s implementar la l�gica para seleccionar las opciones program�ticamente
    //    //// Esto puede implicar interactuar con controles de la interfaz de usuario, como botones y listas desplegables.

    //    //// Haz clic en el bot�n Initialize and Convert
    //    //var initializeAndConvertButton = new GUIContent("Initialize and Convert");
    //    //EditorApplication.ExecuteMenuItem(initializeAndConvertButton.text);

    //    //// Espera un tiempo para que se complete el proceso de conversi�n
    //    //System.Threading.Thread.Sleep(5000); // Ajusta el tiempo seg�n sea
    //}
private struct ConfigData
{
    public bool Executed;
}
}
