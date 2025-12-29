using UnityEngine;

public class UI_HelpPanel : MonoBehaviour
{
    [Header("Pages (GameObjects)")]
    [SerializeField] private GameObject[] pages;

    [Header("Options")]
    [SerializeField] private bool loop = true;

    private int index = 0;

    private void OnEnable()
    {
        index = Mathf.Clamp(index, 0, pages.Length - 1);
        Apply();
    }

    public void Next()
    {
        Debug.Log("[Help] Next called");
        if (pages == null || pages.Length == 0)
        {
            return;
        }
        if (loop)
        {
            index = (index + 1) % pages.Length;
        }
        else
        {
            index = Mathf.Min(pages.Length - 1, index + 1);
        }
        Apply();
    }


    public void Prev()
    {
        Debug.Log("[Help] Prev called");
        if (pages == null || pages.Length == 0)
        {
            return;
        }
        if (loop)
        {
            index = (index - 1 + pages.Length) % pages.Length;
        }
        else
        {
            index = Mathf.Max(0, index - 1);
        }
        Apply();
    }

    private void Apply()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] != null)
            {
                pages[i].SetActive(i == index);
            }
        }
    }
}






