using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class TwitterCube : MonoBehaviour {
    [SerializeField]
    string m_rawTweet = null;

    TMPro.TextMeshProUGUI m_textPro = null;
    Image m_image = null;

    CanvasGroup[] m_canvasGroups = null;

    static List<TwitterCube> m_twitterCubes = new List<TwitterCube>();
    public static List<TwitterCube> TwitterCubes {
        get { return m_twitterCubes; }
    }

    protected virtual void Awake() {
        ApplyTwitterContent();
        m_canvasGroups = GetComponentsInChildren<CanvasGroup>();
        m_twitterCubes.Add(this);
    }

    protected virtual void Update() {
        //TODO: Fade Text alpha
        if (m_canvasGroups != null) {
            CubeSpawner spawner = CubeSpawner.Instance;

            Transform cameraT = Camera.main.transform;
            Vector3 toPoint = transform.position - cameraT.position;
            float distFromCamera = Vector3.Dot(cameraT.forward, toPoint);
            float alpha = 1;

            if (distFromCamera > spawner.AmountDistToFade) {
                float fadeDist = distFromCamera - spawner.StartFadeDistance;
                alpha -= Mathf.Min(fadeDist / spawner.AmountDistToFade);
            }

            foreach (CanvasGroup group in m_canvasGroups) {
                group.alpha = alpha;
            }
        }
    }

    public static void ResetTwitterContent() {
        foreach (TwitterCube twitterCube in m_twitterCubes) {
            twitterCube.ApplyTwitterContent();
        }
    }

    public void ApplyTwitterContent() {
        TwitterManager tweets = TwitterManager.Instance;
        if (tweets != null) {
            TweetSearchTwitterData data = null;

            if (this is AttractedCube || this is StreamingCube) {
                data = tweets.GetNextImgTweet();
                if (data == null && (m_image == null || m_image.sprite == null)) {
                    tweets.GetNextTweet();
                }
            }
            else {
                data = tweets.GetNextTweet();
            }

            if (data == null) {
                Debug.LogError("Data is NULL for some strange reason!!!");
            }
            else {
                SetText(data.tweetText);
                Load(data.tweetMedia);
            }
        }
    }

    //Twitter Code...   //TODO: Review and make sure this code is correct...
    public void SetText(string text) {
        m_rawTweet = text + " ";    //HACK: adding empty space resolves last character from not being included bellow...
        text = "";

        Regex regex = new Regex(@"[^@#]+");
        Match match = regex.Match(m_rawTweet);

        if (match.Success) {
            if (match.Index == 0) {
                text += match.Value;
            }
        }

        regex = new Regex(@"([@#][^:\-=\s]+)([^@#]+)");
        match = regex.Match(m_rawTweet);

        while (match.Success) {
            text += "<color=#F80000FF>" + match.Groups[1] + "</color>" + match.Groups[2];
            match = match.NextMatch();
        }

        //Parses out http(s) and www...
        regex = new Regex(@"\b(?:https?://|www\.)\S+\b\b",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        string str = regex.Replace(text, "");

        if (string.IsNullOrEmpty(str)) {
            str = "Images <color=#F80000FF>@Oracle</color>";
        }

        if (m_textPro == null) {
            m_textPro = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }

        m_textPro.text = str;
    }

    public void Load(string imgUrl) {
        if (string.IsNullOrEmpty(imgUrl)) {
            return;
        }

        if (m_image == null) {
            m_image = GetComponentInChildren<Image>();
        }

        if (m_image) {
            SpriteGenerator.Instance.GenerateSpriteForImage(m_image, imgUrl);
            m_image.color = Color.white;
        }
        else {
            Debug.LogError("Image is null!");
        }
    }
}
