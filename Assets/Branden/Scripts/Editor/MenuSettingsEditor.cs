using UnityEngine;
using UnityEditor;

public class MenuSettingsEditor : MonoBehaviour {
    [MenuItem("Edit/Reset Playerprefs")]
    public static void DeletePlayerPrefs() {
        PlayerPrefs.DeleteAll();
        Debug.LogWarning("Player Prefs Reset!");
    }
}