using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBar : MonoBehaviour
{
    [Header("Settings")]
    public int maxSegments = 6;

    public int currentSegments;
    public Image segmentPrefab;

    [Header("Colors")]
    public Color onColor = Color.red;

    public Color offColor = new(1, 0, 0, 0.2f);

    [Header("Optional images instead of color")]
    public Sprite onSprite;

    public Sprite offSprite;

    private readonly List<Image> _images = new();

    private void Start()
    {
        BuildBar();
        MainMenuManager.instance.SetCarStats();
    }

    private void BuildBar()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        _images.Clear();

        if (maxSegments <= 0 || segmentPrefab == null)
        {
            return;
        }

        for (int i = 0; i < maxSegments; i++)
        {
            Image seg = Instantiate(segmentPrefab, transform);
            seg.name = $"Seg{i + 1}";
            _images.Add(seg);
        }
    }

    public void SetValue(int activeSegments)
    {
        if (_images.Count == 0)
        {
            return;
        }

        activeSegments = Mathf.Clamp(activeSegments, 0, maxSegments);

        for (int i = 0; i < _images.Count; i++)
        {
            bool isActive = i < activeSegments;

            if (onSprite != null && offSprite != null)
            {
                _images[i].sprite = isActive ? onSprite : offSprite;
                _images[i].color = Color.white;
            }
            else
            {
                _images[i].color = isActive ? onColor : offColor;
            }
        }
    }
}