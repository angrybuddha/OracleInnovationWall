using UnityEngine;

public class AppManager : MonoBehaviour {

    public enum AppState {
        ATTRACT_CUBES,   //Cubes in background flying by...
        STREAMING_CUBES,    //User approaches screan and cubes stream in-front of screen...
        JOIN_CONVERSATION,  //When side panel first shows up...
        HIDE_CONVERSATION,  //When no players present after timeout...
        CAN_TAKE_POLL,      //Polling is active...
        STARTING_POLL,      //Countdown begins to start Poll A or poll B...
        POLL_A,             //Poll A is running...
        POLL_B,             //Poll B is running...
        TAKE_AWAY           //Social take away...
    }

    private static AppState m_state = AppState.ATTRACT_CUBES;
    public static AppState State {
        get { return m_state; }
        set { m_state = value; }
    }

    public static bool IsInAmbientMode {
        get{return m_state == AppState.ATTRACT_CUBES ||
                m_state == AppState.STREAMING_CUBES ||
                m_state == AppState.JOIN_CONVERSATION ||
                m_state == AppState.HIDE_CONVERSATION ||
                m_state == AppState.CAN_TAKE_POLL;
        }
    }
}
