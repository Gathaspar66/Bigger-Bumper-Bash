using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlideController : MonoBehaviour
{
    [System.Serializable]
    public class SlideElement
    {
        public RectTransform rect;
        public SlideFrom slideFrom = SlideFrom.Left;
    }

    public enum SlideFrom
    { Left, Right, Top, Bottom }

    public float slideDuration = 1f;
    public List<SlideElement> elements = new();

    [Header("Optional: activation after Show/Hide")]
    public List<GameObject> activateAfterShow = new();

    [Header("Optional: activation before Show/Hide")]
    public List<GameObject> activateBeforeShow = new();

    private readonly Dictionary<RectTransform, Vector2> originalPositions = new();

    private void Awake()
    {
        foreach (SlideElement el in elements)
        {
            if (el.rect != null && !originalPositions.ContainsKey(el.rect))
            {
                originalPositions.Add(el.rect, el.rect.anchoredPosition);
            }
        }
    }

    public void Show()
    {
        StopAllCoroutines();
        _ = StartCoroutine(ShowSequence());
    }

    private IEnumerator ShowSequence()
    {
        foreach (GameObject obj in activateBeforeShow)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        List<Coroutine> running = new();

        foreach (SlideElement el in elements)
        {
            if (el.rect != null)
            {
                running.Add(StartCoroutine(SlideIn(el)));
            }
        }

        foreach (Coroutine c in running)
        {
            yield return c;
        }

        foreach (GameObject obj in activateAfterShow)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    public void Hide()
    {
        StopAllCoroutines();
        foreach (SlideElement el in elements)
        {
            if (el.rect != null)
            {
                _ = StartCoroutine(SlideOut(el));
            }
        }
        foreach (GameObject obj in activateBeforeShow)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        foreach (GameObject obj in activateAfterShow)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    private IEnumerator SlideIn(SlideElement el)
    {
        Vector2 targetPos = originalPositions[el.rect];
        Vector2 startPos = GetOffscreenPosition(el, targetPos);
        el.rect.gameObject.SetActive(true);

        el.rect.anchoredPosition = startPos;

        float t = 0f;
        while (t < slideDuration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / slideDuration);
            el.rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, progress);
            yield return null;
        }

        el.rect.anchoredPosition = targetPos;
    }

    private IEnumerator SlideOut(SlideElement el)
    {
        Vector2 startPos = el.rect.anchoredPosition;
        Vector2 targetPos = GetOffscreenPosition(el, startPos);

        float t = 0f;
        while (t < slideDuration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / slideDuration);
            el.rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, progress);
            yield return null;
        }

        el.rect.anchoredPosition = targetPos;

        el.rect.gameObject.SetActive(false);
    }

    private Vector2 GetOffscreenPosition(SlideElement el, Vector2 basePos)
    {
        Vector2 offscreen = basePos;
        switch (el.slideFrom)
        {
            case SlideFrom.Left:
                offscreen.x = -Screen.width;
                break;

            case SlideFrom.Right:
                offscreen.x = Screen.width;
                break;

            case SlideFrom.Top:
                offscreen.y = Screen.height;
                break;

            case SlideFrom.Bottom:
                offscreen.y = -Screen.height;
                break;
        }
        return offscreen;
    }
}