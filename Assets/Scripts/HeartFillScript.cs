using UnityEngine;
using UnityEngine.UI;

public class HeartFill : MonoBehaviour
{
    [SerializeField] private RectTransform fillRect;
    [SerializeField] private float maxScore = 1000f;
    [SerializeField] private float minHeight = 0f;
    [SerializeField] private float maxHeight = 100f;

    public void UpdateFamilyHeartFill(float score)
    {
        if (gameObject.name == "Family")
        {
            float clampedScore = Mathf.Clamp(score, 0, maxScore);
            float height = Mathf.Lerp(minHeight, maxHeight, clampedScore / maxScore);

            Vector2 size = fillRect.sizeDelta;
            size.y = height;
            fillRect.sizeDelta = size;
        }

       
    }

    public void UpdateFriendHeartFill(float score)
    {
        if (gameObject.name == "Friend")
        {
            float clampedScore = Mathf.Clamp(score, 0, maxScore);
            float height = Mathf.Lerp(minHeight, maxHeight, clampedScore / maxScore);

            Vector2 size = fillRect.sizeDelta;
            size.y = height;
            fillRect.sizeDelta = size;
        }
       
    }
}