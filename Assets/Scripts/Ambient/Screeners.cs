/*******************************************************************************************
* Author: Jed Bursiek
* Created Date: 08-15-16 
* 
* Description: 
*   Used for ambient twitter cubes float to screen - close up cubes
*******************************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Screeners : MonoBehaviour {

    public GameObject rounded_cube;
    private int current = 0;
    private List<TweetSearchTwitterData> TweetsList;

    List<GameObject> screeners_array;
    float[] zoom_targets;
    bool zoom_bool;

    bool INIT = false;
    bool floaters_on = true;


    int timer = 0;
    int MAX_TIME = 1000;

    TweetSearchTwitterData twitterData;

    // Use this for initialization
    void Start() {

        DOTween.Init();

        MainController.TwitterAction += handleAction;

        screeners_array = new List<GameObject>();
    }

    public void handleAction(string action)
    {
        if(action == "build")
        {

            TweetsList =  MainController.Instance.TweetsList;

            buildCubes();

        }else if(action == "screeners_takeaway")
        {

            TweetsList = MainController.Instance.TweetsTakeaway;

            buildCubes();

        }
        else if(action == "toggle_on")
        {

            toggleFloaters(true);


        }else if(action == "toggle_off")
        {

            Debug.Log("REQUEST TO TURN OFF SCREENERS");
            toggleFloaters(false);

        }
    }


    public void buildCubes() {

        DestroyAllCubes();

        int counter = 0;

        float cube_root = TweetsList.Count;

        //MAXIMUM OF 10
        if (cube_root > 10)
            cube_root = 10;

        screeners_array = new List<GameObject>();

        for (float x = 0; x < cube_root; ++x)
        {

            GameObject clone = Instantiate(rounded_cube, new Vector3(-20.0f, Random.Range(-1.5f, 1.5f), 0), transform.rotation) as GameObject;
            clone.GetComponent<RoundedCube>().image_quad.SetActive(true);

            clone.transform.parent = transform;

            //ADD TWITTER DATA TO CUBE
            twitterData = TweetsList[counter];

            DynamicTexture dtex = clone.GetComponentInChildren<DynamicTexture>();

            Text tweet = clone.GetComponentInChildren<Text>();

            if (twitterData.tweetMedia != "")
            {
                dtex.url = twitterData.tweetMedia;
                dtex.Apply(true);
                clone.GetComponent<RoundedCube>().hasImage = true;
            }
            else
            {
                clone.GetComponent<RoundedCube>().image_quad.SetActive(false);
            }

            string text = twitterData.tweetText;

            clone.GetComponentInChildren<RoundedCube>().setTweetText(text);

            //THESE ARE ALL OUR FLOATERS
            //clone.GetComponent<RoundedCube>().floater = true;

            clone.GetComponent<RoundedCube>().index = 5 / 2 * Random.Range(-2.0f, 2.0f);

            clone.GetComponent<RoundedCube>().ypos = Random.Range(-1.5f, 1.5f);

            screeners_array.Add(clone);

            counter++;

        }

        turnOnFloater();

        INIT = true;

    }
    

    public void init()
    {

    }

    public void exit()
    {

    }

    //float countdown = 0;
    
    float range = 20;

    int pointer = 0;

    bool screeners = true;

    void Update()
    {
        if (screeners) {

            //Debug.Log("Timer time " + Time.time + " " + " range " + range);

            if (Time.time > range)
            {

                screeners = false;

                fireScreener();

            }
        }
    }

    void fireScreener()
    {
        //RESET THE RANGE SO WE ONLY FIRE ONCE
        range = Time.time + Random.Range(9, 30);

        turnOnFloater();

        screeners = true;
    }

    private void toggleFloaters(bool on)
    {
        if(on)
        {
            Debug.Log("TURNING ON SCREENERS");
            //screeners = true;
            fireScreener();

        }
        else if(!on)
        {
            Debug.Log("TURNING OFF SCREENERS");
            screeners = false;
        }
    }

    /*IEnumerator turnOffFloaters() {

        foreach (GameObject floater in floats_array) {
            floater.GetComponent<RoundedCube>().floater = false;
        }
        floaters_on = false;
    }*/

    void turnOnFloater() {

        Debug.Log("TURNING ON FLOATER " + pointer);

        screeners_array[pointer].GetComponent<RoundedCube>().doFloat();

        pointer++;

        if(pointer >= screeners_array.Count)
        {
            pointer = 0;
        }

    }

    public void DestroyAllCubes() {

        //toggleFloaters(false);

        if(screeners_array.Count > 0) { 

            foreach (GameObject screener_obj in screeners_array) {

                Destroy(screener_obj);

            }
        }
    }
}
