/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   GUI mask for Settings.
*******************************************************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Settings))]
public class Settings_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("XML File Editor Options: ", EditorStyles.boldLabel);

        GUILayout.BeginVertical();

        EditorGUILayout.Space();

        if (GUILayout.Button(@"Create XML File", GUILayout.Height(25)))
        {
            UserData userData = new UserData();

            //Display
            userData.display.window_override = Core.Instance._settings.Window_override;
            userData.display.window_fullscreen = Core.Instance._settings.Window_fullscreen;
            userData.display.window_x = Core.Instance._settings.Window_x;
            userData.display.window_y = Core.Instance._settings.Window_y;

            //Resources
            userData.resourcesSettings.resource_location = Core.Instance._settings.Resource_location;
            userData.resourcesSettings.resource_db = Core.Instance._settings.Resource_db;
            userData.resourcesSettings.resource_media = Core.Instance._settings.Resource_media;

            //MQTT Settings
            userData.mqttSettings.mqtt_server_ip = Core.Instance._mqttListener.ServerIP;
            userData.mqttSettings.mqtt_topic = Core.Instance._mqttListener.MQTTTopic;

            //HTTP Request
            userData.httpRequestSettings.url_request = Core.Instance._httpRequest.UrlRequest;

            //Kinect Settings
            //userData.kinectSettings.kinect_area_x = Core.Instance._settings.Kinect_area_X;
            //userData.kinectSettings.kinect_area_y = Core.Instance._settings.Kinect_area_y;

            //QualitySettings
            //userData.qualitySettings.refResolution_x = Core.Instance._settings.Qualitysettings_referenceresolution_x;
            //userData.qualitySettings.refResolution_y = Core.Instance._settings.Qualitysettings_referenceresolution_y;

            userData.WriteUserDataFile(@"C:\temp\ConfigFile.xml", userData);

            Debug.Log(@"File Created at c:\temp\ConfigFile.xml");
        }

        if (GUILayout.Button(@"Open XML Location", GUILayout.Height(25)))
        {
            Application.OpenURL("file://c:/temp/");

            Debug.Log(@"Open c:\temp\ folder");
        }

        GUILayout.EndHorizontal();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}