using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

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
}
