using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwitterManager : MonoBehaviour {

    int m_numTwitterResults = 100;

    int m_tweetIndex = 0;
    int m_textTweetIndex = 0;
    int m_imgTweetIndex = 0;

    List<TweetSearchTwitterData> m_tweets = null;
    List<TweetSearchTwitterData> m_allTweets = null;
    List<TweetSearchTwitterData> m_textTweets = new List<TweetSearchTwitterData>();
    List<TweetSearchTwitterData> m_imgTweets = new List<TweetSearchTwitterData>();

    public int NumTweets {
        get { return m_tweets.Count; }
    }

    public int NumTextTweets {
        get { return m_textTweets.Count; }
    }

    public int NumImgTweets {
        get { return m_imgTweets.Count; }
    }

    bool m_isReady = false;
    public bool IsReady {
        get { return m_isReady; }
    }

    static TwitterManager m_instance = null;
    public static TwitterManager Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<TwitterManager>();
            }
            return m_instance;
        }
    }

    public void SetupTwitterCubes() {
        ContentManager content = ContentManager.Instance;

        m_isReady = false;
        LoadingScreen.Instance.PrintMessage("Connecting to Twitter...");

        content.OpenConnection();
        string searchKeywords = ContentManager.Instance.GetSearchQueryForAmbientHandles();
        content.CloseConnection();

        TwitterAPI.instance.SearchTwitter(m_numTwitterResults, searchKeywords, SearchAllTweetsResultsCallBack);
    }

    void SearchAllTweetsResultsCallBack(List<TweetSearchTwitterData> tweets) {
        LoadingScreen.Instance.PrintMessage("Building Ambient Cubes...");

        //Stores twitter information accessed by spawned cubes...
        m_allTweets = tweets;
        UpdateTweets(tweets);

        CubeSpawner.Instance.SpawnAmbientCubes();
        m_isReady = true;
    }

    public void ResetToAllTweets() {
        UpdateTweets(m_allTweets);
    }

    public void UpdateTweets(int expectedNumTweets, List<TweetSearchTwitterData> tweets) {
        int imgTweetIndex = Random.Range(0, m_imgTweets.Count - 1);
        List<TweetSearchTwitterData> updatedTweets = new List<TweetSearchTwitterData>();

        for (int i = 0, count = tweets.Count; i < expectedNumTweets; ++i) {
            if(i < count) {
                updatedTweets.Add(tweets[i]);
            }
            else {
                updatedTweets.Add(m_imgTweets[
                    (imgTweetIndex + i) % m_imgTweets.Count]);
            }
        }

        UpdateTweets(updatedTweets);
    }

    public void UpdateTweets(List<TweetSearchTwitterData> tweets) {
        m_tweets = tweets;
        m_textTweets = new List<TweetSearchTwitterData>();
        m_imgTweets = new List<TweetSearchTwitterData>();

        foreach (TweetSearchTwitterData data in m_tweets) {
            if (string.IsNullOrEmpty(data.tweetMedia)) {
                m_textTweets.Add(data);
            }
            else {
                m_imgTweets.Add(data);
            }
        }
    }

    public TweetSearchTwitterData GetNextTweet() {
        if (m_tweets.Count > 0) {
            int index = (m_tweetIndex++) % m_tweets.Count;
            return m_tweets[index];
        }

        return null;
    }

    public TweetSearchTwitterData GetNextTextTweet() {
        if (m_textTweets.Count > 0) {
            int index = (m_textTweetIndex++) % m_textTweets.Count;
            return m_textTweets[index];
        }

        return null;
    }

    public TweetSearchTwitterData GetNextImgTweet() {
        if (m_imgTweets.Count > 0) {
            int index = (m_imgTweetIndex++) % m_imgTweets.Count;
            return m_imgTweets[index];
        }

        return null;
    }
}
