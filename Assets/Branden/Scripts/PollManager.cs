using UnityEngine;
using System.Collections.Generic;

public class PollManager : MonoBehaviour {
    int m_pollTypeCounter = 0;
    int m_numberTakeawayResults = 15;
    bool m_twitterContentIsDirty = false;

    static PollManager m_instance = null;
    public static PollManager Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<PollManager>();
            }
            return m_instance;
        }
    }

    public void StartNextPoll() {
        ContentManager content = ContentManager.Instance;
        PollA pollA = PollA.Instance;
        PollB pollB = PollB.Instance;

        //Hide "Let's Get Started" text...
        SideMenu.Instance.ShowGetStarted(false);
        Player.ShowTakePollText = false;

        Player.ShowCountdownValue = false;  //Clears countdown...

        bool canRunPollA = content.PollAList.Count > 0;
        bool canRunPollB = content.PollBList.Count > 0;

        bool runPollA = m_pollTypeCounter == 0;

        if ((canRunPollA && runPollA) || (canRunPollA && !canRunPollB)) {
            AppManager.State = AppManager.AppState.POLL_A;
            pollA.StartPoll();
        }
        else if(canRunPollB) {
            AppManager.State = AppManager.AppState.POLL_B;
            pollB.StartPoll();
        }

        //Incrementing to next poll type...
        m_pollTypeCounter = ((m_pollTypeCounter + 1) % 2);
    }

    public void StartTakeaway(string questionId) {
        string query = ContentManager.Instance.GetSearchQueryFromQuestion(questionId);
        TwitterAPI.instance.SearchTwitter(m_numberTakeawayResults,
            query, UpdateTwitterCallback);

        //TODO: Fix this up later...
        //string str = "@" + query.Split(':')[1];
        //Player.HashtagStr = Utility.AddNewLines(str, 12);
    }

    public void ResetTwitter() {
        if (m_twitterContentIsDirty) {
            TwitterManager.Instance.ResetToAllTweets();
            TwitterCube.ResetTwitterContent();
            m_twitterContentIsDirty = false;
        }
    }

    void UpdateTwitterCallback(List<TweetSearchTwitterData> tweetList) {
        m_twitterContentIsDirty = true;
        TwitterManager.Instance.UpdateTweets(StartupSettings.Instance.MinTakeawayTweets, tweetList);
        TwitterCube.ResetTwitterContent();
    }
}
