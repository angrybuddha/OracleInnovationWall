using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
//public delegate void ChangeSectionEvent(string section);

public class PollActivate : MonoBehaviour {

    //Master controller reference 
    //(can't call function unless static/but then objects aren't) use Zenject next time.

    //private GameObject mController;

    public GameObject cubeCrystal;
    public GameObject backDrop_top;
    public GameObject backDrop_bottom;
    public GameObject fakeShadow;
    public TextMeshPro title;
    public TextMeshPro message;

    private GameObject current_player;

    private bool checkRegion;


	// Use this for initialization
	void Start () {

        title.transform.DOMoveX(30, 0);
        message.transform.DOMoveX(30, 0);

        //backDrop_bottom.transform.DOMoveY(-17, 0).SetEase(Ease.OutQuad);
        backDrop_top.transform.DOMoveY(50, 0).SetEase(Ease.OutQuad);

    }

    public void init()
    {

        fakeShadow.transform.DOScaleX(0, 0);

        title.transform.DOMoveX(30, 0);
        message.transform.DOMoveX(30, 0);

        //backDrop_bottom.transform.DOMoveY(-10.0f, 1.0f).SetEase(Ease.OutQuad);
        backDrop_top.transform.DOMoveY(3.6f, 0.75f).SetEase(Ease.OutQuad).OnComplete(initCrystal);

        title.transform.DOMoveX(0.0f, 1).SetEase(Ease.OutQuad);
        message.transform.DOMoveX(0.0f, 1).SetEase(Ease.OutQuad).SetDelay(0.5f); 

    }

    void exit()
    {

        //backDrop_bottom.transform.DOMoveY(-25, 1).SetEase(Ease.OutQuad);
        backDrop_top.transform.DOMoveY(60, 1).SetEase(Ease.InQuad);

        title.transform.DOMoveX(30, 1);
        message.transform.DOMoveX(30, 1);

    }
    
    public void initCrystal()
    {

        List<int> color_me = new List<int>();
        cubeCrystal.GetComponent<Duplicater>().initCubes(9, color_me);

        fakeShadow.transform.DOScaleX(8, 1).SetEase(Ease.OutQuad).SetDelay(1);

    }

    // 
    void Update () {

        if ((MainController.region_occupied[6] || MainController.region_occupied[7]) && checkRegion)
        {
            //stop checking
            checkRegion = false;

            //find the trigger person
            current_player = PlayerController.Instance.getActivePlayer(6, 7);

            //player.transform.DOMove(new Vector3(0, 0, 0), 0);

            StartCoroutine(playElements());

        }
    }


    IEnumerator playElements()
    {

        //stop player updates
        //MainController.Instance.do_player_update = false;

        //then explode cubes
        yield return new WaitForSeconds(3);
        cubeCrystal.GetComponent<Duplicater>().explode();

        yield return new WaitForSeconds(3.0F);
        changeMessaging();
        current_player.GetComponent<PlayerCube>().doCountdown5();

        yield return new WaitForSeconds(10);
        exit();
        MainController.Instance.hideFloaters();

        yield return new WaitForSeconds(2);
        PlayerController.Instance.showAllCubes();
        changeSection();
        MainController.Instance.do_player_update = true;

    }

    void changeMessaging()
    {

        title.transform.DOMoveX(30, 1).SetEase(Ease.OutQuad);
        message.transform.DOMoveX(30, 1).SetEase(Ease.OutQuad).SetDelay(0.25f).OnComplete(showNewMessaging);

    }

    void showNewMessaging()
    {

        title.text = "Let's get started";
        message.text = "The cube represents your vote. Move your body to submit your answer";

        title.transform.DOMoveX(0.0f, 1).SetEase(Ease.OutQuad);
        message.transform.DOMoveX(0.0f, 1).SetEase(Ease.OutQuad).SetDelay(0.5f);

        //start count down of 5 seconds only

    }

    void changeSection()
    {
        //change section to pollTypeA
        MainController.Instance.NextSection(false);
    }

    void OnDisable()
    {
        checkRegion = false;
    }

    void OnEnable()
    {

        checkRegion = true;
       
        //cubeCrystal.GetComponent<Duplicater>().playAnimation();
        
    }
}
