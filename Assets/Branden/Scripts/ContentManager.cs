/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used to query from the cms.
*******************************************************************************************/
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

//For SQLite
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;

public class ContentManager : MonoBehaviour {
    public class AnswerInfo {
        public int uniqueId;
        public string questionId;
    }

    public class AOrBAnswerInfo : AnswerInfo {
        public int answerA = 1;
        public int answerB = 1;
    }

    public class GraphAnswerInfo : AnswerInfo {
        //For bar graph...
        public List<int> numbers = new List<int>();
    }

    static ContentManager m_instance = null;
    public static ContentManager Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<ContentManager>();
            }
            return m_instance;
        }
    }

    //SQLite Connection Properties
    private string connectionString = string.Empty;
    private IDbConnection dbcon;
    private IDbCommand dbcmd;
    private IDataReader reader;
    private bool sqliteconnected = false;

    public List<int> scatter_plot_answer_list;

    public class PollQuestion {
        public string Question_Id { get; private set; }

        public string Question { get; private set; }

        public string Answer_a { get; private set; }

        public string Answer_b { get; private set; }

        public string QuestionType { get; private set; }

        //public int Age { get; private set; }

        public PollQuestion(string question_id, string question, string answer_a, string answer_b, string question_type_id) {
            Question_Id = question_id;

            Question = question;

            Answer_a = answer_a;

            Answer_b = answer_b;

            QuestionType = question_type_id;
        }
        //Other properties, methods, events...
    }

    [NonSerialized]
    public List<PollQuestion> PollAList;
    public List<PollQuestion> PollBList;

    public string background_path;

    public void GetGlobalSettings() {
        string query = "select * from globalsetting";

        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();

        while (reader.Read()) {
            GlobalSettingText.Messages[0] = reader["dwelltimectaheadline"].ToString();
            GlobalSettingText.Messages[1] = reader["dwelltimectadescription"].ToString();
            GlobalSettingText.Messages[2] = reader["countdownctaheadline"].ToString();
            GlobalSettingText.Messages[3] = reader["countdownctadescription"].ToString();
            GlobalSettingText.Messages[4] = reader["cubeinstructionsanswer"].ToString();
            GlobalSettingText.Messages[5] = reader["cubeinstructionscontinue"].ToString();
            Debug.LogWarning("Loaded Global Settings...");
        }
    }

    public void GetBackground() {
        string query = "select * from Resources INNER JOIN ambient ON ambient.image = Resources.resourcesid";
        string path;

        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();

        while (reader.Read()) {
            path = "";

            path = reader["resourcespath"].ToString() + reader["resourcesfilename"].ToString();

            path = string.Format("{0}{1}{2}", "file://", StartupSettings.Instance.Resource_location, path);

            Debug.Log("BACKGROUND " + path);
            background_path = path;
        }
    }

    public void GetActivePlaylist() {
        string query = "select q.id, q.question, q.question_type_id, q.answer_text_a, " +
            "q.answer_text_b, q.active_flag, pq.playlist_id from question q " +
            "INNER JOIN playlistquestion pq ON q.id = pq.question_id " +
            "INNER JOIN playlist p ON p.id = pq.playlist_id " +
            "where q.active_flag is 1 AND p.active_flag is 1";

        GetPollRecords(query);
    }

    public void GetPollRecords(string query) {
        PollAList = new List<PollQuestion>();
        PollBList = new List<PollQuestion>();

        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();

        while (reader.Read()) {
            Debug.Log("****** Name: " + reader["answer_text_a"] + "\tScore: " + reader["answer_text_b"]);

            string questionType = reader["question_type_id"].ToString();
            if (questionType == "1") {
                PollAList.Add(new PollQuestion(reader["id"].ToString(), reader["question"].ToString(),
                    reader["answer_text_a"].ToString(), reader["answer_text_b"].ToString(), questionType));
            }
            else {
                PollBList.Add(new PollQuestion(reader["id"].ToString(), reader["question"].ToString(),
                    reader["answer_text_a"].ToString(), reader["answer_text_b"].ToString(), questionType));
            }
        }
    }

    public AnswerInfo GetPollAnswersA(string questionId) {
        string query = "select * from answer " +
            "where question_id is " + questionId + " " +
            "ORDER BY updated_at DESC Limit 1";

        AOrBAnswerInfo answerInfo = new AOrBAnswerInfo();
        answerInfo.questionId = questionId;

        try {

            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();

            object ans1Obj = null;
            object ans2Obj = null;

            while (reader.Read()) {
                answerInfo.uniqueId = Convert.ToInt32(reader["id"]);
                ans1Obj = reader["answer_text_a"].ToString();
                ans2Obj = reader["answer_text_b"].ToString();
            }

            string ans1 = ans1Obj.ToString();
            string ans2 = ans2Obj.ToString();
            answerInfo.answerA = Convert.ToInt32(string.IsNullOrEmpty(ans1) ? "1" : ans1);
            answerInfo.answerB = Convert.ToInt32(string.IsNullOrEmpty(ans2) ? "1" : ans2);
        }
        catch {
            //TODO: DONT HARDCODE THIS
            Debug.Log("ERROR RETRIEVING DATA");
        }

        Debug.Log(answerInfo.answerA + " " + answerInfo.answerA);
        return answerInfo;
    }

    public AnswerInfo GetPollAnswersB(string questionId) {
        GraphAnswerInfo answerInfo = new GraphAnswerInfo();
        answerInfo.questionId = questionId;

        Debug.Log("RETRIEVING TYPE 2 ANSWERS FROM " + questionId);

        //List<int> numlist = new List<int>();

        string numbers = "[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]";

        try {
            string query = "select id, scatter_plot_answer, updated_at from answer " +
                "where question_id is " + answerInfo.questionId + " " +
                "ORDER BY updated_at DESC Limit 1";

            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();

            while (reader.Read()) {
                string answers = reader["scatter_plot_answer"].ToString();
                answerInfo.uniqueId = Convert.ToInt32(reader["id"]);
                if (!string.IsNullOrEmpty(answers)) {
                    numbers = answers;
                }
            }

            //REMOVES [ ]
            numbers = numbers.Substring(1, numbers.Length - 2);

            foreach (string number in numbers.Split(',')) {
                answerInfo.numbers.Add(Int32.Parse(number));
            }

            while (answerInfo.numbers.Count < 40) {
                answerInfo.numbers.Add(0);
            }
        }
        catch {
            //NEW USER - NO DATA AVAILABLE

            Debug.Log("FAILED TO PULL DATA - USER - NO DATA AVAILABLE");

            List<int> numlist_error = new List<int>();
            string numbers_error = "[1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1]";

            //REMOVES [ ]
            numbers_error = numbers_error.Substring(1, numbers_error.Length - 2);

            foreach (string num in numbers_error.Split(','))
                numlist_error.Add(Int32.Parse(num));

            answerInfo.numbers = numlist_error;
        }

        //scatter_plot_answer_arr = arr;
        return answerInfo;
    }

    public void PutPollAnswers(AOrBAnswerInfo answerInfo) {
        StartCoroutine(HTTPRequest.Instance.SaveVote(answerInfo));
    }

    public void PutPollAnswers(GraphAnswerInfo answerInfo) {
        StartCoroutine(HTTPRequest.Instance.SaveVote(answerInfo));
    }

    public string GetSearchQueryFromQuestion(string questionId) {
        string searchQuery = null;

        List<string> handles = new List<string>();
        List<string> hashtags = new List<string>();

        try {
            //int counter = 0;
            string query = "select * from twitteruser " +
                "INNER JOIN questiontwitteruser ON questiontwitteruser.twitter_user_id = twitteruser.id " +
                "where question_id is " + questionId;

            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();

            while (reader.Read()) {
                Debug.Log("****** Handles: " + reader["handle"]);
                handles.Add(reader["handle"].ToString());
            }
        }
        catch {
            Debug.Log("Error Accessing Twitter Data From Database");
        }

        try {
            //int counter = 0;
            string query = "select t.id, t.hashtag from twitterhashtag t " +
                "INNER JOIN questiontwitterhashtag q ON t.id = q.twitter_hashtag_id " +
                "where q.question_id is " + questionId;

            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();

            while (reader.Read()) {
                Debug.Log("****** Hashtags: " + reader["hashtag"]);
                hashtags.Add(reader["hashtag"].ToString());
            }
        }
        catch {
            Debug.Log("Error Accessing Twitter Data From Database");
        }

        searchQuery = GetSearchQueryFromKeywords(hashtags.ToArray()) +
            " " + GetSearchQueryFromKeywords(handles.ToArray());

        return searchQuery;
    }

    //Pass handles or hashtags, but not both...
    public string GetSearchQueryFromKeywords(params string[] keywords) {
        string searchQuery = "";

        foreach (string keyword in keywords) {
            searchQuery += Regex.Replace(keyword, @"\s+", "") + " OR ";
        }

        //Removing last OR...
        if (searchQuery.Length > 0) {
            searchQuery = searchQuery.Substring(0, searchQuery.Length - 4);
        }
        else {
            searchQuery += "@oracle";
        }

        searchQuery = searchQuery.Replace("@", "from:");

        return searchQuery;
    }

    public string GetSearchQueryForAmbientHandles() {
        string searchQuery = null;

        List<string> handles = new List<string>();

        try {
            //int counter = 0;
            string query = "select * from ambienthandle";

            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();

            while (reader.Read()) {
                Debug.Log("****** Handles: " + reader["handle"]);
                handles.Add(reader["handle"].ToString());
            }
        }
        catch {
            Debug.Log("Error Accessing Twitter Data From Database");
        }

        searchQuery = GetSearchQueryFromKeywords(handles.ToArray());

        return searchQuery;
    }

    //Opens the sql connection
    public void OpenConnection() {
        StartupSettings settings = StartupSettings.Instance;

        string fileLocation = Environment.ExpandEnvironmentVariables(string.Format("{0}{1}",
            settings.Resource_location, settings.Resource_db));

        if (File.Exists(fileLocation)) {
            connectionString = string.Format("URI=file:{0}", fileLocation);

            dbcon = (IDbConnection)new SqliteConnection(connectionString);
            dbcon.Open();

            dbcmd = dbcon.CreateCommand();

            sqliteconnected = true;

            Debug.LogWarning("Open SQLite Connection");
        }
        else {
            sqliteconnected = false;
            Debug.LogWarning("Can't Find '" + fileLocation + "'");
        }
    }

    //Closes the sql connection
    public void CloseConnection() {
        if (sqliteconnected) {
            if (reader != null) {
                reader.Close();
                reader = null;
            }

            dbcmd.Dispose();
            dbcmd = null;
            dbcon.Close();
            dbcon = null;

            Debug.LogWarning("Close SQLite Connection");
        }
    }
}