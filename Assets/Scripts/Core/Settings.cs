/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used for project settings.
*******************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;

public class Settings : MonoBehaviour
{
    #region Properties/Constructor

    //Used for overriding the window settings
    [SerializeField, Header("Display Settings: ")]
    private bool window_override = false;
    public bool Window_override
    {
        get { return window_override; }
        set { window_override = value; }
    }

    //Used for putting in fullscreen
    [SerializeField]
    private bool window_fullscreen = true;
    public bool Window_fullscreen
    {
        get { return window_fullscreen; }
        set { window_fullscreen = value; }
    }

    //Used for setting the width of the window in windowed mode 
    [SerializeField]
    private int window_x = 4320;
    public int Window_x
    {
        get { return window_x; }
        set { window_x = value; }
    }

    //Used for setting the height of the window in windowed mode 
    [SerializeField]
    private int window_y = 1920;
    public int Window_y
    {
        get { return window_y; }
        set { window_y = value; }
    }

    //Resource location
    [SerializeField, Header("Resources Info: ")]
    private string resource_location = string.Empty;
    public string Resource_location
    {
        get { return resource_location; }
        set { resource_location = value; }
    }

    //Media resource location
    [SerializeField]
    private string resource_media = string.Empty;
    public string Resource_media
    {
        get { return resource_media; }
        set { resource_media = value; }
    }

    //DB resource location
    [SerializeField]
    private string resource_db = string.Empty;
    public string Resource_db
    {
        get { return resource_db; }
        set { resource_db = value; }
    }

    //Kinect pivot location
    [SerializeField, Header("Kinect Settings:")]
    private GameObject kinect_pivot;
    public GameObject Kinect_pivot
    {
        get { return kinect_pivot; }
        set { kinect_pivot = value; }
    }

    //Kinect area width
    [SerializeField]
    private float kinect_area_X = 5.4864f;
    public float Kinect_area_X
    {
        get { return kinect_area_X; }
        set { kinect_area_X = value; }
    }

    //Kinect area height
    [SerializeField]
    private float kinect_area_y = 2.1336f;
    public float Kinect_area_y
    {
        get { return kinect_area_y; }
        set { kinect_area_y = value; }
    }

    //Acive cube region scalar
    [SerializeField]
    private bool visual_cue = false;
    public bool Visual_cue {
        get { return visual_cue; }
        set { visual_cue = value; }
    }

    [SerializeField]
    private float active_region_scalar = 0.68f;
    public float Active_region_scalar
    {
        get { return active_region_scalar; }
        set { active_region_scalar = value; }
    }

    //Settings for scalling the GUIs width
    [SerializeField, Header("Quality Settings:")]
    private int qualitysettings_referenceresolution_x = 1920;
    public int Qualitysettings_referenceresolution_x
    {
        get { return qualitysettings_referenceresolution_x; }
        set { qualitysettings_referenceresolution_x = value; }
    }

    //Settings for scalling the GUIs height
    [SerializeField]
    private int qualitysettings_referenceresolution_y = 1080;
    public int Qualitysettings_referenceresolution_y
    {
        get { return qualitysettings_referenceresolution_y; }
        set { qualitysettings_referenceresolution_y = value; }
    }

    static Settings m_instance = null;
    public static Settings Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<Settings>();
            }
            return m_instance;
        }
    }

    #endregion

    #region Unity Functions

    //On awake
    void Awake()
    {
        LoadUserSettings();

        if(Window_override == true)
        {
            Screen.SetResolution(Window_x, Window_y, Window_fullscreen);
        }

        Core.Instance.UICanvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(Qualitysettings_referenceresolution_x, Qualitysettings_referenceresolution_y);
    }

    #endregion

    #region Functions

    //Used for loading the settings from the configfile.xml
    public void LoadUserSettings()
    {
        string url = GetURL("ConfigFile.xml");

        Core.Instance.SaveOutputLine(Core.DebugType.Log, url, true);

        if (File.Exists(url))
        {
            UserData userDataFile = new UserData();
            userDataFile = userDataFile.OpenUserDataFile(url);

            //Display
            Window_override = userDataFile.display.window_override;
            Window_fullscreen = userDataFile.display.window_fullscreen;
            Window_x = userDataFile.display.window_x;
            Window_y = userDataFile.display.window_y;

            //Resource
            Resource_location = Environment.ExpandEnvironmentVariables(userDataFile.resourcesSettings.resource_location);
            Resource_location = Resource_location.Replace('\\', '/');
            Resource_db = userDataFile.resourcesSettings.resource_db;
            Resource_media = userDataFile.resourcesSettings.resource_media;

            //MQTT
            Core.Instance._mqttListener.ServerIP = userDataFile.mqttSettings.mqtt_server_ip;
            Core.Instance._mqttListener.MQTTTopic = userDataFile.mqttSettings.mqtt_topic;

            //HTTP Request
            Core.Instance._httpRequest.UrlRequest = userDataFile.httpRequestSettings.url_request;

            //Kinect
            //Kinect_area_X = userDataFile.kinectSettings.kinect_area_x * 10;
            //Kinect_area_y = userDataFile.kinectSettings.kinect_area_y;

            Kinect_pivot.transform.localScale = new Vector3(Kinect_area_X, Kinect_pivot.transform.localScale.y, Kinect_area_y);

            //Active_region_scalar = userDataFile.kinectSettings.active_region_scalar;
            //Visual_cue = userDataFile.kinectSettings.visual_cue;

            //QualitySettings
            //Qualitysettings_referenceresolution_x = userDataFile.qualitySettings.refResolution_x;
            //Qualitysettings_referenceresolution_y = userDataFile.qualitySettings.refResolution_y;
        }
        else
        {
            Core.Instance.SaveOutputLine(Core.DebugType.Error, string.Format("Can't Find config file at '{0}'", url), true);
        }
    }

    //Gets the filename url wether on a mac or pc
    public string GetURL(string fileName)
    {
        string path = Application.dataPath;
        if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            path += "/../../";
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            path += "/../";
        }

        return String.Format("{0}/{1}", path, fileName);
    }

    #endregion
}