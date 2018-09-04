/*******************************************************************************************
* Author: Jed Bursiek
* Created Date: 07-01-16 
* 
* Description: 
*   Used for ambient twitter cube reactive mode - cube towers
*******************************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class CubeRegion : MonoBehaviour
{

    public GameObject rounded_cube;
    public int cube_id;
    //public Camera local_camera;
    private int current = 0;
    private List<TweetSearchTwitterData> TweetsList;

    private int time_to_activate;

    GameObject[] cubes_array;
    float[] zoom_targets;
    bool zoom_bool;

    bool INIT = false;

    //int maxY = 8;
    int maxX = 10;
    int counter = 0;

    // Use this for initialization
    void Start()
    {

        DOTween.Init();

        MainController.TwitterAction += handleAction;

    }

    void handleAction(string action)
    {
        if(action == "build" && !INIT)
        {
            INIT = true;

            buildCubes();
        }
        else if(action == "exit")
        {
            exit();
        }else if(action == "reset_region")
        {
            Reset();
        }
    }

    void exit()
    {

    }

    public void buildCubes()
    {
        //Camera camera = GetComponent<Camera>();
        //Vector3 p0 = camera.ScreenToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));

        //MainController.TwitterAction -= handleAction;

        float scalar = 0.68F;// Core.Instance._settings.Kinect_area_X/100;

        int counter = 0;

        //cubes_array = new GameObject[maxY, maxX];
        cubes_array = new GameObject[maxX];

        zoom_targets = new float[8];

        zoom_targets[0] = -scalar - scalar * 6;

        zoom_targets[1] = -scalar - scalar * 4;

        zoom_targets[2] = -scalar - scalar * 2;

        zoom_targets[3] = -scalar;

        zoom_targets[4] = scalar;

        zoom_targets[5] = scalar + scalar * 2;

        zoom_targets[6] = scalar + scalar * 4;

        zoom_targets[7] = scalar + scalar * 6;

        //zoom_bool = new bool[8];

        TweetSearchTwitterData twitterData;

        int tweet_counter = 0;
        int tweet_counter_max = MainController.Instance.TweetsList.Count;
        int tweet_index;

        for (int px = 0; px < maxX; ++px)
        {

            GameObject clone = Instantiate(rounded_cube, new Vector3(gameObject.transform.position.x, -4.0F, 10.0f), transform.rotation) as GameObject;

            clone.GetComponent<RoundedCube>().image_quad.SetActive(true);

            //ATTACH TO PARENT
            clone.transform.parent = gameObject.transform;


            //ADD TWITTER DATA TO CUBE
            tweet_index = tweet_counter + (cube_id - 1) * maxX;

            if (tweet_index >= tweet_counter_max) {

                tweet_counter = 0;
                tweet_index = tweet_counter_max -1;

            }

            twitterData = MainController.Instance.TweetsList[tweet_index];

            tweet_counter++;

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

            //clone.GetComponent<RoundedCube>().cube_grp.transform.Rotate(Vector3.up * 90);// Random.Range(0, 20));

            cubes_array[px] = clone;

            counter++;

            //clone.transform.DOScale(0, 0);

            //clone.SetActive(false);

        }

        INIT = true;

    }


    public void ZoomIn()
    {

        zoom_bool = true;//call once

        time_to_activate = 0;

        StartCoroutine(loopDeLoop());

    }

    IEnumerator loopDeLoop()
    {

        immediateCubes();

        while (zoom_bool)

        {

            counter++;

            time_to_activate++;

            if (counter >= maxX)
            {
                counter = 0;
            }

            if (time_to_activate > MainController.Instance.TIME_TO_ACTIVATE_POLL) {

                MainController.Instance.promptPollSection();
                
                //StopAllCoroutines();
                //break;
            }

            yield return new WaitForSeconds(3.2f);

            cubes_array[counter].GetComponent<RoundedCube>().KillTween();//kill previous

            cubes_array[counter].GetComponent<RoundedCube>().DoZoom(zoom_targets[cube_id - 1]);//move yourself

        }

    }

    private void immediateCubes()
    {
        //call once for immediate result
        cubes_array[counter].GetComponent<RoundedCube>().KillTween();//kill previous

        cubes_array[counter].GetComponent<RoundedCube>().DoZoom(zoom_targets[cube_id - 1]);//move yourself

        /*for(int i = 0; i < 3; i++)
        {

            counter++;
            cubes_array[counter].GetComponent<RoundedCube>().DoZoom(zoom_targets[cube_id - 1], -2.0f*i);//move yourself

        }*/

    }


    private void startTrigger()
    {
        MainController.Instance.promptPollSection();
    }


    public void ZoomOut()
    {
        zoom_bool = false;//call once

        StopAllCoroutines();

        for (var i=0; i<maxX; i++) {

            cubes_array[i].GetComponent<RoundedCube>().KillTween();//kill previous
            cubes_array[i].GetComponent<RoundedCube>().DoZoomOut();
        }

    }

    public void Reset()
    {
        foreach(GameObject cube in cubes_array)
        {

            cube.transform.DOScale(0, 0);

            //cube.SetActive(false);

        }

    }


    // Update is called once per frame
    void Update() {

        if (INIT)
        {

            if (MainController.region_occupied[cube_id -1] && !zoom_bool)
            {

                 ZoomIn();

            }
            else if (!MainController.region_occupied[cube_id-1] && zoom_bool)
            {

                ZoomOut();

            }

        }

    }
}