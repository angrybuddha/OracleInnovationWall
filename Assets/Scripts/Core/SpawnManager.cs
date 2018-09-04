/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used for managing player cubes, and default cube votes.
*******************************************************************************************/
using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    //Used for spawning player cubes
    [SerializeField, Header("Spawners: ")]
    private Spawner playerCubes;
    public Spawner PlayerCubes
    {
        get { return playerCubes; }
        set { playerCubes = value; }
    }

    //Used for spawning vote cubes
    [SerializeField]
    private Spawner defaultCubes;
    public Spawner DefaultCubes
    {
        get { return defaultCubes; }
        set { defaultCubes = value; }
    }

    //Delay time
    [SerializeField, Header("Delay Timer: ")]
    private float timeDelaySeconds = 3f;
    public float TimeDelaySeconds
    {
        get { return timeDelaySeconds; }
        set { timeDelaySeconds = value; }
    }

    //Delay countdown
    [SerializeField]
    private float timeDelayCountDown = 0f;
    public float TimeDelayCountDown
    {
        get { return timeDelayCountDown; }
        set { timeDelayCountDown = value; }
    }

    //Spawn time separator seconds
    [SerializeField, Header("Spawn Timer: ")]
    private float spawnSeconds = .1f;
    public float SpawnSeconds
    {
        get { return spawnSeconds; }
        set { spawnSeconds = value; }
    }

    //Spawn count down
    [SerializeField]
    private float spawnCountDown = 0f;
    public float SpawnCountDown
    {
        get { return spawnCountDown; }
        set { spawnCountDown = value; }
    }

    //Spawn Time Stamps
    private float timeDelayStamp = 0f;
    private float timeSpawnStamp = 0f;

    //Vote Counters
    private int answer1Counter = 0;
    private int answer2Counter = 0;
    private int answer3Counter = 0;

    //Syncs the cube votes
    public void SyncCubeVotes(QuestionManager.QuestionState questionState, bool part2 = false)
    {
        SyncTimeDelay();
        SyncSpawnTime();

        if (TimeDelayCountDown > TimeDelaySeconds)
        {
            if (SpawnCountDown > SpawnSeconds)
            {
                switch (questionState)
                {
                    case QuestionManager.QuestionState.None:
                        break;

                    case QuestionManager.QuestionState.Poll:
                        if (answer1Counter < Core.Instance._questionManager.Answer1Records)
                        {
                            Core.Instance._spawnManager.DefaultCubes.Spawn();
                            Core.Instance._spawnManager.DefaultCubes.LastSpawned.tag = "Answer 1";
                            Core.Instance._spawnManager.DefaultCubes.LastSpawned.GetComponent<CubeDefault>().QuestionState = QuestionManager.QuestionState.Poll;
                            
                            answer1Counter++;
                        }
                        if (answer2Counter < Core.Instance._questionManager.Answer2Records)
                        {
                            Core.Instance._spawnManager.DefaultCubes.Spawn();
                            Core.Instance._spawnManager.DefaultCubes.LastSpawned.tag = "Answer 2";
                            Core.Instance._spawnManager.DefaultCubes.LastSpawned.GetComponent<CubeDefault>().QuestionState = QuestionManager.QuestionState.Poll;
                            answer2Counter++;
                        }
                        break;
                    case QuestionManager.QuestionState.MultipleChoice:
                        if (answer1Counter < Core.Instance._questionManager.Answer1Records)
                        {
                            Core.Instance._spawnManager.DefaultCubes.Spawn();
                            Core.Instance._spawnManager.DefaultCubes.LastSpawned.tag = "Answer 1";
                            Core.Instance._spawnManager.DefaultCubes.LastSpawned.GetComponent<CubeDefault>().QuestionState = QuestionManager.QuestionState.MultipleChoice;
                            answer1Counter++;
                        }
                        if (answer2Counter < Core.Instance._questionManager.Answer2Records)
                        {
                            Core.Instance._spawnManager.DefaultCubes.Spawn();
                            Core.Instance._spawnManager.DefaultCubes.LastSpawned.tag = "Answer 2";
                            Core.Instance._spawnManager.DefaultCubes.LastSpawned.GetComponent<CubeDefault>().QuestionState = QuestionManager.QuestionState.MultipleChoice;
                            answer2Counter++;
                        }
                        if (answer3Counter < Core.Instance._questionManager.Answer3Records)
                        {
                            Core.Instance._spawnManager.DefaultCubes.Spawn();
                            Core.Instance._spawnManager.DefaultCubes.LastSpawned.tag = "Answer 3";
                            Core.Instance._spawnManager.DefaultCubes.LastSpawned.GetComponent<CubeDefault>().QuestionState = QuestionManager.QuestionState.MultipleChoice;
                            answer3Counter++;
                        }
                        break;

                    case QuestionManager.QuestionState.ScatterPlot:

                        if (answer1Counter < Core.Instance._questionManager.AnswerVectorRecords.Count)
                        {
                            Core.Instance._spawnManager.DefaultCubes.Spawn();
                            Core.Instance._spawnManager.DefaultCubes.LastSpawned.tag = "Scatter";
                            Core.Instance._spawnManager.DefaultCubes.LastSpawned.GetComponent<CubeDefault>().QuestionState = QuestionManager.QuestionState.ScatterPlot;

                            if (!part2)
                            {
                                float pivot_x = -35f;
                                float z = 32.5f;

                                Core.Instance._spawnManager.DefaultCubes.LastSpawned.GetComponent<CubeDefault>().StopAtTarget = true;
                                Core.Instance._spawnManager.DefaultCubes.LastSpawned.GetComponent<CubeDefault>().Target = new Vector3(pivot_x + (70f * Core.Instance._questionManager.AnswerVectorRecords[answer1Counter].x), Random.Range(9f, 15f), z);
                            }
                            else
                            {
                                float pivot_x = -32f;
                                float pivot_z = -12f;
                                float z = 32.5f;

                                Core.Instance._spawnManager.DefaultCubes.LastSpawned.GetComponent<CubeDefault>().StopAtTarget = false;
                                Core.Instance._spawnManager.DefaultCubes.LastSpawned.GetComponent<CubeDefault>().Target = new Vector3(pivot_x + (64f * Core.Instance._questionManager.AnswerVectorRecords[answer1Counter].x), pivot_z + (24f * Core.Instance._questionManager.AnswerVectorRecords[answer1Counter].y), z);
                            }

                            answer1Counter++;
                        }

                        break;
                }

                timeSpawnStamp = 0;
            }
        }
    }

    //Resets all spawned objects
    public void ResetAllSpawnedObjects()
    {
        CleanSpawnManager();

        PlayerCubes.ResetAllSpawnedObjects();
        DefaultCubes.ResetAllSpawnedObjects();
    }

    //Cleans up spawn manager
    public void CleanSpawnManager()
    {
        answer1Counter = 0;
        answer2Counter = 0;
        answer3Counter = 0;

        timeDelayStamp = 0f;
        timeSpawnStamp = 0f;

        TimeDelayCountDown = 0f;
        SpawnCountDown = 0f;
    }

    //Syncs the time delay 
    void SyncTimeDelay()
    {
        if (timeDelayStamp == 0)
        {
            timeDelayStamp = Time.time;
        }

        TimeDelayCountDown = (Time.time - timeDelayStamp);
    }

    //Syncs the spawn time
    void SyncSpawnTime()
    {
        if (timeSpawnStamp == 0)
        {
            timeSpawnStamp = Time.time;
        }

        SpawnCountDown = (Time.time - timeSpawnStamp);
    }
}
