using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using TMPro;
using System;

/*public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
{
    HashSet<TKey> seenKeys = new HashSet<TKey>();
    foreach (TSource element in source)
    {
        if (seenKeys.Add(keySelector(element)))
        {
            yield return element;
        }
    }
}*/

public class PollTakeaway : MonoBehaviour {

    public GameObject take_away;
    private List<TweetSearchTwitterData> TweetsList;
    private List<GameObject> cubes;
    bool check_region = true;

    public static event TwitterEvent TwitterAction;
    //private GameObject mController;

    // Use this for initialization
    void Start () {

        //mController = GameObject.Find("MasterController");
        cubes = new List<GameObject>();

    }
	
	// Update is called once per frame
	void Update () {


        /*if ((MainController.region_occupied[6] || MainController.region_occupied[7]) && check_region) {
            check_region = false;
            StartCoroutine(getPoll());
        }

        if ((MainController.region_occupied[0] || MainController.region_occupied[1]) && check_region) {
            //stop checking
            check_region = false;
            StartCoroutine(getAmbient());
        }*/

	}

    

    public void buildTakeaway()
    {
        removeAll();

        //TweetSearchTwitterData twitterData;

        Core.Instance._cms.OpenConnection();

        string tweet_query = Core.Instance._cms.GetTakeAway(MainController.Instance.current_question_id);

        Core.Instance._cms.CloseConnection();


        //string tweet_query = Core.Instance._cms.RequestTwitterRecords(MainController.Instance.current_question_id);

        TwitterAPI.instance.SearchTwitter(10, tweet_query, buildTwits); 

    }

    void removeAll()
    {
        foreach(GameObject cube in cubes)
        {

            Destroy(cube);

        }
    }

    void OnEnable(){

        check_region = true;

    }

    float count;
    //List<TweetSearchTwitterData> uniqueness_list;

    void buildTwits(List<TweetSearchTwitterData> tweetList)
    {

        Debug.Log("BUILDING TWITS LIST");

        count = 0;
        //updates the close up tweets
        //uniqueness_list = new List<TweetSearchTwitterData>();

        MainController.Instance.TweetsTakeaway = tweetList;

        Debug.Log("NEED TO SOLVE THE TAKEAWAY SCREENER UPDATE ISSUE");

        if (TwitterAction != null) TwitterAction("screeners_takeaway");

        List<TweetSearchTwitterData> uniqueness_list = tweetList
          .GroupBy(p => p.screenName)
          .Select(g => g.First())
          .ToList();

        for (int y = 0; y < uniqueness_list.Count; ++y)
        {

            makeTwit(uniqueness_list[y]);

        }

        MainController.Instance.AfterTakeaway();

    }


    void makeTwit(TweetSearchTwitterData tweetData)
    {

        GameObject clone = Instantiate(take_away, new Vector3(2, 10.0f - count * 2.5f, 0), transform.rotation) as GameObject;

        cubes.Add(clone);

        //clone.transform.Translate(new Vector3(15, 5, 15));
        clone.transform.parent = gameObject.transform;

        DynamicTexture dtex = clone.GetComponentInChildren<DynamicTexture>();

        TextMeshPro tweet = clone.GetComponentInChildren<TextMeshPro>();

        if (tweetData.profileImageUrl != "")
        {
            dtex.url = tweetData.profileImageUrl;
            dtex.Apply(false);
        }

        tweet.text = tweetData.screenName;

        count++;

        //if (count > 6)
        //{
        //    break;
        //}

        clone.transform.DOMoveX(30, 1).From().SetDelay(count / 10);
    }

}
