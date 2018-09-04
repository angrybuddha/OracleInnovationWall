using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.IO;

public class StartupSettings : MonoBehaviour {
    #region Properties/Constructor

    [SerializeField]
    KeyCode m_quitKey = KeyCode.Escape;

    [SerializeField, Header("Default should be 24...")]
    float m_editorBezelWidth = 24f;

    //Used for overriding the window settings
    [SerializeField, Header("Display Settings: ")]
    private bool window_override = false;
    public bool Window_override {
        get { return window_override; }
        set { window_override = value; }
    }

    //Used for putting in fullscreen
    [SerializeField]
    private bool window_fullscreen = true;
    public bool Window_fullscreen {
        get { return window_fullscreen; }
        set { window_fullscreen = value; }
    }

    //Used for setting the width of the window in windowed mode 
    [SerializeField]
    private int window_x = 4320;
    public int Window_x {
        get { return window_x; }
        set { window_x = value; }
    }

    //Used for setting the height of the window in windowed mode 
    [SerializeField]
    private int window_y = 1920;
    public int Window_y {
        get { return window_y; }
        set { window_y = value; }
    }

    //Resource location
    [SerializeField, Header("Resources Info: ")]
    private string resource_location = "%USERPROFILE%/Documents/downstream/oracle/";
    public string Resource_location {
        get { return resource_location; }
        set { resource_location = value; }
    }

    //Media resource location
    [SerializeField]
    private string resource_media = "oracle/";
    public string Resource_media {
        get { return resource_media; }
        set { resource_media = value; }
    }

    //DB resource location
    [SerializeField]
    private string resource_db = "db/oracle.sqlite";
    public string Resource_db {
        get { return resource_db; }
        set { resource_db = value; }
    }

    [SerializeField, Header("App Settings:")]
    float m_attractTimeout = 10f;
    public float AttractTimeout {
        get { return m_attractTimeout; }
        set { m_attractTimeout = value; }
    }

    [SerializeField]
    float m_afterPollATimeout = 5f;
    public float AfterPollATimeout {
        get { return m_afterPollATimeout; }
        set { m_afterPollATimeout = value; }
    }

    [SerializeField]
    float m_afterPollBTimeout = 8f;
    public float AfterPollBTimeout {
        get { return m_afterPollBTimeout; }
        set { m_afterPollBTimeout = value; }
    }

    [SerializeField]
    int m_pollPrepareCountdown = 5;
    public int PollPrepareCountdown {
        get { return m_pollPrepareCountdown; }
    }

    [SerializeField]
    int m_pollCountdown = 10;
    public int PollCountdown {
        get { return m_pollCountdown; }
    }

    [SerializeField]
    int m_minTakeawayTweets = 5;
    public int MinTakeawayTweets {
        get { return m_minTakeawayTweets; }
    }

    [SerializeField]
    float m_screenPercentX = .5f;
    public float ScreenPercentX {
        get { return m_screenPercentX; }
    }

    //In military time...
    [SerializeField]
    string m_appRefreshTimeStr = "0300";
    public string AppRefreshTimeStr {
        get { return m_appRefreshTimeStr; }
    }

    bool m_showDebugLogging = false;
    public bool ShowDebugLogging {
        get { return m_showDebugLogging; }
    }

    static StartupSettings m_instance = null;
    public static StartupSettings Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<StartupSettings>();
            }
            return m_instance;
        }
    }

    #endregion

    #region Unity Functions

    //On awake
    void Awake() {
        LoadUserSettings();
    }

    void Update() {
        if (Input.GetKeyDown(m_quitKey)) {
            Application.Quit();
        }
    }

    #endregion

    #region Functions

    //Used for loading the settings from the configfile.xml
    public void LoadUserSettings() {
        string url = GetURL("ConfigFile.xml");
        if (File.Exists(url)) {
            UserData userDataFile = new UserData();
            userDataFile = userDataFile.OpenUserDataFile(url);

            //App Settings...
            m_attractTimeout = userDataFile.appSettings.attract_timeout;
            m_afterPollATimeout = userDataFile.appSettings.poll_a_timeout;
            m_afterPollBTimeout = userDataFile.appSettings.poll_b_timeout;
            m_appRefreshTimeStr = userDataFile.appSettings.app_refresh_time;
            m_pollPrepareCountdown = userDataFile.appSettings.poll_prepare_countdown;
            m_pollCountdown = userDataFile.appSettings.poll_countdown;
            m_minTakeawayTweets = userDataFile.appSettings.min_takeaway_tweets;
            Bezel.Width = userDataFile.appSettings.bezel_width;

            for (int i = 0, count = CubeSpawner.Instance.CubeOffsets.Length; i < count; ++i) {
                CubeSpawner.Instance.CubeOffsets[i] =
                    userDataFile.appSettings.cube_spawn_offsets[i];
            }

            //Debug Settings...
            Cursor.visible = userDataFile.appDebugSettings.debug_show_cursor;
            m_showDebugLogging = userDataFile.appDebugSettings.debug_show_logging;
            Bezel.Show = userDataFile.appDebugSettings.debug_show_bezels;

            List<string> strOfColors = userDataFile.appSettings.player_color_by_order;
            var playerColors = PlayerManager.Instance.PlayerColorList;
            for (int i = 0, firstCount = playerColors.Count, secCount =
                strOfColors.Count; i < firstCount && i < secCount; ++i) {

                Color color = new Color();
                ColorUtility.TryParseHtmlString(strOfColors[i], out color);
                playerColors[i].color = color;
            }

            //Display
            m_screenPercentX = userDataFile.display.scrn_x_perc;
            Window_override = userDataFile.display.window_override;
            Window_fullscreen = userDataFile.display.window_fullscreen;
            Window_x = userDataFile.display.window_x +
                (int)(Bezel.Width * Bezel.NumBezels);
            Window_y = userDataFile.display.window_y;

            //Resource
            Resource_location = Environment.ExpandEnvironmentVariables(
                userDataFile.resourcesSettings.resource_location);
            Resource_location = Resource_location.Replace('\\', '/');
            Resource_db = userDataFile.resourcesSettings.resource_db;
            Resource_media = userDataFile.resourcesSettings.resource_media;

            //MQTT
            MQTTListener mqtt = MQTTListener.Instance;
            mqtt.ServerIP = userDataFile.mqttSettings.mqtt_server_ip;
            mqtt.MQTTTopic = userDataFile.mqttSettings.mqtt_topic;

            //HTTP Request
            HTTPRequest http = HTTPRequest.Instance;
            http.UrlRequest = userDataFile.httpRequestSettings.url_request;
        }
        else {
            Debug.LogWarning("Can't Find config file at " + url);
        }

        if (Window_override == true) {
            Screen.SetResolution(Window_x, Window_y, Window_fullscreen);
        }

        CanvasScaler[] canvasScalers = FindObjectsOfType<CanvasScaler>();
        foreach (CanvasScaler scaler in canvasScalers) {
            scaler.referenceResolution = new Vector2(Window_x, Window_y);
        }
    }

    //Gets the filename url whether on a mac or pc
    public string GetURL(string fileName) {
        string path = Application.dataPath;
        if (Application.platform == RuntimePlatform.OSXPlayer) {
            path += "/../../";
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer) {
            path += "/../";
        }

        return String.Format("{0}/{1}", path, fileName);
    }

    #endregion
}