using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

public class PollTypeB : MonoBehaviour {

    /*GAME OBJECTS*/

    [SerializeField, Header("Objects:")]
    public GameObject poll_graph;
    public GameObject answer;
    public GameObject line;

    public TextMeshPro question;
    public TextMeshPro answer_left;
    public TextMeshPro answer_right;

    private string question_id;

    public List<int> answer_array;

    // Use this for initialization
	void Start () {

        //init();
        Reset();

	}

    void Reset()
    {
        //red_bar_right.SetActive(false);
        //red_bar_left.SetActive(false);

        question.transform.DOMoveY(10, 0);
        answer.transform.DOMoveY(-10, 0);
        //line.transform.DOMoveY(0, 0);
        line.transform.DOScaleY(0, 0);
    }

    public void init()
    {
        Reset();

        poll_graph.SetActive(false);

        question_id = Core.Instance._cms.PollList[MainController.Instance.POLL_POINTER].Question_Id;

        Debug.Log("THIS IS THE ID " + question_id);

        MainController.Instance.current_question_id = question_id;

        question.text = Core.Instance._cms.PollList[MainController.Instance.POLL_POINTER].Question;
        answer_left.text = Core.Instance._cms.PollList[MainController.Instance.POLL_POINTER].Answer_a;
        answer_right.text = Core.Instance._cms.PollList[MainController.Instance.POLL_POINTER].Answer_b;

        question.transform.DOMoveY(4.5f, 0);
        question.material.DOFade(0, 0.5f).From();

        answer.transform.DOMoveY(0, 0);

        line.SetActive(true);
        line.transform.DOScaleY(1, 0.5f);

        Core.Instance._cms.OpenConnection();

        //Debug.Log("Grabbing Answers");
        //GRAB ANSWERS FROM THE DATABASE
        answer_array = Core.Instance._cms.GetPollAnswersB(question_id);

        Core.Instance._cms.CloseConnection();

        Core.Instance._cms.scatter_plot_answer_list = answer_array;

        StartCoroutine(checkVote());
    }

    void exit()
    {
        question.transform.DOMoveY(10, 1);
        answer.transform.DOMoveY(-10, 1);
        //line.transform.DOMoveY(0, 0);
        line.transform.DOScaleY(0, 1);
    }

    IEnumerator checkVote()
    {

        //then explode cubes
        yield return new WaitForSeconds(11);
        MainController.Instance.submitVoteType02();

        yield return new WaitForSeconds(1);

        poll_graph.GetComponent<PollGraph>().build(answer_array);
        poll_graph.SetActive(true);
        //moveParts();

        yield return new WaitForSeconds(6);
        exit();

        yield return new WaitForSeconds(1);
        changeSection();

    }

    void changeSection()
    {
        Core.Instance._cms.PutPollAnswersB(question_id, Core.Instance._cms.scatter_plot_answer_list);
        //change section to next Poll
        MainController.Instance.pollTakeawaySection();

    }

    private void moveParts()
    {
        //question.transform.DOMoveY(6, 1);
        answer.transform.DOMoveY(-3, 1);
        //line.transform.DOMoveY(-3, 1);
    }

    public void ShowResult()
    {

        //Debug.Log("SHOW RESULT PLEASE");
        poll_graph.SetActive(true);

        poll_graph.GetComponent<PollGraph>().playIntro();

        //Core.Instance._cms.PutPollAnswersB(id, answer_array);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnEnable()
    {
        
    }
}
