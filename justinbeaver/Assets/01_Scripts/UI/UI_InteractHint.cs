using TMPro;
using UnityEngine;

public class UI_InteractHint : MonoBehaviour
{
    [SerializeField] private TMP_Text hintText;

    RectTransform rect;
    Camera cam;
    Transform target;

    Vector3 worldOffset = new Vector3(0, 1.2f, 0);

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        cam = Camera.main;

        Hide();
    }

    private void LateUpdate()
    {
        if (target == null || cam == null)
            return;

        Vector3 screenPosition = cam.WorldToScreenPoint(target.position + worldOffset);

        if (screenPosition.z < 0f)  // 카메라 뒤면 안보이게
        {
            Hide();
            return;
        }

        rect.position = screenPosition;
    }

    public void Show(Transform targetTransform, string text)
    {
        target = targetTransform;
        hintText.text = text;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        target = null;
        gameObject.SetActive(false);
    }
}
