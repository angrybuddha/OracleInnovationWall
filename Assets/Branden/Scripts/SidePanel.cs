using UnityEngine;
using System.Collections;

public class SidePanel : MonoBehaviour {
    [SerializeField]
    TakePoll m_leftTakePoll = null;

    [SerializeField]
    TakePoll m_centerTakePoll = null;

    [SerializeField]
    TakePoll m_rightTakePoll = null;

    [SerializeField]
    float m_cubeWaitTime = 1f;

    [SerializeField]
    float m_exploadWaitTime = 1f;

    [SerializeField]
    float m_exploadRunTime = 2f;

    [SerializeField]
    float m_pollWaitTime = 2f;

    float m_timeoutTimer = 0f;

    Animator m_animator = null;

    static SidePanel m_instance = null;
    public static SidePanel Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<SidePanel>();
            }
            return m_instance;
        }
    }

    void Awake() {
        PanelManager.Instance.SubscribeToActivePanel(0, OnActivateLeftPanel);
        PanelManager.Instance.SubscribeToActivePanel(1, OnActivateCenterPanel);
        PanelManager.Instance.SubscribeToActivePanel(2, OnActivateRightPanel);
        PanelManager.Instance.SubscribeToActivePanel(3, OnActivateSidePanel);
        LargeCubeCluster.Instance.CreateCluster();
        SmallCubeCluster.Instance.CreateCluster();
        EqualCubeCluster.Instance.CreateCluster();
    }

    void Update() {
        UpdateTimeout();
    }

    void UpdateTimeout() {
        if (AppManager.State == AppManager.AppState.CAN_TAKE_POLL) {
            if (PlayerManager.Instance.ActivePlayers.Count == 0) {
                m_timeoutTimer += Time.deltaTime;
                if (m_timeoutTimer > StartupSettings.Instance.AttractTimeout) {
                    AppManager.State = AppManager.AppState.HIDE_CONVERSATION;
                    StartCoroutine(RunHideSideMenu());
                }
                else {
                    return;
                }
            }
        }
        else if (AppManager.State == AppManager.AppState.STREAMING_CUBES) {
            if (PlayerManager.Instance.ActivePlayers.Count == 0) {
                AppManager.State = AppManager.AppState.ATTRACT_CUBES;
            }
        }

        m_timeoutTimer = 0f;
    }

    public void OnActivateSidePanel(bool panelIsActive) {
        ActivatePoll();
    }

    public void OnActivateLeftPanel(bool panelIsActive) {
        if (AppManager.State == AppManager.AppState.CAN_TAKE_POLL) {
            m_leftTakePoll.Show(panelIsActive);
        }
        else {
            if (AppManager.State == AppManager.AppState.ATTRACT_CUBES) {
                AppManager.State = AppManager.AppState.STREAMING_CUBES;
            }

            ShowSideMenu();
        }
    }

    public void OnActivateCenterPanel(bool panelIsActive) {
        if (AppManager.State == AppManager.AppState.CAN_TAKE_POLL) {
            m_centerTakePoll.Show(panelIsActive);
        }
        else {
            if (AppManager.State == AppManager.AppState.ATTRACT_CUBES) {
                AppManager.State = AppManager.AppState.STREAMING_CUBES;
            }

            ShowSideMenu();
        }
    }

    public void OnActivateRightPanel(bool panelIsActive) {
        if (AppManager.State == AppManager.AppState.CAN_TAKE_POLL) {
            m_rightTakePoll.Show(panelIsActive);
        }
        else {
            if (AppManager.State == AppManager.AppState.ATTRACT_CUBES) {
                AppManager.State = AppManager.AppState.STREAMING_CUBES;
            }

            ShowSideMenu();
        }
    }

    public void ShowSideMenu() {
        ContentManager content = ContentManager.Instance;
        bool canRunPoll = content.PollAList.Count > 0 ||
            content.PollBList.Count > 0;

        if (canRunPoll) {
            if (AppManager.State == AppManager.AppState.STREAMING_CUBES) {
                AppManager.State = AppManager.AppState.JOIN_CONVERSATION;
                StartCoroutine(RunShowSideMenu());
            }
        }
    }

    [ContextMenu("Simulate ActivatePoll...")]
    public void ActivatePoll() {
        if (AppManager.State == AppManager.AppState.CAN_TAKE_POLL) {
            AppManager.State = AppManager.AppState.STARTING_POLL;
            Player.ShowPointPoll = false;
            m_leftTakePoll.Show(false);
            m_centerTakePoll.Show(false);
            m_rightTakePoll.Show(false);

            StartCoroutine(StartPoll());
        }
    }

    IEnumerator StartPoll() {
        SideMenu sideMenu = SideMenu.Instance;
        LargeCubeCluster cubeCluster = LargeCubeCluster.Instance;
        float pollCountdown = StartupSettings.Instance.PollPrepareCountdown;

        float timer = pollCountdown;

        //Hide "Join Conversation text"...
        sideMenu.ShowJoinConversation(false);
        sideMenu.ShowTakePoll(false);
        CubeCluster.HidePlayerCubes = true;

        Player.ShowHashtag = false;
        Player.ShowMoveIcon = false;
        Player.ShowColor = true;
        Player.ShowInactiveColor = false;

        //Explode cube...
        yield return new WaitForSeconds(m_exploadWaitTime);
        Player.ShowTakePollText = true;
        cubeCluster.Explode();

        //Show "Let's Get Started" text...
        sideMenu.ShowGetStarted(true);

        //Show countdown cube...
        yield return new WaitForSeconds(m_exploadRunTime);
        sideMenu.ShowCameraView(false); //Fading out cube cluster

        yield return new WaitForSeconds(m_pollWaitTime);
        cubeCluster.DestroyCluster();
        cubeCluster.CreateCluster();

        Player.CountdownValue = (int)pollCountdown;
        Player.ShowCountdownValue = true;
        StartCoroutine(Player.RunCountdown(null));

        yield return new WaitForSeconds(pollCountdown + 1f);
        PollManager.Instance.StartNextPoll();
    }

    IEnumerator RunShowSideMenu() {
        SideMenu sideMenu = SideMenu.Instance;
        LargeCubeCluster cubeCluster = LargeCubeCluster.Instance;

        sideMenu.ShowBackground(true);  //Fades in background...

        //Wait sometime after background fades in...
        yield return new WaitForSeconds(1.5f);
        sideMenu.ShowJoinConversation(true);
        CubeCluster.HidePlayerCubes = false;    //Show player cubes, in cube cluster...
        sideMenu.ShowCameraView(true);  //Fades in cube cluster view...

        yield return new WaitForSeconds(m_cubeWaitTime);
        cubeCluster.ShowCluster(true);    //Slides in cube cluster...

        yield return new WaitForSeconds(1f);
        sideMenu.ShowTakePoll(true);

        yield return new WaitForSeconds(3f);

        Player.ShowPointPoll = true;
        AppManager.State = AppManager.AppState.CAN_TAKE_POLL;
        PanelManager.Instance.ClearAllPanels();
    }

    IEnumerator RunHideSideMenu() {
        SideMenu sideMenu = SideMenu.Instance;
        LargeCubeCluster cubeCluster = LargeCubeCluster.Instance;

        sideMenu.ShowTakePoll(false);
        yield return new WaitForSeconds(1f);

        cubeCluster.ShowCluster(false);
        yield return new WaitForSeconds(m_cubeWaitTime);

        sideMenu.ShowCameraView(false);
        CubeCluster.HidePlayerCubes = true;
        sideMenu.ShowJoinConversation(false);
        sideMenu.ShowBackground(false);  //Fade out background...

        yield return new WaitForSeconds(1f);
        PollManager.Instance.ResetTwitter();

        AppManager.State = AppManager.AppState.ATTRACT_CUBES;
    }
}
