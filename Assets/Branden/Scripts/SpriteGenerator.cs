using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpriteGenerator : MonoBehaviour {
    public class GenSprite {
        public Sprite sprite = null;
    }

    int m_numSpritesLoading = 0;

    public bool IsLoadingSprites {
        get { return m_numSpritesLoading > 0; }
    }

    Dictionary<string, GenSprite> m_generatedSprites =
        new Dictionary<string, GenSprite>();

    static SpriteGenerator m_instance = null;
    public static SpriteGenerator Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<SpriteGenerator>();
            }
            return m_instance;
        }
    }

    //Assignes a valid sprite once sprite in GeneratedSprite has been generated...
    public void GenerateSpriteForImage(Image img, string url) {
        GenSprite genSprite = GenerateSprite(url);
        StartCoroutine(GenerateSpriteForImage(img, genSprite));
    }

    //Note: Sprites in GeneratedSprites take time to generate...
    public GenSprite GenerateSprite(string url) {
        if (m_generatedSprites.ContainsKey(url)) {
            return m_generatedSprites[url];
        }
        else {
            GenSprite genSprite = new GenSprite();
            m_generatedSprites.Add(url, genSprite);
            StartCoroutine(GenerateSprite(genSprite, url));
            return genSprite;
        }
    }

    IEnumerator GenerateSpriteForImage(Image img, GenSprite genSprite) {
        while (genSprite.sprite == null) {
            yield return null;
        }

        img.sprite = genSprite.sprite;
    }

    IEnumerator GenerateSprite(GenSprite genSprite, string url) {
        ++m_numSpritesLoading;

        WWW www = new WWW(url);
        yield return www;

        Texture2D texture = www.texture;

        genSprite.sprite = Sprite.Create(texture, new Rect(0, 0,
                texture.width, texture.height), new Vector2(0.5f, 0.5f));

        --m_numSpritesLoading;
    }
}
