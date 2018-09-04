using UnityEngine;
using System.Collections;

public class HTTPRequst_Dump : MonoBehaviour
{
    private HTTPRequest_Old httpRequst;

    private bool finished = false;

    // Use this for initialization
    void Start()
    {
        httpRequst = this.GetComponent<HTTPRequest_Old>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!finished)
        {
            Dump3("3", "666", "0.0", "0.0");

            Debug.Log("Finished");

            finished = true;
        }
    }

    void Dump(string questiontypeid, string questionid, string valuea, string valueb, int numberOfRecords)
    {
        for (int i = 0; i < numberOfRecords; i++)
        {
            StartCoroutine(httpRequst.SaveVote(questiontypeid,questionid,valuea, valueb));
        }
    }

    void Dump2(string questiontypeid, string questionid)
    {
        float counter = 0.0f;

        while(counter <= 1f)
        {
            StartCoroutine(httpRequst.SaveVote(questiontypeid, questionid, counter.ToString(), counter.ToString()));

            counter = counter + 0.01f;
        }
    }

    void Dump3(string questiontypeid, string questionid, string valuea, string valueb)
    {
        StartCoroutine(httpRequst.SaveVote(questiontypeid, questionid, valuea, valueb));
    }
}
