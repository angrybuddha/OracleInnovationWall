using UnityEngine;
using System.Collections;

public class PlaygroundNextSceneWithEnter : MonoBehaviour {
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return))
			Application.LoadLevel ((Application.loadedLevel+1)%Application.levelCount);
	}
}
