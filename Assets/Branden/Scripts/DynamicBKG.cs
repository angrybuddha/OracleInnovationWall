using UnityEngine;
using UnityEngine.UI;

public class DynamicBKG : MonoBehaviour {

    Image m_image = null;

    static DynamicBKG m_instance = null;
    public static DynamicBKG Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<DynamicBKG>();
            }
            return m_instance;
        }
    }

    public void Load(string imgUrl) {
        if (m_image == null) {
            m_image = GetComponentInChildren<Image>();
        }

        if (m_image) {
            SpriteGenerator.Instance.GenerateSpriteForImage(m_image, imgUrl);
        }
        else {
            Debug.LogError("Image is null!");
        }
    }

    //Sets pixel color from center of background image to fog color...
    public void UpdateFogColor() {
        Texture2D texture = m_image.sprite.texture;
        int x = texture.width / 2;
        int y = texture.height / 2;
        Color pixelColor = texture.GetPixel(x, y);
        RenderSettings.fogColor = pixelColor;
    }
}
