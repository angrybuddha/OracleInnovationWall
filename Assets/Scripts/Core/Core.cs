/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Core data logic used for the innocation wall.
*******************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Core : MonoBehaviour
{
    #region Enums

    //Debug text message type
    public enum DebugType
    {
        Log,
        Warning,
        Error,
    }

    //Wall state
    public enum WallState
    {
        Ambient,
        Question,
    }

    #endregion

    #region Properties

    #region Singleton

    private static Core _instance;
    public static Core Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<Core>();

            return _instance;
        }
    }

    #endregion

    //Used for the current wall state of the app
    [SerializeField, Header("Core States: ")]
    private WallState state = WallState.Ambient;
    public WallState _state
    {
        get { return state; }
        set { state = value; }
    }

    //Used for the CMS
    [SerializeField, Header("Core Components:")]
    private CMS cms;
    public CMS _cms
    {
        get { return cms; }
        set { cms = value; }
    }

    //Binds the CMS to the proejct
    [SerializeField]
    private CMS_Controler cms_Controler;
    public CMS_Controler _cms_Controler
    {
        get { return cms_Controler; }
        set { cms_Controler = value; }
    }

    //Used for getting the mqtt player data
    [SerializeField]
    private MQTTListener mqttListener;
    public MQTTListener _mqttListener
    {
        get { return mqttListener; }
        set { mqttListener = value; }
    }

    //Used for saving votes by sending a http request
    [SerializeField]
    private HTTPRequest_Old httpRequest;
    public HTTPRequest_Old _httpRequest
    {
        get { return httpRequest; }
        set { httpRequest = value; }
    }

    //Used for managing all question data 
    [SerializeField]
    private QuestionManager questionManager;
    public QuestionManager _questionManager
    {
        get { return questionManager; }
        set { questionManager = value; }
    }

    //Manages all players info
    [SerializeField]
    private PlayersManager playerManager;
    public PlayersManager _playerManager
    {
        get { return playerManager; }
        set { playerManager = value; }
    }

    //Manages all spawned objects
    [SerializeField]
    private SpawnManager spawnManager;
    public SpawnManager _spawnManager
    {
        get { return spawnManager; }
        set { spawnManager = value; }
    }

    //Used for reading the xml settings file
    [SerializeField]
    private Settings settings;
    public Settings _settings
    {
        get { return settings; }
        set { settings = value; }
    }

    //Main camera prefab
    [SerializeField, Header("Core Prefabs: ")]
    private Camera cameraMain;
    public Camera CameraMain
    {
        get { return cameraMain; }
        set { cameraMain = value; }
    }

    //UI canvas prefab
    [SerializeField]
    private GameObject uiCanvas;
    public GameObject UICanvas
    {
        get { return uiCanvas; }
        set { uiCanvas = value; }
    }

    //Floor prefab
    [SerializeField]
    private GameObject floor;
    public GameObject Floor
    {
        get { return floor; }
        set { floor = value; }
    }

    //Debug panel prefab
    [SerializeField, Header("Debug Prefabs: ")]
    private GameObject panelDebug;
    public GameObject PanelDebug
    {
        get { return panelDebug; }
        set { panelDebug = value; }
    }

    //Output log text prefab
    [SerializeField]
    private TextMeshProUGUI outputLog;
    public TextMeshProUGUI OutputLog
    {
        get { return outputLog; }
        set { outputLog = value; }
    }

    //Output state text prefab
    [SerializeField]
    private TextMeshProUGUI outputState;
    public TextMeshProUGUI OutputState
    {
        get { return outputState; }
        set { outputState = value; }
    }

    //Flag used for debuging mode
    [SerializeField]
    private bool debugMode = false;
    public bool DebugMode
    {
        get { return debugMode; }
        set { debugMode = value; }
    }

    private Animator stateMachine;

    #endregion

    #region Unity Functions

    //Use this for initialization
    void Start()
    {
        //Gets the state machine animator
        //stateMachine = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //State Machine Values
        //stateMachine.SetInteger("State", (int)_state);
        //stateMachine.SetInteger("Question State", (int)_questionManager._questionState);
        //stateMachine.SetInteger("Vote State", (int)_questionManager._voteState);

        //MQTTListener
        //_mqttListener.SyncRawPlayers();

        //Debuging
        //SyncDebugOptions();
    }

    #endregion

    #region Debug Functions

    //Syncs up the input controls for debug
    public void SyncDebugOptions()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OpenDebugMenu();
        }
        else if(Input.GetKeyUp(KeyCode.F5))
        {
            RestartScene();
        }

        SyncOutputText();

        //PanelDebug.SetActive(DebugMode);
    }

    //Used for outputing to the screen
    public void SaveOutputLine(DebugType type, string line, bool OutputToScreen = false)
    {
        switch (type)
        {
            case DebugType.Log:
                line = string.Format("<b><color=#4A6C9F>LOG: {0}</color></b>", line);
                Debug.Log(line);
                break;
            case DebugType.Warning:
                line = string.Format("<b><color=yellow>WARNING: {0}</color></b>", line);
                Debug.LogWarning(line);
                break;
            case DebugType.Error:
                line = string.Format("<b><color=red>ERROR: {0}</color></b>", line);
                Debug.LogError(line);
                break;
        }

        if (OutputToScreen)
            OutputLog.text = string.Format("{0}{1}{2}", OutputLog.text, line, "\n");
    }

    //Used for outputing the diffrenct states to the screen
    public void SyncOutputText()
    {
        if (DebugMode)
        {
            if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("Ambient"))
            {
                OutputState.text = "Ambient";
            }
            else if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("Questions"))
            {
                OutputState.text = "Questions";
            }
            
            else if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("Poll"))
            {
                OutputState.text = "Poll";
            }
            else if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("Poll Results"))
            {
                OutputState.text = "Poll Results";
            }
            
            else if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("MultipleChoice"))
            {
                OutputState.text = "MultipleChoice";
            }
            else if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("MultipleChoice Results"))
            {
                OutputState.text = "MultipleChoice Results";
            }

            else if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("ScatterPlot (Part 1)"))
            {
                OutputState.text = "ScatterPlot (Part 1)";
            }
            else if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("ScatterPlot (Part 1) Results"))
            {
                OutputState.text = "ScatterPlot (Part 1) Results";
            }
            else if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("ScatterPlot (Part 2)"))
            {
                OutputState.text = "ScatterPlot (Part 2)";
            }
            else if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("ScatterPlot (Part 2) Results"))
            {
                OutputState.text = "ScatterPlot (Part 2) Results";
            }

            else if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("Fact"))
            {
                OutputState.text = "Fact";
            }
            else if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("Feed"))
            {
                OutputState.text = "Feed";
            }
        }
    }

    //Dubug event
    public void OpenDebugMenu()
    {
        DebugMode = !DebugMode;
    }

    //Restart scene event
    public void RestartScene()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    //Quit scene event
    public void QuitScene()
    {
        Application.Quit();
    }

    #endregion
}