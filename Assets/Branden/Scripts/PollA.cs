using UnityEngine;
using System.Collections;

using AOrBAnswerInfo = ContentManager.AOrBAnswerInfo;

public class PollA : MonoBehaviour {
    [SerializeField]
    float m_pollWindowWaitTime = 1f;

    [SerializeField]
    float m_showPollWaitTime = 1f;

    [SerializeField]
    float m_beforeCountdownTime = 2f;

    [SerializeField]
    float m_percentCountdownTime = 1.5f;

    [SerializeField]
    float m_hashtagShowTime = 20f;

    bool m_yesNoActive = false;

    int m_pollQuestionCounter = 0;

    ContentManager.PollQuestion m_poll = null;
    AOrBAnswerInfo m_answerInfo = null;

    static PollA m_instance = null;
    public static PollA Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<PollA>();
            }
            return m_instance;
        }
    }

    void Awake() {
        PanelManager panelManager = PanelManager.Instance;
        panelManager.SubscribeToActivePanel(1, OnActivateYes);
        panelManager.SubscribeToActivePanel(2, OnActivateNo);
    }

    //For Poll A...
    void OnActivateYes(bool isYes) {
        if (m_yesNoActive) {
            CenterMenu centerMenu = CenterMenu.Instance;
            centerMenu.ShowActiveYes(isYes);
            centerMenu.HideInactiveYes(isYes);
        }
    }

    //For Poll A...
    void OnActivateNo(bool isNo) {
        if (m_yesNoActive) {
            RightMenu rightMenu = RightMenu.Instance;
            rightMenu.ShowActiveNo(isNo);
            rightMenu.HideInactiveNo(isNo);
        }
    }

    public void StartPoll() {
        if (AppManager.State == AppManager.AppState.POLL_A) {
            StartCoroutine(RunPoll());
        }
    }

    //Filters players from cube clusters they didn't vote for...
    void FilterActivePlayers(bool largeIsRight, bool smallIsRight) {
        PanelManager panelManager = PanelManager.Instance;
        PlayerManager playerManager = PlayerManager.Instance;
        LargeCubeCluster largeCubeCluster = LargeCubeCluster.Instance;
        SmallCubeCluster smallCubeCluster = SmallCubeCluster.Instance;
        EqualCubeCluster equalCubeCluster = EqualCubeCluster.Instance;

        //Filters players from cube clusters they didn't vote for...
        foreach (Player player in playerManager.ActivePlayers) {
            //Player is in left center panel...
            if (panelManager.ContainsPlayer(player, 1)) {
                if (largeIsRight) {
                    largeCubeCluster.FilteredPlayers.Add(player);
                }
                else if (smallIsRight) {
                    smallCubeCluster.FilteredPlayers.Add(player);
                }
                else {  //Both sides are equal...
                    largeCubeCluster.FilteredPlayers.Add(player);
                }
            }   //Player is in right center panel...
            else if (panelManager.ContainsPlayer(player, 2)) {
                if (largeIsRight) {
                    smallCubeCluster.FilteredPlayers.Add(player);
                }
                else if (smallIsRight) {
                    largeCubeCluster.FilteredPlayers.Add(player);
                }
                else {  //Both sides are equal...
                    equalCubeCluster.FilteredPlayers.Add(player);
                }
            }
            else {  //Player did not vote, so filter everything...
                smallCubeCluster.FilteredPlayers.Add(player);
                largeCubeCluster.FilteredPlayers.Add(player);
                equalCubeCluster.FilteredPlayers.Add(player);
            }
        }
    }

    //Poll starts here...
    IEnumerator RunPoll() {
        ContentManager content = ContentManager.Instance;
        CubeSpawner spawner = CubeSpawner.Instance;
        SideMenu sideMenu = SideMenu.Instance;

        CenterScreen centerScreen = CenterScreen.Instance;
        CenterMenu centerMenu = CenterMenu.Instance;
        LeftMenu leftMenu = LeftMenu.Instance;
        RightMenu rightMenu = RightMenu.Instance;

        yield return new WaitForSeconds(m_pollWindowWaitTime);
        Player.ShowMoveCubeText = true;
        rightMenu.ShowBackground(true);
        centerMenu.ShowBackground(true);
        leftMenu.ShowBackground(true);

        yield return new WaitForSeconds(1.2f);

        //Set poll question and responses...
        m_pollQuestionCounter = (m_pollQuestionCounter + 1) %
            content.PollAList.Count;
        m_poll = content.PollAList[m_pollQuestionCounter];
        content.OpenConnection();
        m_answerInfo = (AOrBAnswerInfo)content.GetPollAnswersA(m_poll.Question_Id);
        content.CloseConnection();

        centerScreen.HeaderText.text = m_poll.Question;
        centerMenu.ActivePollAAnswer.text = m_poll.Answer_a;
        centerMenu.InactivePollAAnswer.text = m_poll.Answer_a;
        rightMenu.ActivePollAAnswer.text = m_poll.Answer_b;
        rightMenu.InactivePollAAnswer.text = m_poll.Answer_b;

        yield return new WaitForSeconds(m_showPollWaitTime);
        centerScreen.ShowHeader(true);
        centerMenu.ShowPollA(true);
        rightMenu.ShowPollA(true);
        PanelManager.Instance.ClearPanel(1);
        PanelManager.Instance.ClearPanel(2);
        m_yesNoActive = true;

        StartCoroutine(RunPollCountdown());
    }

    IEnumerator RunPollCountdown() {
        ContentManager content = ContentManager.Instance;
        PanelManager panelManager = PanelManager.Instance;
        CenterScreen centerScreen = CenterScreen.Instance;
        SideMenu sideMenu = SideMenu.Instance;
        CenterMenu centerMenu = CenterMenu.Instance;
        RightMenu rightMenu = RightMenu.Instance;
        LeftMenu leftMenu = LeftMenu.Instance;
        LargeCubeCluster largeCubeCluster = LargeCubeCluster.Instance;
        SmallCubeCluster smallCubeCluster = SmallCubeCluster.Instance;
        EqualCubeCluster equalCubeCluster = EqualCubeCluster.Instance;

        float pollCountdown = StartupSettings.Instance.PollCountdown;

        PlayerManager playerManager = PlayerManager.Instance;
        var activePlayers = playerManager.ActivePlayers;

        yield return new WaitForSeconds(m_beforeCountdownTime);

        //Wait for show icon above to go away...
        yield return new WaitForSeconds(1f);
        Player.ShowMoveCubeText = false;
        Player.CountdownValue = (int)pollCountdown;
        Player.ShowCountdownValue = true;

        StartCoroutine(Player.RunCountdown(null));
        yield return new WaitForSeconds(pollCountdown + 1f);

        Player.ShowCountdownValue = false;
        yield return new WaitForSeconds(1f);

        m_yesNoActive = false;
        rightMenu.ShowActiveNo(false);
        centerMenu.ShowActiveYes(false);
        rightMenu.HideInactiveNo(false);
        centerMenu.HideInactiveYes(false);

        {   //Poll results start here...
            bool leftHasNotChanged = true;
            bool rightHasNotChanged = true;

            foreach (Player player in playerManager.ActivePlayers) {
                if (panelManager.ContainsPlayer(player, 1)) {    //center panel...
                    leftHasNotChanged = false;
                    m_answerInfo.answerA += 1;
                }
                else if (panelManager.ContainsPlayer(player, 2)) {   //right panel...
                    rightHasNotChanged = false;
                    m_answerInfo.answerB += 1;
                }
            }

            bool largeIsRight = m_answerInfo.answerA < m_answerInfo.answerB;
            bool smallIsRight = m_answerInfo.answerA > m_answerInfo.answerB;

            //Filters players from cube clusters they didn't vote for...
            FilterActivePlayers(largeIsRight, smallIsRight);

            centerScreen.MoveUpHeader(true);
            centerMenu.MoveDownYes(true);
            rightMenu.MoveDownNo(true);

            yield return new WaitForSeconds(1f);

            CubeCluster.HidePlayerCubes = false;
            Player.ShowColor = false;

            float total = m_answerInfo.answerA + m_answerInfo.answerB;
            int leftPercent = (int)((m_answerInfo.answerA / total) * 100);
            int rightPercent = (int)((m_answerInfo.answerB / total) * 100);

            centerMenu.StartPollAPercentCountdown(m_percentCountdownTime, leftPercent);
            rightMenu.StartPollAPercentCountdown(m_percentCountdownTime, rightPercent);

            largeCubeCluster.InstantShowCluster();
            smallCubeCluster.InstantShowCluster();
            equalCubeCluster.InstantShowCluster();

            if (largeIsRight) {
                centerMenu.ShowSmallResults(true);
                rightMenu.ShowLargeResults(true);
            }
            else if (smallIsRight) {
                centerMenu.ShowLargeResults(true);
                rightMenu.ShowSmallResults(true);
            }
            else {  //They are equal...
                rightMenu.ShowLargeResults(true);
                centerMenu.ShowEqualResults(true);
            }

            yield return new WaitForSeconds(m_percentCountdownTime);

            yield return new WaitForSeconds(2f);
            if (leftHasNotChanged) {
                centerMenu.HideLargePercent(true);
                centerMenu.HideSmallPercent(true);

                if (largeIsRight) {
                    smallCubeCluster.Explode();
                }
                else if (smallIsRight) {
                    largeCubeCluster.Explode();
                }
                else {
                    equalCubeCluster.Explode();
                }
            }
            if (rightHasNotChanged) {
                rightMenu.HideLargePercent(true);
                rightMenu.HideSmallPercent(true);

                if (largeIsRight) {
                    largeCubeCluster.Explode();
                }
                else if (smallIsRight) {
                    smallCubeCluster.Explode();
                }
                else {
                    largeCubeCluster.Explode();
                }
            }

            yield return new WaitForSeconds(StartupSettings.Instance.AfterPollATimeout);
            centerMenu.ShowLargeResults(false);
            centerMenu.ShowSmallResults(false);
            centerMenu.ShowEqualResults(false);
            rightMenu.ShowLargeResults(false);
            rightMenu.ShowSmallResults(false);
            rightMenu.ShowEqualResults(false);

            //Updating poll answers and starting takeaway...
            content.OpenConnection();
            content.PutPollAnswers(m_answerInfo);
            PollManager.Instance.StartTakeaway(m_poll.Question_Id);
            content.CloseConnection();

            yield return new WaitForSeconds(2f);
            centerMenu.HideLargePercent(false);
            rightMenu.HideLargePercent(false);
            centerMenu.HideSmallPercent(false);
            rightMenu.HideSmallPercent(false);

            largeCubeCluster.DestroyCluster();
            largeCubeCluster.CreateCluster();
            smallCubeCluster.DestroyCluster();
            smallCubeCluster.CreateCluster();
            equalCubeCluster.DestroyCluster();
            equalCubeCluster.CreateCluster();

            //Cleanup...
            largeCubeCluster.FilteredPlayers.Clear();
            smallCubeCluster.FilteredPlayers.Clear();
            equalCubeCluster.FilteredPlayers.Clear();
        }

        //Cleanup...
        Player.ShowColor = false;
        Player.ShowInactiveColor = true;
        Player.ShowCountdownValue = false;
        Player.ShowMoveIcon = true;
        StartCoroutine(Player.ShowHashtagOverTime(m_hashtagShowTime));

        sideMenu.ShowBackground(false);
        rightMenu.ShowBackground(false);
        centerMenu.ShowBackground(false);
        leftMenu.ShowBackground(false);
        centerScreen.ShowHeader(false);
        centerMenu.ShowPollA(false);
        rightMenu.ShowPollA(false);

        //Wait some time for animations above to take effect...
        yield return new WaitForSeconds(1.5f);
        centerScreen.ResetHeader();
        centerMenu.ResetYesAnimator();
        rightMenu.ResetNoAnimator();

        yield return new WaitForSeconds(1.5f);

        //Allow streaming cubes to start streaming in again...
        //TODO: Should be AppState.TAKE_AWAY...
        AppManager.State = AppManager.AppState.ATTRACT_CUBES;
        PanelManager.Instance.ClearAllPanels();
    }
}