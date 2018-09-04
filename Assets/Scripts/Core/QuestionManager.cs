/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used for managing all question data. 
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CircularGravityForce;
using ParticlePlayground;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    //Used for question state
    public enum QuestionState
    {
        None,
        Poll,
        MultipleChoice,
        ScatterPlot
    }

    //Used for vote state
    public enum VoteState
    {
        None,
        Waiting,
        Voting,
        Voted,
    }

    //Selected question type id
    [SerializeField, Header("Question Info: ")]    
    private string questionTypeId;
    public string QuestionTypeId
    {
        get { return questionTypeId; }
        set { questionTypeId = value; }
    }

    //Selected question id
    [SerializeField]
    private string questionId;
    public string QuestionId
    {
        get { return questionId; }
        set { questionId = value; }
    }

    //Current selected question state
    [SerializeField]
    private QuestionState questionState = QuestionState.None;
    public QuestionState _questionState
    {
        get { return questionState; }
        set
        {
            Core.Instance._playerManager.ResetAllPlayerModeTime();
            questionState = value;
        }
    }

    //Question text
    [SerializeField, TextArea]
    private string question1 = "Question 1";
    public string Question1
    {
        get { return question1; }
        set { question1 = value; }
    }

    //Question text 2, only used for scatter plots
    [SerializeField, TextArea]
    private string question2 = "Question 2";
    public string Question2
    {
        get { return question2; }
        set { question2 = value; }
    }

    //Answer text 1
    [SerializeField]
    private string answer1Title;
    public string Answer1Title
    {
        get { return answer1Title; }
        set { answer1Title = value; }
    }

    //Answer text 2
    [SerializeField]
    private string answer2Title;
    public string Answer2Title
    {
        get { return answer2Title; }
        set { answer2Title = value; }
    }

    //Answer text 3
    [SerializeField]
    private string answer3Title;
    public string Answer3Title
    {
        get { return answer3Title; }
        set { answer3Title = value; }
    }

    //Answer text 4
    [SerializeField]
    private string answer4Title;
    public string Answer4Title
    {
        get { return answer4Title; }
        set { answer4Title = value; }
    }

    //Answer 1 records
    [SerializeField]
    private float answer1Records = 0;
    public float Answer1Records
    {
        get { return answer1Records; }
        set { answer1Records = value; }
    }

    //Answer 2 records
    [SerializeField]
    private float answer2Records = 0;
    public float Answer2Records
    {
        get { return answer2Records; }
        set { answer2Records = value; }
    }

    //Answer 3 records
    [SerializeField]
    private float answer3Records = 0;
    public float Answer3Records
    {
        get { return answer3Records; }
        set { answer3Records = value; }
    }

    //Answer Vector records, used for scatter plot
    [SerializeField]
    private List<Vector2> answerVectorRecords;
    public List<Vector2> AnswerVectorRecords
    {
        get { return answerVectorRecords; }
        set { answerVectorRecords = value; }
    }

    //Fact text
    [SerializeField, TextArea]
    private string fact;
    public string Fact
    {
        get { return fact; }
        set { fact = value; }
    }

    //Proposition text
    [SerializeField, TextArea]
    private string proposition;
    public string Proposition
    {
        get { return proposition; }
        set { proposition = value; }
    }

    //Image fact URL
    [SerializeField]
    private string factURL;
    public string FactURL
    {
        get { return factURL; }
        set { factURL = value; }
    }

    //Image used for the fact
    [SerializeField]
    private Image factImage;
    public Image FactImage
    {
        get { return factImage; }
        set { factImage = value; }
    }

    //Answer 1 percent
    [SerializeField, Header("Static Question Info: ")]    
    private float answer1Percent = 0;
    public float Answer1Percent
    {
        get { return answer1Percent; }
        set { answer1Percent = value; }
    }

    //Answer 2 percent
    [SerializeField]
    private float answer2Percent = 0;
    public float Answer2Percent
    {
        get { return answer2Percent; }
        set { answer2Percent = value; }
    }

    //Answer 3 percent
    [SerializeField]
    private float answer3Percent = 0;
    public float Answer3Percent
    {
        get { return answer3Percent; }
        set { answer3Percent = value; }
    }

    //Ambient UI used
    [SerializeField, Header("UI:")]
    private GameObject ambientUI;
    public GameObject AmbientUI
    {
        get { return ambientUI; }
        set { ambientUI = value; }
    }

    //Poll UI used
    [SerializeField]
    private GameObject pollUI;
    public GameObject PollUI
    {
        get { return pollUI; }
        set { pollUI = value; }
    }

    //Multiple choice UI used
    [SerializeField]
    private GameObject multipleChoiceUI;
    public GameObject MultipleChoiceUI
    {
        get { return multipleChoiceUI; }
        set { multipleChoiceUI = value; }
    }

    //Scatter Plot Part1 UI used
    [SerializeField]
    private GameObject scatterPlotPart1UI;
    public GameObject ScatterPlotPart1UI
    {
        get { return scatterPlotPart1UI; }
        set { scatterPlotPart1UI = value; }
    }

    //Scatter Plot Part2 UI used
    [SerializeField]
    private GameObject scatterPlotPart2UI;
    public GameObject ScatterPlotPart2UI
    {
        get { return scatterPlotPart2UI; }
        set { scatterPlotPart2UI = value; }
    }

    //Fact UI used
    [SerializeField]
    private GameObject factUI;
    public GameObject FactUI
    {
        get { return factUI; }
        set { factUI = value; }
    }

    //Logo UI used
    [SerializeField]
    private GameObject logoUI;
    public GameObject LogoUI
    {
        get { return logoUI; }
        set { logoUI = value; }
    }

    //Ambient tigger area
    [SerializeField, Header("Trigger Areas:")]
    private GameObject ambientTrigger;
    public GameObject AmbientTrigger
    {
        get { return ambientTrigger; }
        set { ambientTrigger = value; }
    }

    //Poll tigger area
    [SerializeField]
    private GameObject pollTrigger;
    public GameObject PollTrigger
    {
        get { return pollTrigger; }
        set { pollTrigger = value; }
    }

    //Multiple choice tigger area
    [SerializeField]
    private GameObject multipleChoiceTrigger;
    public GameObject MultipleChoiceTrigger
    {
        get { return multipleChoiceTrigger; }
        set { multipleChoiceTrigger = value; }
    }

    //Scatter plot tigger area
    [SerializeField]
    private GameObject scatterPlotTrigger;
    public GameObject ScatterPlotTrigger
    {
        get { return scatterPlotTrigger; }
        set { scatterPlotTrigger = value; }
    }

    //Physics used for the poll groups
    [SerializeField, Header("CGF Areas:")]
    private List<CircularGravity> pollCgfs;
    public List<CircularGravity> PollCgfs
    {
        get { return pollCgfs; }
        set { pollCgfs = value; }
    }

    //Physics used for the multiple choice groups
    [SerializeField]
    private List<CircularGravity> multipleChoiceCgfs;
    public List<CircularGravity> MultipleChoiceCgfs
    {
        get { return multipleChoiceCgfs; }
        set { multipleChoiceCgfs = value; }
    }

    //Physics used for the scatter Plot groups
    [SerializeField]
    private List<CircularGravity> scatterPlotCgfs;
    public List<CircularGravity> ScatterPlotCgfs
    {
        get { return scatterPlotCgfs; }
        set { scatterPlotCgfs = value; }
    }

    //Poll particl effects used
    [SerializeField, Header("Particle Areas:")]
    private List<PlaygroundParticlesC> pollParticles;
    public List<PlaygroundParticlesC> PollParticles
    {
        get { return pollParticles; }
        set { pollParticles = value; }
    }

    //Multiple choice particl effects used
    [SerializeField]
    private List<PlaygroundParticlesC> multipleChoiceParticles;
    public List<PlaygroundParticlesC> MultipleChoiceParticles
    {
        get { return multipleChoiceParticles; }
        set { multipleChoiceParticles = value; }
    }

    //Start vote timer
    [SerializeField, Header("Voting Info: ")]
    private float startVotingTime = 10f;
    public float StartVotingTime
    {
        get { return startVotingTime; }
        set { startVotingTime = value; }
    }

    //Vote timer
    [SerializeField]
    private float voteTimer = 10f;
    public float VoteTimer
    {
        get { return voteTimer; }
        set { voteTimer = value; }
    }
    
    //Voting state
    [SerializeField, Header("Static Voting Info: ")]
    private VoteState voteState = VoteState.None;
    public VoteState _voteState
    {
        get { return voteState; }
        set
        {
            voteCountdown = VoteTimer;
            voteTimerStamp = Time.time;

            voteState = value;
        }
    }

    //Used for the vote countdown
    [SerializeField]
    private float voteCountdown;
    public float VoteCountDown
    {
        get { return voteCountdown; }
        set { voteCountdown = value; }
    }

    //Answer 1 Vote
    [SerializeField]
    private int answer1Vote = 0;
    public int Answer1Vote
    {
        get { return answer1Vote; }
        set { answer1Vote = value; }
    }

    //Answer 2 Vote
    [SerializeField]
    private int answer2Vote = 0;
    public int Answer2Vote
    {
        get { return answer2Vote; }
        set { answer2Vote = value; }
    }

    //Answer 3 Vote
    [SerializeField]
    private int answer3Vote = 0;
    public int Answer3Vote
    {
        get { return answer3Vote; }
        set { answer3Vote = value; }
    }

    //Answer Scatter Plot Vote
    [SerializeField]
    private Vector2 answerScatterPlotVote = Vector2.zero;
    public Vector2 AnswerScatterPlotVote
    {
        get { return answerScatterPlotVote; }
        set { answerScatterPlotVote = value; }
    }

    //Vote time stamp
    private float voteTimerStamp = 0.0f;

    //Use this for initialization
    public void Start()
    {
        AmbientUI.SetActive(true);
        PollUI.SetActive(true);
        MultipleChoiceUI.SetActive(true);
        ScatterPlotPart1UI.SetActive(true);
        ScatterPlotPart2UI.SetActive(true);
        FactUI.SetActive(true);
        LogoUI.SetActive(true);
    }

    // Update is called once per frame
    public void Update()
    {
        float total = 0;

        switch (_questionState)
        {
            case QuestionState.Poll:
                
                total = (Answer1Records + Answer2Records) + (Answer1Vote + Answer2Vote);

                if (Core.Instance._spawnManager.TimeDelaySeconds < Core.Instance._spawnManager.TimeDelayCountDown)
                {
                    Answer1Percent = GetTotalPercent(total, "Answer 1", Answer1Vote);
                    Answer2Percent = GetTotalPercent(total, "Answer 2", Answer2Vote);
                }
                else
                {
                    Answer1Percent = 0.0f;
                    Answer2Percent = 0.0f;
                }

                break;
            case QuestionState.MultipleChoice:
                if (Core.Instance._spawnManager.TimeDelaySeconds < Core.Instance._spawnManager.TimeDelayCountDown)
                    total = (Answer1Records + Answer2Records + Answer3Records) + (Answer1Vote + Answer2Vote + Answer3Vote);
                else
                    total = 0f;

                if (Core.Instance._spawnManager.TimeDelaySeconds < Core.Instance._spawnManager.TimeDelayCountDown)
                {
                    Answer1Percent = GetTotalPercent(total, "Answer 1", Answer1Vote);
                    Answer2Percent = GetTotalPercent(total, "Answer 2", Answer2Vote);
                    Answer3Percent = GetTotalPercent(total, "Answer 3", Answer3Vote);
                }
                else
                {
                    Answer1Percent = 0.0f;
                    Answer2Percent = 0.0f;
                    Answer3Percent = 0.0f;
                }

                break;
        }
    }

    //Gets a total Percent of object in scene using the tag
    public float GetTotalPercent(float total, string tag, float offset)
    {
        var list =
        from g in Core.Instance._spawnManager.DefaultCubes.SpawnPool
        where
            g.activeInHierarchy == true &&
            g.tag == tag
        select g;

        float totalSpawnedItems = (float)list.Count();

        return Mathf.Round(((totalSpawnedItems + offset) / total) * 100f);
    }

    //Syncs the player voting
    public void SyncVoting()
    {
        var list =
        from g in Core.Instance._playerManager.Players
        where
            g.GetComponent<Player_Old>().Mode == Player_Old.PlayerMode.Engaged &&
            g.GetComponent<Player_Old>().ModeTime > StartVotingTime
        select g;

        if (list.Count() > 0)
        {
            if (_voteState == VoteState.Waiting)
            {
                _voteState = VoteState.Voting;
            }
        }

        if (_voteState == VoteState.Voting)
        {
            if (voteCountdown >= 0f)
            {
                voteCountdown = VoteTimer - (Time.time - voteTimerStamp);
            }
            else if (voteCountdown <= 0f)
            {
                voteCountdown = VoteTimer;

                _voteState = VoteState.Voted;
            }
        }
    }

    //Gathers the poll and multiple choice votes
    public void GatherPollAndMuliVotes()
    {
        string saveValue = string.Empty;

        var list =
        from g in Core.Instance._playerManager.Players
        where
            g.GetComponent<Player_Old>().Mode == Player_Old.PlayerMode.Engaged
        select g;

        foreach (var player in list)
        {
            if (player.GetComponent<Player_Old>().PlayerAnswer == 1)
            {
                //Set Tag
                player.GetComponent<Player_Old>().ActivateCubeVote(false, "Answer 1");
                
                //Count Vote
                Core.Instance._questionManager.Answer1Vote++;

                //Save Vote
                StartCoroutine(Core.Instance._httpRequest.SaveVote(Core.Instance._questionManager.QuestionTypeId, Core.Instance._questionManager.questionId, "0.0", "0.0"));
            }
            else if (player.GetComponent<Player_Old>().PlayerAnswer == 2)
            {
                //Set Tag
                player.GetComponent<Player_Old>().ActivateCubeVote(false, "Answer 2");

                //Count Vote
                Core.Instance._questionManager.Answer2Vote++;

                //Save Vote
                switch (Core.Instance._questionManager._questionState)
                {
                    case QuestionState.Poll:
                        saveValue = "1.0";
                        break;
                    case QuestionState.MultipleChoice:
                        saveValue = "0.5";
                        break;
                }
                StartCoroutine(Core.Instance._httpRequest.SaveVote(Core.Instance._questionManager.QuestionTypeId, Core.Instance._questionManager.questionId, saveValue, "0.0"));
            }
            else if (player.GetComponent<Player_Old>().PlayerAnswer == 3)
            {
                //Set Tag
                player.GetComponent<Player_Old>().ActivateCubeVote(false, "Answer 3");

                //Count Vote
                Core.Instance._questionManager.Answer3Vote++;

                //Save Vote
                StartCoroutine(Core.Instance._httpRequest.SaveVote(Core.Instance._questionManager.QuestionTypeId, Core.Instance._questionManager.questionId, "1.0", "0.0"));
            }
        }
    }

    //Gathers the scatter plot part1 and part2 choice votes
    public void GatherScatterPlotVote(bool part2 = false)
    {
        switch (questionState)
        {
            case QuestionManager.QuestionState.ScatterPlot:

                //Core.Instance._playerManager.CubiePrefab.GetComponent<Cubie>().EnableScatter = true;

                if(!part2)
                {
                    Core.Instance._questionManager.AnswerScatterPlotVote = new Vector3(Core.Instance._playerManager.AvgScatterValue, 0);
                }
                else
                {
                    Core.Instance._questionManager.AnswerScatterPlotVote = new Vector2(Core.Instance._questionManager.AnswerScatterPlotVote.x, Core.Instance._playerManager.AvgScatterValue);

                    //Save Vote
                    StartCoroutine(Core.Instance._httpRequest.SaveVote(Core.Instance._questionManager.QuestionTypeId, Core.Instance._questionManager.questionId, Core.Instance._questionManager.AnswerScatterPlotVote.x.ToString(), Core.Instance._questionManager.AnswerScatterPlotVote.y.ToString()));
                }
                

                break;
        }
    }

    //Enables trigger areas based on the question state
    public void EnableTriggerAreas(QuestionManager.QuestionState questionState)
    {
        switch (questionState)
        {
            case QuestionManager.QuestionState.None:
                
                //Trigger Areas
                AmbientTrigger.SetActive(true);
                PollTrigger.SetActive(false);
                MultipleChoiceTrigger.SetActive(false);
                ScatterPlotTrigger.SetActive(false);

                //CGF Areas
                foreach (var cgf in PollCgfs)
                {
                    cgf.Enable = false;
                }
                foreach (var cgf in MultipleChoiceCgfs)
                {
                    cgf.Enable = false;
                }

                break;
            case QuestionManager.QuestionState.Poll:

                //Trigger Areas
                AmbientTrigger.SetActive(false);
                PollTrigger.SetActive(true);
                MultipleChoiceTrigger.SetActive(false);
                ScatterPlotTrigger.SetActive(false);

                //CGF Areas
                foreach (var cgf in PollCgfs)
                {
                    cgf.Enable = true;
                }
                foreach (var cgf in MultipleChoiceCgfs)
                {
                    cgf.Enable = false;
                }

                break;
            case QuestionManager.QuestionState.MultipleChoice:

                //Trigger Areas
                AmbientTrigger.SetActive(false);
                PollTrigger.SetActive(false);
                MultipleChoiceTrigger.SetActive(true);
                ScatterPlotTrigger.SetActive(false);

                //CGF Areas
                foreach (var cgf in PollCgfs)
                {
                    cgf.Enable = false;
                }
                foreach (var cgf in MultipleChoiceCgfs)
                {
                    cgf.Enable = true;
                }

                break;
            case QuestionManager.QuestionState.ScatterPlot:

                //Trigger Areas
                AmbientTrigger.SetActive(false);
                PollTrigger.SetActive(false);
                MultipleChoiceTrigger.SetActive(false);
                ScatterPlotTrigger.SetActive(true);

                //CGF Areas
                foreach (var cgf in PollCgfs)
                {
                    cgf.Enable = false;
                }
                foreach (var cgf in MultipleChoiceCgfs)
                {
                    cgf.Enable = false;
                }

                break;
        }
    }

    //Enables physics areas based off the question state
    public void EnableCGFAreas(QuestionManager.QuestionState questionState, bool positiveForce = false)
    {
        switch (questionState)
        {
            case QuestionManager.QuestionState.None:

                //CGF Areas
                foreach (var cgf in PollCgfs)
                {
                    cgf.Enable = false;

                    if (positiveForce)
                        cgf.ForcePower = Mathf.Abs(cgf.ForcePower);
                    else
                        cgf.ForcePower = -Mathf.Abs(cgf.ForcePower);
                }
                foreach (var cgf in MultipleChoiceCgfs)
                {
                    cgf.Enable = false;

                    if (positiveForce)
                        cgf.ForcePower = Mathf.Abs(cgf.ForcePower);
                    else
                        cgf.ForcePower = -Mathf.Abs(cgf.ForcePower);
                }

                break;
            case QuestionManager.QuestionState.Poll:

                //CGF Areas
                foreach (var cgf in PollCgfs)
                {
                    cgf.Enable = true;

                    if (positiveForce)
                        cgf.ForcePower = Mathf.Abs(cgf.ForcePower);
                    else
                        cgf.ForcePower = -Mathf.Abs(cgf.ForcePower);
                }
                foreach (var cgf in MultipleChoiceCgfs)
                {
                    cgf.Enable = false;

                    if (positiveForce)
                        cgf.ForcePower = Mathf.Abs(cgf.ForcePower);
                    else
                        cgf.ForcePower = -Mathf.Abs(cgf.ForcePower);
                }

                break;
            case QuestionManager.QuestionState.MultipleChoice:

                //CGF Areas
                foreach (var cgf in PollCgfs)
                {
                    cgf.Enable = false;

                    if (positiveForce)
                        cgf.ForcePower = Mathf.Abs(cgf.ForcePower);
                    else
                        cgf.ForcePower = -Mathf.Abs(cgf.ForcePower);
                }
                foreach (var cgf in MultipleChoiceCgfs)
                {
                    cgf.Enable = true;

                    if (positiveForce)
                        cgf.ForcePower = Mathf.Abs(cgf.ForcePower);
                    else
                        cgf.ForcePower = -Mathf.Abs(cgf.ForcePower);
                }

                break;
            case QuestionManager.QuestionState.ScatterPlot:

                //CGF Areas
                foreach (var cgf in PollCgfs)
                {
                    cgf.Enable = false;

                    if (positiveForce)
                        cgf.ForcePower = Mathf.Abs(cgf.ForcePower);
                    else
                        cgf.ForcePower = -Mathf.Abs(cgf.ForcePower);
                }

                foreach (var cgf in MultipleChoiceCgfs)
                {
                    cgf.Enable = false;

                    if (positiveForce)
                        cgf.ForcePower = Mathf.Abs(cgf.ForcePower);
                    else
                        cgf.ForcePower = -Mathf.Abs(cgf.ForcePower);
                }

                break;
        }
    }

    //Enables physics scatter plot areas
    public void EnableCGFScatterAreas(bool enable)
    {
        if (enable)
        {
            var list =
            from g in Core.Instance._spawnManager.DefaultCubes.SpawnPool
            where g.activeInHierarchy == true && g.GetComponent<CubeDefault>().QuestionState == QuestionState.ScatterPlot
            select g;

            foreach (var item in list)
            {
                item.GetComponent<CubeDefault>().QuestionState = QuestionState.None;
            }

            //Core.Instance._playerManager.CubiePrefab.GetComponent<Cubie>().tag = "Scatter";
            //Core.Instance._playerManager.CubiePrefab.GetComponent<Cubie>().Target = Vector3.zero;
            //Core.Instance._playerManager.CubiePrefab.GetComponent<Cubie>().GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        foreach (var cgf in ScatterPlotCgfs)
        {
            cgf.Enable = enable;
        }
    }
}
