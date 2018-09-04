/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used to query from the cms.
*******************************************************************************************/
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

//For SQLite
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;
using System.Text.RegularExpressions;

public class CMS : MonoBehaviour
{
   

    //SQLite Connection Properties
    private string connectionString = string.Empty;
    private IDbConnection dbcon;
    private IDbCommand dbcmd;
    private IDataReader reader;
    private bool sqliteconnected = false;

    public List<int> scatter_plot_answer_list;

    public class PollQuestion
    {
        public string Question_Id { get; private set; }

        public string Question { get; private set; }

        public string Answer_a { get; private set; }
        
        public string Answer_b { get; private set; }

        public string QuestionType { get; private set; }
        
        //public int Age { get; private set; }

        public PollQuestion(string question_id, string question, string answer_a, string answer_b, string question_type_id)
        {
            Question_Id = question_id;

            Question = question;

            Answer_a = answer_a;

            Answer_b = answer_b;

            QuestionType = question_type_id;
        }
        //Other properties, methods, events...
    }


    public class TwitterRecord
    {
        public string Id { get; private set; }

        public string Handle { get; private set; }

        //public int Age { get; private set; }

        public TwitterRecord(string id, string handle)
        {
            Id = id;

            Handle = handle;

        }
        //Other properties, methods, events...
    }

    [NonSerialized]
    public List<PollQuestion> PollList;
    public List<TwitterRecord> TwitterList;

    public string background_path;

