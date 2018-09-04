/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used for saving off votes to the CMS.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

public class HTTPRequest_Old : MonoBehaviour
{
    //Address to send the post http request
    [SerializeField]
    private string urlRequest;
    public string UrlRequest
    {
        get { return urlRequest; }
        set { urlRequest = value; }
    }

    static HTTPRequest_Old m_instance = null;
    public static HTTPRequest_Old Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<HTTPRequest_Old>();
            }
            return m_instance;
        }
    }

    public IEnumerator SaveVote(string question_id, string answer_text_a, string answer_text_b, string scatter_plot_answer)
    {
        if (urlRequest != string.Empty)
        {
            // Create a form object for sending high score data to the server
            WWWForm form = new WWWForm();

            //form.AddField("questiontypeid", questiontypeid);
            form.AddField("question_id", question_id);
            form.AddField("answer_text_a", answer_text_a);
            form.AddField("answer_text_b", answer_text_b);
            form.AddField("scatter_plot_answer", scatter_plot_answer);

            // Create a download object
            WWW download = new WWW(urlRequest, form);

            // Wait until the download is done
            yield return download;

            if (!string.IsNullOrEmpty(download.error))
            {
                Debug.Log("ARGGH");
                Core.Instance.SaveOutputLine(Core.DebugType.Error, download.error, true);
            }
            else
            {
                Debug.Log("ALL MY DATA " + download);
                Core.Instance.SaveOutputLine(Core.DebugType.Log, download.text);
            }
        }
        else
        {
            Core.Instance.SaveOutputLine(Core.DebugType.Error, "Url Request Not Setup", true);
        }
        //return null;
    }
}
