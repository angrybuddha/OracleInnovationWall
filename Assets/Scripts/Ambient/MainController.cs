/*******************************************************************************************
* Author: Jed Bursiek
* Created Date: 07-01-16 
* 
* Description: 
*   Used for managing the entire application
*******************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public delegate void CubeEvent(int _id, string _instruct);
public delegate void PlayerEvent(int _id, float _x, float _y);
public delegate void ActivePlayerEvent(int id, bool active);
public delegate void ChangeSectionEvent(string section);
public delegate void TwitterEvent(string action);

public class MainController : MonoBehaviour
{

    /* REGIONS */
    float quad_w = Screen.width / 8;

    float quad_h = Screen.height;

    Rect myRect;

    /*ACTIVE REGIONS VARS*/
    static float[] screen_quadrant;
    static float[] camera_quadrant;
    public static bool[] region_occupied;
    int quotient = 8;


    /*SECTIONS*/
    [SerializeField, Header("Sections:")]

    public GameObject activatePoll;
    public GameObject pollTypeA;
    public GameObject pollTypeB;
    public GameObject pollTakeaway;
    public GameObject twitterCubes;
    public GameObject screeners;
    public GameObject floaters;
    public GameObject background;

    /*DATA SCRIPTS*/
    [SerializeField, Header("Data:")]

    public MQTTListener _MQTTData;

    [SerializeField, Header("Players:")]

    public GameObject player_controller;

    /*SECTIONS*/
    [SerializeField, Header("UI:")]
    public TextMeshProUGUI Loader;
    public GameObject LoaderBG;

    /*EVENTS*/
    public static event PlayerEvent MovePlayer;
    public static event CubeEvent UpdatePlayer;
    public static event ActivePlayerEvent ActivatePlayer;
    public static event TwitterEvent TwitterAction;
    public event ChangeSectionEvent ChangeSection;

    bool ambient_active = false;
    bool takeaway_active = false;

    private bool[] active_players;

    private int current_player_count = 0;

    private bool update_player_grp = false;

    public bool do_player_update = true;

    public string current_question_id;

    private bool ALLOW_NEW_PLAYERS = true;

    private int MAX_PLAYERS = 10;

    public List<int> active_player_list;
    public List<float> scatter_map_list = new List<float>();
    public int SCATTER_MAX = 40;
    public int POLL_POINTER = 0;

    public Camera camera_cube;

    //SET TIMER VARIABLES HERE (EXTERNAL SETTINGS?)

    public int TIME_TO_ACTIVATE_POLL = 20;
    public int TIME_TO_NEXTPOLL = 30;
    private int TIME_TO_RESET = 100;

    //int UPDATE_DELAY_COUNTER = 0;

    //int UPDATE_DELAY_MAX = 5;

    /*TWITTER FEED*/
    [System.NonSerialized]
    public List<TweetSearchTwitterData> TweetsList;
    public List<TweetSearchTwitterData> TweetsTakeaway;


    private static MainController _instance;

    public static MainController Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<MainController>();

            return _instance;
        }
    }


    public void Start()
    {
        //Loader.text = "STARTING UP";
        //soloRedCube();

        //build the active regions for the display wall (in 1/8ths)

        region_occupied = new bool[8];

        screen_quadrant = new float[8];

        camera_quadrant = new float[8];

        active_players = new bool[10];

        //

        float camera_quotient = Core.Instance._settings.Kinect_area_X * Core.Instance._settings.Active_region_scalar;

        for (var i = 0; i < quotient; i++)
        {

            screen_quadrant[i] = Screen.width * i / quotient;

            camera_quadrant[i] = camera_quotient*(i+1)/quotient - camera_quotient / 2;
                //((Screen.width / 200.0f) * (i + 1) / quotient) - ((Screen.width / 200.0f) / 2);

            //camera_cube.WorldToScreenPoint(CubePlayers[_id - 1].GetComponent<PlayerCube>().transform.position));
            region_occupied[i] = false;

        }

        //BUILD SCATTER GRAPH PLOT POINTS

        for (float px = 0; px < SCATTER_MAX; ++px)
        {

            scatter_map_list.Add((px - 20) / 3);

        }

    }

    public void StartTwitterCubes(string search)
    {

        Loader.text = "CONNECTING TO TWITTER...";

        TwitterAPI.instance.SearchTwitter(100, search, SearchTweetsResultsCallBack);

    }


    void SearchTweetsResultsCallBack(List<TweetSearchTwitterData> tweetList)
    {

        Loader.text = "BUILDING AMBIENT CUBES";

        //SET THE MASTER TWEETS LIST
        TweetsList = tweetList;

        buildBackground();

        //Got to the ambient section
        ambientSection();

        //Start listening for players
        do_player_update = true;

    }

    public void buildBackground() {
        string bgPath = Core.Instance._cms.background_path;
        if (string.IsNullOrEmpty(bgPath)) {
            Debug.LogError("Note: Background Path is Null. Will NOT load background!");
        }
        else {
            DynamicBG dynamicBg = background.GetComponent<DynamicBG>();
            dynamicBg.url = bgPath;
            dynamicBg.Apply();
        }
    }

    public void updateScreenersForTakeaway(List<TweetSearchTwitterData> tweetList)
    {

        //TweetsTakeaway
        if (TwitterAction != null) TwitterAction("screeners_takeaway");

    }

    void Update()
    {

        if (do_player_update)
            updatePlayers();

    }

    /*
     * Sets the region id via the player cube
     * whenever the player cube moves (which is always)
     */
    public static int setRegion(float x)
    {
        //float quad_x = 0;
        int reg_id = 0;

        if (x <= camera_quadrant[0]){ reg_id = 0; }

        else if (x > camera_quadrant[0] && x <= camera_quadrant[1]){ reg_id = 1; }

        else if (x > camera_quadrant[1] && x <= camera_quadrant[2]){ reg_id = 2; }

        else if (x > camera_quadrant[2] && x <= camera_quadrant[3]){ reg_id = 3; }

        else if (x > camera_quadrant[3] && x <= camera_quadrant[4]){ reg_id = 4; }

        else if (x > camera_quadrant[4] && x <= camera_quadrant[5]){ reg_id = 5; }

        else if (x > camera_quadrant[5] && x <= camera_quadrant[6]){ reg_id = 6; }

        else if (x > camera_quadrant[6] && x <= camera_quadrant[7]){ reg_id = 7; }

        return reg_id;
    }


    public void setPlayerReg()
    {

        foreach (int id in active_player_list)

        {

            //if (MovePlayer != null) MovePlayer(id, 8.5F, 0.0F);//, -4.0

        }

    }

    public void AfterTakeaway()
    {
        StartCoroutine(getPoll());
        
    }

    IEnumerator getPoll()
    {

        //then explode cubes
        yield return new WaitForSeconds(MainController.Instance.TIME_TO_NEXTPOLL);
        MainController.Instance.NextSection(false);

    }

    public void getAmbient()
    {

        StopAllCoroutines();

        //then explode cubes
        //yield return new WaitForSeconds(8);
        ambientSection();

    }

    public void NextSection(bool floats_hidden)
    {
        


        if (POLL_POINTER >= Core.Instance._cms.PollList.Count)
        {
            POLL_POINTER = 0;
        }

        //Debug.Log("Q Type + " + Core.Instance._cms.PollList[POLL_POINTER].QuestionType);

        if(Core.Instance._cms.PollList[POLL_POINTER].QuestionType == "1")
        {
            pollTypeASection();
        }
        else
        {
            pollTypeBSection();
        }

        POLL_POINTER++;

        pollTakeaway.SetActive(false);
        twitterCubes.SetActive(false);

        takeaway_active = false;

        if(!floats_hidden)
            hideFloaters();

    }

    IEnumerator ResetToAmbient()
    {

        Debug.Log("STARTING UP COUNTDOWN TIMER");

        yield return new WaitForSeconds(TIME_TO_RESET);

        //ambientSection();

    }

    public void goAmbient()
    {
        string search_me = Core.Instance._cms.RequestTwitterRecords("all");

        StartTwitterCubes(search_me);
    }
    
    //MAKE EACH SECTION NON-LINEARLY ACTIVE
    public void ambientSection()
    {
        //Hides loading background image...
        LoaderBG.SetActive(false);

        //NOW BUILD CUBES WITH TWEETS --
        /*
        The first time this will build all - floaters, cube_regions, and screeners
        After that this will only rebuild the screeners
        */
        if (TwitterAction != null) TwitterAction("build");

        takeaway_active = false;
        ambient_active = true;
        
        if(!Core.Instance._settings.Visual_cue)
            PlayerController.Instance.hideAllCubes();

        ALLOW_NEW_PLAYERS = true;

        activatePoll.SetActive(false);
        pollTypeA.SetActive(false);
        pollTypeB.SetActive(false);
        pollTakeaway.SetActive(false);

        twitterCubes.SetActive(true);

        if(TwitterAction != null) TwitterAction("reset_region");

        floaters.SetActive(true);


    }


    public void promptPollSection()
    {
        /* No Cube elements should be visible
         * 
         */

        ambient_active = false;

        ALLOW_NEW_PLAYERS = true;

        if (!activatePoll.activeSelf) { 

            //Poll PROMPT
            activatePoll.SetActive(true);
            activatePoll.GetComponent<PollActivate>().init();
            //activatePoll.GetComponent<PollActivate>().initCrystal();

            //HIDE THE OTHERS
            pollTypeA.SetActive(false);
            pollTypeB.SetActive(false);
            pollTakeaway.SetActive(false);

            twitterCubes.SetActive(true);
            
        }
    }

    public void submitVoteType01()
    {


        /* No Cube elements should be visible
         * 
         */
        
        foreach (int id in active_player_list)
        {

            if (UpdatePlayer != null) UpdatePlayer(id, "scale_down");
            if (UpdatePlayer != null) UpdatePlayer(id, "hideAll");

        }

        
        //if (UpdatePlayer != null) UpdatePlayer(1, "scale_down");
    }

    public void submitVoteType02()
    {

        /* No Cube elements should be visible
         * 
         */

        foreach (int id in active_player_list)
        {

            if (UpdatePlayer != null) UpdatePlayer(id, "snap_B_col");
        //PlayerController.Instance.sweepValues();

        }
    }

    public void pollTypeASection()
    {
        /*  no arrow - Show person mover and countdown
         * 
         */
        ALLOW_NEW_PLAYERS = false;
        //Poll TYPE A
        activatePoll.SetActive(false);

        

        foreach(int id in active_player_list)
        {
            if (UpdatePlayer != null) UpdatePlayer(id, "typeA");
        }

        pollTypeA.SetActive(true);
        pollTypeA.GetComponent<PollTypeA>().init();
        //if (Build != null) Build(100, 50);

        pollTypeB.SetActive(false);
        

    }

    public void hideFloaters()
    {
        //LOL
        floaters.GetComponent<Floaters>().exit();

        if (TwitterAction != null) TwitterAction("toggle_off");

        screeners.SetActive(false);

        twitterCubes.SetActive(false);

    }

    public void showFloaters()
    {
        screeners.SetActive(true);

        floaters.GetComponent<Floaters>().init();

        if (TwitterAction != null) TwitterAction("toggle_on");

        
    }

    public void pollTypeBSection()
    {
        /* No person mover - countdown and arrow
         * 
         */
        ALLOW_NEW_PLAYERS = false;
        //Poll TYPE B
        activatePoll.SetActive(false);
        pollTypeA.SetActive(false);

        foreach (int id in active_player_list)
        {
            if (UpdatePlayer != null) UpdatePlayer(id, "typeB");
        }

        pollTypeB.SetActive(true);
        pollTypeB.GetComponent<PollTypeB>().init();

        
    }

    public void pollTakeawaySection()
    {

        /* No cube elements
         * 
         */
        showFloaters();

        takeaway_active = true;
        
        ALLOW_NEW_PLAYERS = true;

        foreach (int id in active_player_list)
        {
            if (UpdatePlayer != null) UpdatePlayer(id, "scale_up");
            if (UpdatePlayer != null) UpdatePlayer(id, "hideAll");
        }

        do_player_update = true;

        //Takeaway
        activatePoll.SetActive(false);
        pollTypeA.SetActive(false);
        pollTypeB.SetActive(false);

        pollTakeaway.SetActive(true);
        pollTakeaway.GetComponent<PollTakeaway>().buildTakeaway();

        twitterCubes.SetActive(false);
        
    }

    private void updatePlayers()
    {

        //Debug.Log("UPDATE PLAYER " + do_player_update);

        //CHECK TO SEE IF ANY NEW PLAYERS WE'RE ADDED TO THE AREA
        if (_MQTTData.RawPlayers.Count != current_player_count && ALLOW_NEW_PLAYERS && _MQTTData.RawPlayers.Count < MAX_PLAYERS)
        {

            Debug.Log("NEW PLAYER COUNT : " + _MQTTData.RawPlayers.Count + " OLD PLAYER COUNT : " + current_player_count);
            //IF SO THEN SET NEW PLAYER COUNT FOR CHECK
            current_player_count = _MQTTData.RawPlayers.Count;

            if(current_player_count == 0)
            {
                //dispatch to the floaters to start up
                //if (TwitterAction != null) TwitterAction("toggle_on");

                if (!ambient_active)
                    getAmbient();

            }
            else
            {
                //dispatch to the floaters to stop
                //if (TwitterAction != null) TwitterAction("toggle_off");
                if(!takeaway_active)
                    screeners.SetActive(false);

                //if (!ambient_active)
                //StopCoroutine(ResetToAmbient());
            }

            active_players = new bool[10];

            update_player_grp = true;

            //reset the player list
            active_player_list = new List<int>();

        }


        foreach (var player in _MQTTData.RawPlayers)
        {

            //Debug.Log("PLAYER " + player.ToString());

            int id = int.Parse(player.Id);// as int;

            //SETS ONLY THE ACTIVE PLAYER TO TRUE
            if (update_player_grp) {
                active_players[id - 1] = true;
                active_player_list.Add(id);
            }

            //TRANSLATE DATA FROM THE MQQT TO THE SCREEN/CAMERA SPACE

            //PREVENTS JITTER BY ROUNDING (TO 1 DECIMAL PLACE FOR MORE PRECISION)

            float x = (float)System.Math.Round((player.X - .5f) * Core.Instance._settings.Kinect_area_X, 1);
            //float x = (player.X - .5f) * Core.Instance._settings.Kinect_area_X;

            float y = 0.0F;//((player.Y - .5f) * Core.Instance._settings.Kinect_area_y);

            //DISPATCH THE NEW LOCATION AND ID FOR EACH CUBE
            //PLAYER CONTROLLER THEN HANDLES THE LISTENING FOR THIS EVENT
            if (MovePlayer != null) MovePlayer(id, x, y);

        }


    }

    void LateUpdate()
    {
        //HIDE-SHOW PLAYERS
        if (update_player_grp)
        {
            //SET ALL THE PLAYERS TO THEIR ACTIVE STATE
            for (var i = 0; i < 10; i++)
            {
                //DISPATCHES THE EVENT TO THE PLAYER CONTROLLER 
                if (ActivatePlayer != null) ActivatePlayer(i, active_players[i]);

            }

            //STOP UPDATING PLAYERS GROUP
            update_player_grp = false;
        }
    }

    void OnGUI()
    {

        //Debug.Log(Core.Instance._settings.Visual_cue);
        if (Core.Instance._settings.Visual_cue) {

            GUIStyle style = new GUIStyle();

            for (var i=0; i<8; i++)
            {
                if (region_occupied[i])
                {
                
                    Rect rect = new Rect(screen_quadrant[i], 0, quad_w, quad_h);
                    GUI.Box(rect, "");

                }
            
            }
        }
    }
}
