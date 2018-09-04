using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

public class PollTypeA : MonoBehaviour {

    public GameObject poll_left;
    public GameObject poll_right;

    public TextMeshPro pct_left;
    public TextMeshPro pct_right;

    public TextMeshPro answer_left;
    public TextMeshPro answer_right;

    public GameObject red_bar_left;
    public GameObject red_bar_right;

    //private GameObject mController;

    public TextMeshPro question;
    public GameObject answer_grp;
    private string question_id;

    private bool checkRegion = true;
    int[] answer_array;
    float[] result_array = new float[2];


    // Use this for initialization
    void Start () {

        pct_left.text = "";
        pct_right.text = "";

        red_bar_left.SetActive(false);
        red_bar_right.SetActive(false);

        Reset();

    }

    void Reset()
    {
        //red_bar_right.SetActive(false);
        //red_bar_left.SetActive(false);
        pct_left.text = "";
        pct_right.text = "";
        poll_left.SetActive(false);
        poll_right.SetActive(false);
        question.transform.DOMoveY(10, 0);
        answer_grp.transform.DOMoveY(-10, 0);
    }

    public void init()
    {

        question_id = Core.Instance._cms.PollList[MainController.Instance.POLL_POINTER].Question_Id;
        MainController.Instance.current_question_id = question_id;

        Core.Instance._cms.OpenConnection();

        answer_array = Core.Instance._cms.GetPollAnswersA(question_id);

        Core.Instance._cms.CloseConnection();

        question.text = Core.Instance._cms.PollList[MainController.Instance.POLL_POINTER].Question;
        answer_left.text = Core.Instance._cms.PollList[MainController.Instance.POLL_POINTER].Answer_a;
        answer_right.text = Core.Instance._cms.PollList[MainController.Instance.POLL_POINTER].Answer_b;

        question.transform.DOMoveY(4.5f, 0.5f);
        answer_grp.transform.DOMoveY(0, 0.5f);

        StartCoroutine(checkVote());

    }

    void exit()
    {
        pct_left.text = "";
        pct_right.text = "";
        poll_left.SetActive(false);
        poll_right.SetActive(false);
        question.transform.DOMoveY(10, 1);
        answer_grp.transform.DOMoveY(-10, 1);
    }
	
	// Update is called once per frame
	void Update () {


        if ((MainController.region_occupied[2] || MainController.region_occupied[3]) && checkRegion)
        {
            answer_left.color = new Color(1, 1, 1);
            red_bar_left.SetActive(true);
        }
        else
        {
            answer_left.color = new Color(0, 0, 0);
            red_bar_left.SetActive(false);
        }

        if ((MainController.region_occupied[4] || MainController.region_occupied[5]) && checkRegion)
        {
            answer_right.color = new Color(1, 1, 1);
            red_bar_right.SetActive(true);
        }
        else
        {
            answer_right.color = new Color(0, 0, 0);
            red_bar_right.SetActive(false);
        }
	}

    IEnumerator checkVote()
    {
        
        
        //then explode cubes
        yield return new WaitForSeconds(11);
        MainController.Instance.submitVoteType01();

        yield return new WaitForSeconds(1);
        moveParts();

        yield return new WaitForSeconds(2);
        showResult1();

        yield return new WaitForSeconds(2);
        showResult2();

        yield return new WaitForSeconds(8);
        exit();

        yield return new WaitForSeconds(1);
        changeSection();

    }

    void moveParts()
    {
        checkRegion = false;
        red_bar_right.SetActive(false);
        red_bar_left.SetActive(false);
        question.transform.DOMoveY(5, 1);
        answer_grp.transform.DOMoveY(-5, 1);
    }

    void showResult1()
    {

        List<int> color_me = new List<int>();

        float pct = Mathf.Round(100*((float)answer_array[0] / (float)(answer_array[0] + answer_array[1])));
        string display = pct.ToString();
        int index = Mathf.FloorToInt(pct * .1f);

        foreach (int id in MainController.Instance.active_player_list)
        {
            if (PlayerController.Instance.CubePlayers[id - 1].GetComponent<PlayerCube>().region_id == 2 || PlayerController.Instance.CubePlayers[id - 1].GetComponent<PlayerCube>().region_id == 3)
            {
                color_me.Add(id);
            }
        }

        result_array[0] = answer_array[0] + color_me.Count;


        //pct_left.SetActive(true);
        pct_left.text = display + "%";//pct1.ToString() + "%";
        pct_left.material.DOFade(0, 0.5f).From();

        //poll_left.GetComponent<Duplicater>().check_arr = [0, 1];
        poll_left.SetActive(true);

        if(index == 10)
        {
            index = 9;
        }

        poll_left.GetComponent<Duplicater>().initCubes(index, color_me);
        
    }

    void showResult2()
    {

        List<int> color_me = new List<int>();

        float pct = Mathf.Round(100*((float)answer_array[1] / (float)(answer_array[0] + answer_array[1])));
        string display = pct.ToString();
        int index = Mathf.FloorToInt(pct * .1f);

        foreach (int id in MainController.Instance.active_player_list)
        {
            if (PlayerController.Instance.CubePlayers[id - 1].GetComponent<PlayerCube>().region_id == 4 || PlayerController.Instance.CubePlayers[id - 1].GetComponent<PlayerCube>().region_id == 5)
            {
                color_me.Add(id);
            }
        }

        pct_right.text = display + "%";//pct2.ToString() + "%";
        pct_right.material.DOFade(0, 0.5f).From();

        if (index == 10)
        {
            index = 9;
        }

        poll_right.SetActive(true);
        poll_right.GetComponent<Duplicater>().initCubes(index, color_me);

        result_array[1] = answer_array[1] + color_me.Count;

        //SAVE ALL POLL DATA
        //Core.Instance._cms.OpenConnection();

        Core.Instance._cms.PutPollAnswersA(question_id, result_array);

        //Core.Instance._cms.CloseConnection();

    }


    void changeSection()
    {
        //change section to pollTypeA
        MainController.Instance.pollTakeawaySection();
    }

    void OnDisable()
    {
        checkRegion = false;
        StopAllCoroutines();
    }

    void OnEnable()
    {
        checkRegion = true;
        
    }
}
