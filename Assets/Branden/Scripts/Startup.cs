using UnityEngine;
using System.Collections;
using System;

public class Startup : MonoBehaviour {
    static bool m_contentUpdating = false;
    static public bool ContentUpdating {
        get { return m_contentUpdating; }
        set { m_contentUpdating = value; }
    }

    // Use this for initialization
    void Awake() {
        LoadContent();
    }

    void Update() {
        RestartWhenTimeToRefresh();
    }

    void LoadContent() {
        ContentManager content = ContentManager.Instance;
        LoadingScreen screen = LoadingScreen.Instance;
        screen.Init();

        content.OpenConnection();
        content.GetGlobalSettings();
        content.GetActivePlaylist();
        content.GetBackground();
        content.CloseConnection();

        TwitterManager.Instance.SetupTwitterCubes();
        DynamicBKG.Instance.Load(content.background_path);

        StartCoroutine(FinishLoading());
    }

    void RestartWhenTimeToRefresh() {
        if (ContentUpdating) {
            return;
        }

        DateTime systemTime = DateTime.Now;
        string hour = systemTime.Hour.ToString();
        string minute = systemTime.Minute.ToString();

        if (hour.Length < 2) {
            hour = "0" + hour;
        }
        if (minute.Length < 2) {
            minute = "0" + minute;
        }

        if (StartupSettings.Instance.AppRefreshTimeStr == (hour + minute)) {
            ContentUpdating = true;
            LoadingScreen.Instance.Show();
            LoadContent();
        }
    }

    IEnumerator FinishLoading() {
        SpriteGenerator spriteGen = SpriteGenerator.Instance;
        while (spriteGen.IsLoadingSprites &&
            !TwitterManager.Instance.IsReady) {
            yield return new WaitForSeconds(.1f);
        }

        DynamicBKG.Instance.UpdateFogColor();
        LoadingScreen.Instance.FinishLoading();
    }
}