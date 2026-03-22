using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

// most of these written by gemini 3 pro/chatgpt 5.4 im way too lazy lmao
// (and i dont know enough math to do these by myself within this timeframe)
public static class UIDotweenExtensions
{
    /// <summary>
    /// Play a series of sprites on an image based on an input collection of frames
    /// </summary>
    /// <param name="img">target image to play the sprite animation</param>
    /// <param name="frames">collection of animation frames</param>
    /// <param name="timePerFrame">how long (seconds) one sprite stays on screen before swapping to next one in array</param>
    /// <param name="loop">whether to loop it or not</param>
    public static Tween DOPlaySprites(this Image img, List<Sprite> frames, float timePerFrame, bool loop = true)
    {
        if (frames == null || frames.Count == 0)
        {
            Debug.LogError("[UIDotweenExtensions] sprite list reference null or there are no frames; returning null.");
            return null;
        }

        // kill any tweens targeting this image
        img.DOKill(true);

        float duration = timePerFrame * frames.Count;

        Tween tween = DOVirtual.Int(0, frames.Count - 1, duration, (i) => 
        {
            img.sprite = frames[i];
        })
        .SetEase(Ease.Linear)
        .SetTarget(img); // so img.DOKill() can find it later

        if (loop) 
            tween.SetLoops(-1, LoopType.Restart);

        return tween;
    }
    
    public static Tween DOAnchorPosArc(
        this RectTransform rectTransform,
        Vector2 endValue,
        float arcHeight,
        float duration,
        bool useCurrentAsStart = true)
    {
        Vector2 start = useCurrentAsStart ? rectTransform.anchoredPosition : rectTransform.anchoredPosition;

        Vector2 control = (start + endValue) * 0.5f + Vector2.up * arcHeight;

        return DOTween.To(() => 0f, t =>
        {
            rectTransform.anchoredPosition = EvaluateQuadraticBezier(t, start, control, endValue);
        }, 1f, duration);
    }

    public static Tween DOAnchorPosArc(
        this RectTransform rectTransform,
        Vector2 startValue,
        Vector2 endValue,
        float arcHeight,
        float duration)
    {
        rectTransform.anchoredPosition = startValue;

        Vector2 control = (startValue + endValue) * 0.5f + Vector2.up * arcHeight;

        return DOTween.To(() => 0f, t =>
        {
            rectTransform.anchoredPosition = EvaluateQuadraticBezier(t, startValue, control, endValue);
        }, 1f, duration);
    }

    private static Vector2 EvaluateQuadraticBezier(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float u = 1f - t;
        return (u * u * p0) + (2f * u * t * p1) + (t * t * p2);
    }
}