    public void GetBackground()
    {
        string query = "select * from Resources INNER JOIN ambient ON ambient.image = Resources.resourcesid";
        string path;

        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            path = "";

            path = reader["resourcespath"].ToString() + reader["resourcesfilename"].ToString();

            path = string.Format("{0}{1}{2}", "file://", Core.Instance._settings.Resource_location, path);

            Debug.Log("BACKGROUND " + path);
            background_path = path;
        }

    }

    public void GetAllPollRecords()
    {

        string query = "select * from question where active_flag is 1";

        PollList = new List<PollQuestion>();

        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            Debug.Log("****** Name: " + reader["answer_text_a"] + "\tScore: " + reader["answer_text_b"]);
            PollList.Add(new PollQuestion(reader["id"].ToString(), reader["question"].ToString(), reader["answer_text_a"].ToString(), reader["answer_text_b"].ToString(), reader["question_type_id"].ToString()));
        }
        
    }

    public string GetTakeAway(string id)
    {
        List<string> twit_list = new List<string>();

        //string query = "select twitteruserid from questiontwitteruser where question_id is " + id.ToString();

        string query = "select * from twitteruser INNER JOIN questiontwitteruser ON questiontwitteruser.twitter_user_id = twitteruser.id where question_id is " + id;

        try
        {
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();

            while (reader.Read())
            {

                Debug.Log("****** Name: " + reader["handle"]);// + "\tScore: " + reader["answer_text_b"]);
                twit_list.Add(reader["handle"].ToString());

            }
        }
        catch
        {
            Debug.LogWarning("no data exists for this question ");
        }

        return ConvertTwitterToString(twit_list);
    }

    public string ConvertTwitterToString(List<string> TwitterList)
    {

        string records = "";

        foreach (string tweet_search in TwitterList)
        {
            records = records + tweet_search + " OR ";
        }

        if (records.Length > 0) { 
            records = records.Substring(0, records.Length - 4);
        }
        else
        {
            records = "@oracle";
        }

        records = records.Replace("@", "from:");

        Debug.Log("TAKEAWAY " + records);

        return records;

    }

    public int[] GetPollAnswersA(string id)
    {
        Debug.Log(id);

        string query = "select * from answer where question_id is " + id;

        Debug.Log(query);

        int[] arr = new int[2];
        arr[0] = 1;
        arr[1] = 1;

        try { 

            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();

            while (reader.Read())
            {
                Debug.Log("****** Name: " + reader["answer_text_a"] + "\tScore: " + reader["answer_text_b"]);
                string ans1 = reader["answer_text_a"].ToString();
                string ans2 = reader["answer_text_b"].ToString();

                Debug.Log(ans1 + " " + ans2);

                arr[0] = System.Convert.ToInt32(string.IsNullOrEmpty(ans1) ? "1" : ans1);
                arr[1] = System.Convert.ToInt32(string.IsNullOrEmpty(ans2) ? "1" : ans2);
            }

        }
        catch {
            //TODO: DONT HARDCODE THIS
            Debug.Log("ERROR RETRIEVING DATA");

        }

        Debug.Log(arr[0] + " " + arr[1]);
        return arr;

    }

    public List<int> GetPollAnswersB(string id)
    {
        string[] arr = new string[40];

        Debug.Log("RETRIEVING TYPE 2 ANSWERS FROM " + id);

        List<int> numlist = new List<int>();

        string numbers = "[1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1]";

        try { 

            string query = "select scatter_plot_answer from answer where question_id is " + id;
            

            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();

            while (reader.Read())
            {

                numbers = reader["scatter_plot_answer"].ToString();

            }

            //REMOVES [ ]
            numbers = numbers.Substring(1, numbers.Length - 2);

            foreach (string number in numbers.Split(','))
                numlist.Add(Int32.Parse(number));

            while (numlist.Count < 40)
            {
                numlist.Add(1);
            }

        }
        catch
        {
            //NEW USER - NO DATA AVAILABLE

            Debug.Log("FAILED TO PULL DATA - USER - NO DATA AVAILABLE");

            List<int> numlist_error = new List<int>();
            string numbers_error = "[1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1]";

            //REMOVES [ ]
            numbers_error = numbers_error.Substring(1, numbers_error.Length - 2);

            foreach (string num in numbers_error.Split(','))
                numlist_error.Add(Int32.Parse(num));

            numlist = numlist_error;

        }

        //scatter_plot_answer_arr = arr;

        return numlist;

    }

    public void PutPollAnswersA(string id, float[] arr)
    {

        Debug.Log("PUTTING POLL A " + arr[0].ToString() + " " + arr[1].ToString());
        StartCoroutine(Core.Instance._httpRequest.SaveVote(id, arr[0].ToString(), arr[1].ToString(), ""));

    }

    public void PutPollAnswersB(string id, List<int> arr)
    {

        string put = "[";

        for(int i=0; i<arr.Count; i++)
        {
            if(i == arr.Count - 1)
                put += arr[i].ToString() + "]";
            else
                put += arr[i].ToString() + ",";
        }

        Debug.Log("PUT TYPE B " + put);

        StartCoroutine(Core.Instance._httpRequest.SaveVote(id, "", "", put));

    }

    public void GetAllTwitterRecords() {
        try {
            //int counter = 0;
            string query = "select * from ambienthandle";
            TwitterList = new List<TwitterRecord>();

            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();

            while (reader.Read()) {
                Debug.Log("****** Name: " + reader["id"] + "\tScore: " + reader["handle"]);
                TwitterList.Add(new TwitterRecord(reader["id"].ToString(), reader["handle"].ToString()));
                //counter++;
            }
        }
        catch {
            Debug.Log("Error Accessing Twitter Data From Database");
        }

    }

    public string RequestTwitterRecords(string id)
    {
        //pass in string "all" or "99" to get all the records
        string records = "";


        foreach (TwitterRecord tweet_search in TwitterList )
        {
            //Debug.Log("tweet search " + tweet_search.Id + " " + id);
            if (tweet_search.Id == id)
            {
                //Debug.Log("MY ID " + tweet_search.Id);
                records = records + tweet_search.Handle + " OR ";
            }
            else
            {
                records = records + tweet_search.Handle + " OR ";
            }
        }

        //IF WE HAVE RESULTS - THEN REMOVE THE LAST OR
        if(records.Length > 0)
            records = records.Substring(0, records.Length - 4);
        //records = records + " -RT";

        records = records.Replace("@", "from:");

        Debug.Log("Records " + records);

        return records;
    }


    //Opens the sql connection
    public void OpenConnection()
    {
        string fileLocation = Environment.ExpandEnvironmentVariables(string.Format("{0}{1}", GameObject.Find("_Core").GetComponent<Core>()._settings.Resource_location, GameObject.Find("_Core").GetComponent<Core>()._settings.Resource_db));

        if (File.Exists(fileLocation))
        {
            connectionString = string.Format("URI=file:{0}", fileLocation);

            dbcon = (IDbConnection)new SqliteConnection(connectionString);
            dbcon.Open();

            dbcmd = dbcon.CreateCommand();

            sqliteconnected = true;

            Core.Instance.SaveOutputLine(Core.DebugType.Log, string.Format("Open SQLite Connection"));
        }
        else
        {
            sqliteconnected = false;

            Core.Instance.SaveOutputLine(Core.DebugType.Error, string.Format("Can't Find '{0}'", fileLocation));
        }
    }

    //Closes the sql connection
    public void CloseConnection()
    {
        if (sqliteconnected)
        {
            if (reader != null)
            {
                reader.Close();
                reader = null;
            }

            dbcmd.Dispose();
            dbcmd = null;
            dbcon.Close();
            dbcon = null;

            Core.Instance.SaveOutputLine(Core.DebugType.Log, string.Format("Close SQLite Connection"));
        }
    }

    //Generates the querys being used
    /*public string GenerateQuery(int index)
    {
        string tableColumns = string.Empty;
        for (int i = 0; i < Tables[index].Columns.Count; i++)
        {
            if (i == 0)
            {
                tableColumns = string.Format("{0}", Tables[index].Columns[i].Name);
            }
            else
            {
                tableColumns = string.Format("{0},{1}", tableColumns, Tables[index].Columns[i].Name);
            }
        }

        string query = string.Format("SELECT {0} FROM {1}", tableColumns, Tables[index].Name);

        if (Tables[index].Join != string.Empty)
        {
            query = string.Format("{0}\n{1}", query, Tables[index].Join);
        }
        if (Tables[index].Where != string.Empty)
        {
            query = string.Format("{0}\n{1}", query, Tables[index].Where);
        }
        if (Tables[index].Group != string.Empty)
        {
            query = string.Format("{0}\n{1}", query, Tables[index].Group);
        }
        if (Tables[index].Order != string.Empty)
        {
            query = string.Format("{0}\n{1}", query, Tables[index].Order);
        }
        if (Tables[index].Limit != string.Empty)
        {
            query = string.Format("{0}\n{1}", query, Tables[index].Limit);
        }

        return query;
    }

    

    //Gets table records from the index
    public void GetTableRecords(int index)
    {
        if (sqliteconnected)
        {
            string query = GenerateQuery(index);

            try
            {
                for (int i = 0; i < Tables[index].Columns.Count; i++)
                {
                    Tables[index].Columns[i].Records.Clear();
                }
                Debug.Log("my query :" + query);
                dbcmd.CommandText = query;
                reader = dbcmd.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < Tables[index].Columns.Count; i++)
                    {
                        switch (Tables[index].Columns[i]._columnType)
                        {
                            case Column.ColumnType._int:
                                if (reader.IsDBNull(i) != true)
                                {
                                    Tables[index].Columns[i].Records.Add(reader.GetInt32(i).ToString());
                                }
                                else
                                {
                                    Tables[index].Columns[i].Records.Add(null);
                                }
                                break;
                            case Column.ColumnType._string:
                                if (reader.IsDBNull(i) != true)
                                {
                                    Tables[index].Columns[i].Records.Add(reader.GetString(i));
                                }
                                else
                                {
                                    Tables[index].Columns[i].Records.Add(null);
                                }
                                break;
                        }
                    }
                }

                Core.Instance.SaveOutputLine(Core.DebugType.Log, string.Format("Executed Query:\n{0}", query));
            }
            catch (Exception)
            {
                Core.Instance.SaveOutputLine(Core.DebugType.Error, string.Format("Can't Executed Query:\n{0}", query), true);
            }
        }
    }

    //Gets all records from table
    public void GetAllTableRecords()
    {
        for (int i = 0; i < Tables.Count; i++)
        {
            GetTableRecords(i);
        }
    }

    //Gets a targeted record
    public string GetRecord(int table, int column, int row)
    {
        if (Core.Instance._cms.Tables.Count > table)
        {
            if (Core.Instance._cms.Tables[table].Columns.Count > column)
            {
                if (Core.Instance._cms.Tables[table].Columns[column].Records.Count > row)
                {
                    return Core.Instance._cms.Tables[table].Columns[column].Records[row];
                }
            }
        }

        return string.Empty;
    }

    //Gets number of records within a table/column
    public int GetRecordCount(int table, int column)
    {
        if (Core.Instance._cms.Tables.Count > table)
        {
            if (Core.Instance._cms.Tables[table].Columns.Count > column)
            {
                return Core.Instance._cms.Tables[table].Columns[column].Records.Count;
            }
        }

        return 0;
    }*/
}
