/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used to bind the CMS data to the innocation wall project.
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Globalization;

public class CMS_Controler : MonoBehaviour
{
    //Question table id
    public const int Question_Table = 0;
    //Question table column ids
    public enum Question_Columns
    {
        QuestionType = 0,
        Question1 = 1,
        Question2 = 2,
        Answer1 = 3,
        Answer2 = 4,
        Answer3 = 5,
        Answer4 = 6,
        //Fact = 7,
        //FileName = 8,
        //QuestionTypeId = 9,
        //QuestionId = 10,
        //Proposition = 11,
    }

    //Result table id
    public const int Result_Table = 1;
    //Result table column ids
    public enum Result_Columns
    {
        ValueA = 0,
        ValueB = 1,
    }

    //Current question index
    [SerializeField]
    private int questionIndex = 0;
    public int QuestionIndex
    {
        get { return questionIndex; }
        set { questionIndex = value; }
    }

    private CMS cms;

    //Used for the facts picture
    private Texture2D _prevFactTexture2D;
    private Texture2D _factTexture2D;
    private Sprite _prevFactSprite;
    private Sprite _factSprite;


    //Use this for initialization
    public void Start()
    {
        //Gets a the cms ref
        cms = this.GetComponent<CMS>();

        //Gets all question records
        cms.OpenConnection();

        MainController.Instance.Loader.text = "OPEN DB CONNECTION";

        cms.GetAllPollRecords();

        MainController.Instance.Loader.text = "GATHER POLL RECORDS";

        cms.GetAllTwitterRecords();

        MainController.Instance.Loader.text = "GATHER TWITTER RECORDS";

        cms.GetBackground();

        cms.CloseConnection();

        MainController.Instance.Loader.text = "CLOSE CONNECTION";

        SetupTwitterCubes();

    }

    public void SetupTwitterCubes()
    {

        string search_me = Core.Instance._cms.RequestTwitterRecords("all");

        MainController.Instance.StartTwitterCubes(search_me);

        //Debug.Log("CHANGE THIS BACK");
        //MainController.Instance.promptPollSection();
    }

    //Loads the question fact image
    //public void LoadQuestionMedia()
    //{
        //Core.Instance._questionManager.FactURL = string.Format("{0}{1}{2}{3}", "file://", Core.Instance._settings.Resource_location, Core.Instance._settings.Resource_media, cms.GetRecord(Question_Table, (int)Question_Columns.FileName, QuestionIndex));
        //LoadFactImage();
    //}
}
