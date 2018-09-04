using UnityEngine;
using System.Collections;

using GraphAnswerInfo = ContentManager.GraphAnswerInfo;

public class PollB : MonoBehaviour {

    [SerializeField]
    float m_pollWindowWaitTime = 1f;

    [SerializeField]
    float m_showPollWaitTime = 1f;

    [SerializeField]
    float m_beforeCountdownTime = 2f;

    [SerializeField]
    float m_hashtagShowTime = 20f;

    int m_pollQuestionCounter = 0;

    static PollB m_instance = null;
    public static PollB Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<PollB>();
            }
            return m_instance;
        }
    }

    public void StartPoll() {
        if (AppManager.State == AppManager.AppState.POLL_B) {
            StartCoroutine(RunPoll());
        }
    }

    //Poll starts here...
    IEnumerator RunPoll() {
        ContentManager content = ContentManager.Instance;
        CenterScreen centerScreen = CenterScreen.Instance;
        CenterMenu centerMenu = CenterMenu.Instance;
        LeftMenu leftMenu = LeftMenu.Instance;
        RightMenu rightMenu = RightMenu.Instance;

        //Wait for show icon above to show...
        yield return new WaitForSeconds(1f);

        // Set poll question and responses...
        m_pollQuestionCounter = (m_pollQuestionCounter + 1) %
            content.PollBList.Count;

        var poll = content.PollBList[m_pollQuestionCounter];
        centerScreen.HeaderText.text = poll.Question;
        centerScreen.FirstLabel.text = poll.Answer_a;
        centerScreen.LastLabel.text = poll.Answer_b;

        yield return new WaitForSeconds(m_pollWindowWaitTime);
        Player.ShowMoveCubeText = true;
        rightMenu.ShowBackground(true);
        centerMenu.ShowBackground(true);
        leftMenu.ShowBackground(true);

        yield return new WaitForSeconds(m_showPollWaitTime);
        centerScreen.ShowHeader(true);
        centerScreen.ShowPollB(true);
        StartCoroutine(RunPollCountdown());
    }

    IEnumerator RunPollCountdown() {
        ContentManager content = ContentManager.Instance;
        CenterScreen centerScreen = CenterScreen.Instance;
        SideMenu sideMenu = SideMenu.Instance;
        CenterMenu centerMenu = CenterMenu.Instance;
        RightMenu rightMenu = RightMenu.Instance;
        LeftMenu leftMenu = LeftMenu.Instance;
        BarGraph barGraph = BarGraph.Instance;
        PlayerManager playerManager = PlayerManager.Instance;

        float pollCountdown = StartupSettings.Instance.PollCountdown;

        GraphAnswerInfo answerInfo = null;
        var poll = content.PollBList[m_pollQuestionCounter];
        var activePlayers = playerManager.ActivePlayers;

        yield return new WaitForSeconds(m_beforeCountdownTime);
        Player.CheckRange = true;

        //Wait for show icon above to go away...
        yield return new WaitForSeconds(1f);
        Player.ShowMoveCubeText = false;
        Player.CountdownValue = (int)pollCountdown;
        Player.ShowCountdownValue = true;

        StartCoroutine(Player.RunCountdown(null));

        yield return new WaitForSeconds(pollCountdown + 1f);
        Player.ShowCountdownValue = false;
        Player.CheckRange = false;

        yield return new WaitForSeconds(1f);
        centerScreen.MoveUpHeader(true);
        centerScreen.HideRangeBar(true);
        centerScreen.ShowBarGraph(true);

        //Wait some time before showing bar graph...
        yield return new WaitForSeconds(1f);

        //Bar graph stuff...
        {
            content.OpenConnection();

            answerInfo = (GraphAnswerInfo)content.GetPollAnswersB(poll.Question_Id);
            content.CloseConnection();

            //Setting data and showing bar graph...
            int highestNum = 0;
            foreach (int num in answerInfo.numbers) {
                if (num > highestNum) {
                    highestNum = num;
                }
            }

            float multipler = highestNum > 0f ? 10f / highestNum : 0f;
            int numVotesPerCube = (int)Mathf.Ceil(highestNum / 10f);

            for (int i = 0, count = answerInfo.numbers.Count; i < count; ++i) {
                int num = highestNum > 10 ? (int)Mathf.Ceil(answerInfo.numbers[i] /
                    ((float)(numVotesPerCube))) :
                    (int)Mathf.Round(answerInfo.numbers[i] * multipler);
                barGraph.SetNumberRows(i, num);
            }

            barGraph.ShowTable(true);

            //Adding player cubes to bar graph...
            yield return new WaitForSeconds(3f);
            foreach (Player player in activePlayers) {
                float range = 0f;
                if (centerScreen.InRangeOfRangeBar(
                    player.transform.position, ref range)) {

                    int index = (int)(answerInfo.numbers.Count * range);
                    GameObject cube = barGraph.AddCubeToRow(index);
                    barGraph.ShowCube(cube, player.ActiveColor);
                    ++answerInfo.numbers[index];

                    Player.ShowColor = false;
                }
            }
        }

        //Wait some time before cleaning up bar graph...
        yield return new WaitForSeconds(
            StartupSettings.Instance.AfterPollBTimeout);

        barGraph.HideTable();

        //Wait some time for animation above to take effect...
        yield return new WaitForSeconds(2f);
        content.OpenConnection();
        content.PutPollAnswers(answerInfo);
        PollManager.Instance.StartTakeaway(poll.Question_Id);
        content.CloseConnection();

        yield return new WaitForSeconds(1f);

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
        centerScreen.ShowPollB(false);

        //Wait some time for animations above to take effect...
        yield return new WaitForSeconds(2f);
        centerScreen.ResetHeader();
        centerScreen.HideRangeBar(false);
        centerScreen.ShowBarGraph(false);

        //Puts app back in ambient mode...
        AppManager.State = AppManager.AppState.ATTRACT_CUBES;
        PanelManager.Instance.ClearAllPanels();
    }
}