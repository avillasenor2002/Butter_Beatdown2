using UnityEngine;
using UnityEngine.UI;

public class ScoreFillUI : MonoBehaviour
{
    [Header("Player Scores")]
    public ButterChurn player1Churn;
    public ButterChurn player2Churn;

    [Header("UI Elements")]
    public Image player1Fill; // Fill image for player 1
    public Image player2Fill; // Fill image for player 2

    [Header("Animation Settings")]
    public float scaleAmount = 1.1f; // How much bigger the winner gets
    public float shakeAmount = 5f; // Max shake in pixels
    public float shakeSpeed = 10f; // How fast the shake occurs

    private Vector3 player1OriginalScale;
    private Vector3 player2OriginalScale;
    private Vector3 player1OriginalPos;
    private Vector3 player2OriginalPos;

    void Start()
    {
        // Save original scales and positions
        player1OriginalScale = player1Fill.rectTransform.localScale;
        player2OriginalScale = player2Fill.rectTransform.localScale;
        player1OriginalPos = player1Fill.rectTransform.anchoredPosition;
        player2OriginalPos = player2Fill.rectTransform.anchoredPosition;
    }

    void Update()
    {
        UpdateScoreUI();
        AnimateWinningFill();
    }

    private void UpdateScoreUI()
    {
        float totalScore = player1Churn.score + player2Churn.score;

        if (totalScore == 0)
        {
            player1Fill.fillAmount = 0.5f;
            player2Fill.fillAmount = 0.5f;
            return;
        }

        float player1Percent = player1Churn.score / totalScore;
        float player2Percent = player2Churn.score / totalScore;

        player1Fill.fillAmount = player1Percent;
        player2Fill.fillAmount = player2Percent;
    }

    private void AnimateWinningFill()
    {
        // Determine the winner
        if (player1Churn.score > player2Churn.score)
        {
            AnimateFill(player1Fill.rectTransform, player1OriginalScale, player1OriginalPos);
            ResetFill(player2Fill.rectTransform, player2OriginalScale, player2OriginalPos);
        }
        else if (player2Churn.score > player1Churn.score)
        {
            AnimateFill(player2Fill.rectTransform, player2OriginalScale, player2OriginalPos);
            ResetFill(player1Fill.rectTransform, player1OriginalScale, player1OriginalPos);
        }
        else
        {
            // Tie - reset both
            ResetFill(player1Fill.rectTransform, player1OriginalScale, player1OriginalPos);
            ResetFill(player2Fill.rectTransform, player2OriginalScale, player2OriginalPos);
        }
    }

    private void AnimateFill(RectTransform fillRect, Vector3 originalScale, Vector3 originalPos)
    {
        // Slightly increase scale
        fillRect.localScale = Vector3.Lerp(fillRect.localScale, originalScale * scaleAmount, Time.deltaTime * 5f);

        // Shake by modifying anchored position
        float shakeX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        float shakeY = Mathf.Cos(Time.time * shakeSpeed) * shakeAmount;
        fillRect.anchoredPosition = originalPos + new Vector3(shakeX, shakeY, 0);
    }

    private void ResetFill(RectTransform fillRect, Vector3 originalScale, Vector3 originalPos)
    {
        fillRect.localScale = Vector3.Lerp(fillRect.localScale, originalScale, Time.deltaTime * 5f);
        fillRect.anchoredPosition = Vector3.Lerp(fillRect.anchoredPosition, originalPos, Time.deltaTime * 5f);
    }
}
