using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UISlideInSimple : MonoBehaviour
{
    public float slideDuration = 2f;

    public enum SlideFrom
    { Left, Right, Top, Bottom }

    public SlideFrom slideFrom = SlideFrom.Left;

    private void Start()
    {
        SlideAllChildren(transform);
    }

    private void SlideAllChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            RectTransform rect = child.GetComponent<RectTransform>();
            if (rect != null)
            {
                _ = StartCoroutine(SlideInChild(rect));
            }

            Image img = child.GetComponent<Image>();
            if (img != null && child.childCount > 0)
            {
                continue;
            }

            if (child.childCount > 0 && child.GetComponent<HorizontalLayoutGroup>() == null && child.GetComponent<GridLayoutGroup>() == null)
            {
                SlideAllChildren(child);
            }
        }
    }

    private IEnumerator SlideInChild(RectTransform rectTransform)
    {
        Vector2 targetPos = rectTransform.anchoredPosition;
        Vector2 startPos = targetPos;

        switch (slideFrom)
        {
            case SlideFrom.Left:
                startPos.x = -Screen.width;
                break;

            case SlideFrom.Right:
                startPos.x = Screen.width;
                break;

            case SlideFrom.Top:
                startPos.y = Screen.height;
                break;

            case SlideFrom.Bottom:
                startPos.y = -Screen.height;
                break;
        }

        rectTransform.anchoredPosition = startPos;

        float t = 0f;
        while (t < slideDuration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / slideDuration);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, progress);
            yield return null;
        }

        rectTransform.anchoredPosition = targetPos;
    }
}