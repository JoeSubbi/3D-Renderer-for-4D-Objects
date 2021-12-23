using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class VideoControls : MonoBehaviour
{
    public VideoPlayer video;
    public RectTransform progress;
    public RectTransform timeline;
    public GameObject continue_button;

    void Awake()
    {
        continue_button.SetActive(false);
    }

    void Update()
    {
        if ((long)video.frame < (long)video.frameCount)
        {
            float x = -timeline.rect.width * (1-((float)video.frame / (float)video.frameCount));
            progress.sizeDelta = new Vector2(x, progress.sizeDelta.y);
        }

        if ((long)video.frame == (long)video.frameCount-1)
            continue_button.SetActive(true);
        else
            continue_button.SetActive(false);
    }

    public void Pause()
    {
        if (video.isPlaying)
            video.Pause();
        else
            video.Play();        
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {
            // Point to skip to
            float x;

            // Get point on timeline
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(timeline, Input.mousePosition, null, out localPoint))
            {
                x = timeline.rect.width / 2;

                // check point is inside timeline panel
                if (Mathf.Abs(localPoint.x) < x &&
                    localPoint.y < timeline.rect.height)
                {
                    // Convert point to 0 to timeline length
                    x = localPoint.x + x;
                    // Convert point to 0 to 1
                    x /= timeline.rect.width;

                    // Move to frame
                    video.frame = (int)(x * video.frameCount);
                }
            }

        }
    }

}
