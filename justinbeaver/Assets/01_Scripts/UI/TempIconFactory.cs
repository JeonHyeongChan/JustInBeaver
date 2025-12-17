using UnityEngine;

public static class TempIconFactory
{
    public static Sprite Create(Color color)
    {
        Texture2D tex = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];

        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = color;

        tex.SetPixels(pixels);
        tex.Apply();

        return Sprite.Create(
            tex,
            new Rect(0, 0, 32, 32),
            new Vector2(0.5f, 0.5f)
        );
    }
}