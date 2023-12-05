using UnityEngine;
using UnityEngine.UI;

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
