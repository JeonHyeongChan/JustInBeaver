using UnityEngine;
using UnityEngine.UI;

public class UI_HelpPanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image helpImage;

    [Header("Pages")]
    [SerializeField] private Sprite[] pages;

    [Header("Options")]
    [SerializeField] private bool loop = true;

    private int index;

    private void OnEnable()
    {
        index = Mathf.Clamp(index, 0, (pages != null ? pages.Length : 0) - 1);
        Apply();
    }

    public void Next()
    {
        if (pages == null || pages.Length == 0) return;

        if (loop) index = (index + 1) % pages.Length;
        else index = Mathf.Min(pages.Length - 1, index + 1);

        Apply();
    }

    public void Prev()
    {
        if (pages == null || pages.Length == 0) return;

        if (loop) index = (index - 1 + pages.Length) % pages.Length;
        else index = Mathf.Max(0, index - 1);

        Apply();
    }

    private void Apply()
    {
        if (helpImage == null) return;

        if (pages == null || pages.Length == 0)
        {
            helpImage.sprite = null;
            helpImage.enabled = false;
            return;
        }

        helpImage.enabled = true;
        helpImage.sprite = pages[index];
        helpImage.SetNativeSize(); // 원본 크기 사용 싫으면 제거
    }
}