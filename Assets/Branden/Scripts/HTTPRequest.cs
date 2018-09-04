/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used for saving off votes to the CMS.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

using AnswerInfo = ContentManager.AnswerInfo;
using AOrBAnswerInfo = ContentManager.AOrBAnswerInfo;
using GraphAnswerInfo = ContentManager.GraphAnswerInfo;

public class HTTPRequest : MonoBehaviour {
    //Address to send the post http request
    [SerializeField]
    private string urlRequest;
    public string UrlRequest {
        get { return urlRequest; }
        set { urlRequest = value; }
    }

    static HTTPRequest m_instance = null;
    public static HTTPRequest Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<HTTPRequest>();
            }
            return m_instance;
        }
    }

    ////public IEnumerator SaveVote(int uniqueId, string question_id, string answer_text_a, string answer_text_b, string scatter_plot_answer) {
    //public IEnumerator SaveVote(AnswerInfo answerInfo) {
    //    ContentManager content = ContentManager.Instance;

    //    if (urlRequest != string.Empty) {
    //        // Create a form object for sending high score data to the server
    //        WWWForm form = new WWWForm();

    //        //form.AddField("questiontypeid", questiontypeid);
    //        //form.headers.Add("", "PUT");
    //        form.AddField("id", answerInfo.uniqueId);
    //        form.AddField("question_id", answerInfo.questionId);

    //        // Create a download object
    //        WWW download = new WWW(urlRequest, form);

    //        // Wait until the download is done
    //        yield return download;

    //        if (!string.IsNullOrEmpty(download.error)) {
    //            Debug.Log("ARGGH");
    //            Debug.LogError("Error: " + download.error);
    //        }
    //        else {
    //            Debug.Log("ALL MY DATA " + download);
    //            Debug.Log(download.text);
    //        }
    //    }
    //    else {
    //        Debug.LogError("Error: Url Request Not Setup");
    //    }
    //    //return null;
    //}

    public IEnumerator SaveVote(AOrBAnswerInfo info) {
        WWWForm form = new WWWForm();
        form.AddField("id", info.uniqueId);
        form.AddField("question_id", info.questionId);
        form.AddField("answer_text_a", info.answerA.ToString());
        form.AddField("answer_text_b", info.answerB.ToString());
        return SaveVote(form);
    }

    public IEnumerator SaveVote(GraphAnswerInfo info) {
        WWWForm form = new WWWForm();
        string toStrArray = "[";

        for (int i = 0; i < info.numbers.Count; i++) {
            if (i == info.numbers.Count - 1)
                toStrArray += info.numbers[i].ToString() + "]";
            else
                toStrArray += info.numbers[i].ToString() + ",";
        }

        form.AddField("id", info.uniqueId);
        form.AddField("question_id", info.questionId);
        form.AddField("scatter_plot_answer", toStrArray);
        return SaveVote(form);
    }

    //public IEnumerator SaveVote(int uniqueId, string question_id, string answer_text_a, string answer_text_b, string scatter_plot_answer) {
    public IEnumerator SaveVote(WWWForm form) {
        ContentManager content = ContentManager.Instance;

        if (urlRequest != string.Empty) {
            // Create a download object
            WWW download = new WWW(urlRequest, form);

            // Wait until the download is done
            yield return download;

            if (!string.IsNullOrEmpty(download.error)) {
                Debug.Log("ARGGH");
                Debug.LogError("Error: " + download.error);
            }
            else {
                Debug.Log("ALL MY DATA " + download);
                Debug.Log(download.text);
            }
        }
        else {
            Debug.LogError("Error: Url Request Not Setup");
        }
        //return null;
    }
}