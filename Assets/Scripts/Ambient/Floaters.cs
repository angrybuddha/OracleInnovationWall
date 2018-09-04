using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Floaters : MonoBehaviour {

    public GameObject rounded_cube;
    private int current = 0;
    private List<TweetSearchTwitterData> TweetsList;

    List<GameObject> floats_array;
    float[] zoom_targets;
    bool zoom_bool;

    bool INIT = false;
    bool floaters_on = true;


    int maxX = 64;
    int timer = 0;
    int MAX_TIME = 1000;

    // Use this for initialization
    void Start() {

        DOTween.Init();

        MainController.TwitterAction += handleAction;
        //MainController.TwitterAction += toggleFloaters;

        floats_array = new List<GameObject>();

    }

    public void init()
    {
        foreach (GameObject cube in floats_array)
        {
            cube.GetComponent<RoundedCube>().showSelf();
        }
    }

    void handleAction(string action)
    {
        if(action == "build")
        {
            buildCubes();
        }
    }

    public void exit()
    {
        foreach (GameObject cube in floats_array)
        {
            cube.GetComponent<RoundedCube>().hideSelf();
        }
    }

    public void buildCubes() {

        MainController.TwitterAction -= handleAction;

        int counter = 0;
        float x_factor = 14;
        float y_factor = 3;
        float z_factor = 10;

        float noisefy = 0.5f;
        float noisefx = 7.0f;

        //float noisefy = 0f;
        //float noisefx = 0f;

        float cube_root = Mathf.Floor(Mathf.Pow(maxX, 1f / 3f));

        TweetSearchTwitterData twitterData;

        int max_count = MainController.Instance.TweetsList.Count;

        for (float x = 0; x < cube_root; ++x)
        {
            for (float y = 0; y < cube_root; ++y)
            {
                for (float z = 0; z < cube_root; ++z)
                {
                    //GameObject clone = Instantiate(rounded_cube, new Vector3(px / 2 * Random.Range(-2.0f, 2.0f), Random.Range(-5.0f, 5.0f), Random.Range(-10.0f, 30.0f)), transform.rotation) as GameObject;
                    float noisey = Random.Range(-noisefy, noisefy);
                    float noisex = Random.Range(-noisefx, noisefx);

                    GameObject clone = Instantiate(rounded_cube, new Vector3(x_factor*(x - (cube_root - 1)/2) + noisex, y_factor*(y - (cube_root - 1)/2) + noisey, z_factor*z), transform.rotation) as GameObject;

                    clone.GetComponent<RoundedCube>().image_quad.SetActive(true);

                    clone.GetComponent<RoundedCube>().speed = 1.0f - 1.5f * z / z_factor;

                    //clone.transform.parent = transform;
                    //ADD TWITTER DATA TO CUBE
                    if (counter >= max_count)
                        counter = 0;

                    twitterData = MainController.Instance.TweetsList[counter];

                    DynamicTexture dtex = clone.GetComponentInChildren<DynamicTexture>();
                    Text tweet = clone.GetComponentInChildren<Text>();

                    if (twitterData.tweetMedia != "")
                    {
                        dtex.url = twitterData.tweetMedia;
                        dtex.Apply(true);
                    }
                    else
                    {
                        clone.GetComponent<RoundedCube>().image_quad.SetActive(false);
                    }

                    string text = twitterData.tweetText;

                    clone.GetComponentInChildren<RoundedCube>().setTweetText(text);

                    clone.GetComponent<RoundedCube>().cube_grp.transform.Rotate(Vector3.up * Random.Range(0, 20));

                    clone.GetComponentInChildren<RoundedCube>().ambient = true;

                    floats_array.Add(clone);

                    counter++;
                }
            }
        }

        //turnOnFloaters();

        INIT = true;

    }

    public void DestroyAllCubes() {

        foreach (GameObject cube in floats_array) {
            Destroy(cube);
        }

    }
}
