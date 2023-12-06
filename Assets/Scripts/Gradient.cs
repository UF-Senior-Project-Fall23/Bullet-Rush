using UnityEngine;
using UnityEngine.UI;

// Utility that generates a 1x2 image with 2 colors, which when stretched out forms a nice gradient.
// Currently used by the Death Screen background.
public class Gradient : MonoBehaviour
{
    public Color TopColor;
    public Color BottomColor;

    private RawImage img;

    private Texture2D backgroundTexture;

    void Awake()
    {
        img = GetComponent<RawImage>();
        backgroundTexture = new Texture2D(1, 2);
        backgroundTexture.wrapMode = TextureWrapMode.Clamp;
        backgroundTexture.filterMode = FilterMode.Bilinear;
        SetColor(BottomColor, TopColor);
    }

    public void SetColor(Color color1, Color color2)
    {
        backgroundTexture.SetPixels(new Color[] { color1, color2 });
        backgroundTexture.Apply();
        img.texture = backgroundTexture;
    }
}
