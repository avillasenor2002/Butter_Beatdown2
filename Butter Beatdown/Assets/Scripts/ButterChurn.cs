using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButterChurn : MonoBehaviour
{
    public enum PlayerID { Player1, Player2 }
    public PlayerID playerID;

    public int score = 0;
    public int scoreCap1;
    public int scoreCap2;
    public TMP_Text scoreText;
    public TMP_Text FinalscoreText;
    public Image FillUI;
    public bool isStarted;
    public ParticleSystem part;

    public float danger;
    public float maxdanger;
    public float subtractionAmount;
    public float CurrentsubtractionInterval;
    public float subtractionInterval;
    public float subtractionInterval2;
    public float subtractionInterval3;

    public bool isemergency;
    public bool isright;
    public bool sickomode;
    public SpriteRenderer ChurnedWell1;
    public SpriteRenderer ChurnedWell2;
    public SpriteRenderer DangerButt;

    public Sprite ChurnWIMG1;
    public Sprite ChurnWIMG2;

    public Sprite NormalChurn;
    public Sprite MidChurn;
    public Sprite DangerChurn;

    public GameObject CloggedUI;
    public GameObject DangerUI;

    public GameObject SpeedUP;

    public bool speed1;
    public bool speed2;
    public bool speed3;

    private KeyCode churnKey;
    private KeyCode emergencyLeftKey;
    private KeyCode emergencyRightKey;

    private float downPosY_Player1 = -1f;
    private float upPosY_Player1 = 2.19f;

    private float downPosY_Player2 = -1.5f;
    private float upPosY_Player2 = 2.4f;

    public GameObject SickoModeUI;
    private Coroutine sickoBlinkRoutine;

    [Header("Churn Audio")]
    public AudioSource churnAudioSource;
    public AudioClip[] churnSounds;
    public AudioClip cloggedSound;

    private bool cloggedSoundPlayed = false;

    [Header("Demo / End Sequence")]
    public bool demoMode = false;
    public TMP_Text demoReadyText;
    public string demoNextSceneName;
    public Image fadeImage;
    public float fadeDuration = 0.75f;
    public float sceneLoadDelay = 1.25f;

    private static bool demoPlayer1Ready;
    private static bool demoPlayer2Ready;
    private static bool demoTransitionStarted;

    private bool gameEnded = false;
    public ButterChurn winningChurn;
    public float holdDuration = 5f;
    public float holdTimer = 0f;
    public Image endHoldFillUI;
    public bool canInteract = true;
    public string nextSceneName;

    void Start()
    {
        AssignControls();
        CurrentsubtractionInterval = subtractionInterval;
        UpdateScoreText();
        StartCoroutine(SubtractVariable());

        if (demoMode && demoReadyText != null)
        {
            demoReadyText.text = "Not Ready";
            demoReadyText.gameObject.SetActive(true);
        }
    }

    void AssignControls()
    {
        if (playerID == PlayerID.Player1)
        {
            churnKey = KeyCode.S;
            emergencyLeftKey = KeyCode.A;
            emergencyRightKey = KeyCode.D;
        }
        else
        {
            churnKey = KeyCode.DownArrow;
            emergencyLeftKey = KeyCode.LeftArrow;
            emergencyRightKey = KeyCode.RightArrow;
        }
    }

    void Update()
    {
        FillUI.fillAmount = danger / maxdanger;

        if (!isStarted)
            return;

        if (canInteract)
        {
            if (danger < maxdanger)
            {
                if (Input.GetKeyDown(churnKey) && !isemergency)
                {
                    float newY = (playerID == PlayerID.Player1) ? downPosY_Player1 : downPosY_Player2;
                    transform.position = new Vector3(transform.position.x, newY, transform.position.z);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    danger += 2;
                    AddScore(5);

                    if (churnAudioSource != null && churnSounds != null && churnSounds.Length > 0)
                    {
                        churnAudioSource.PlayOneShot(
                            churnSounds[Random.Range(0, churnSounds.Length)]
                        );
                    }
                }

                if (danger <= 25) DangerButt.sprite = NormalChurn;
                else if (danger > 25 && danger <= 35) DangerButt.sprite = MidChurn;
                else DangerButt.sprite = DangerChurn;
            }
            else
            {
                isemergency = true;
            }

            if (Input.GetKeyUp(churnKey) && !isemergency)
            {
                float newY = (playerID == PlayerID.Player1) ? upPosY_Player1 : upPosY_Player2;
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
                part.Play();
            }

            danger = Mathf.Clamp(danger, 0, 50);

            if (isemergency)
            {
                CloggedUI.SetActive(true);

                if (!cloggedSoundPlayed && churnAudioSource != null && cloggedSound != null)
                {
                    churnAudioSource.PlayOneShot(cloggedSound);
                    cloggedSoundPlayed = true;
                }

                if (demoMode)
                {
                    if (demoReadyText != null)
                    {
                        demoReadyText.text = "Ready";
                        demoReadyText.gameObject.SetActive(true);
                    }
                    if (playerID == PlayerID.Player1)
                        demoPlayer1Ready = true;
                    else
                        demoPlayer2Ready = true;

                    if (demoPlayer1Ready && demoPlayer2Ready && !demoTransitionStarted)
                    {
                        demoTransitionStarted = true;
                        StartCoroutine(DemoEndSequence());
                    }
                }

                if (Input.GetKeyDown(emergencyRightKey) && isright)
                {
                    danger -= 2;
                    isright = false;
                    transform.rotation = Quaternion.Euler(0, 0, -37.6f);
                }

                if (Input.GetKeyDown(emergencyLeftKey) && !isright)
                {
                    danger -= 2;
                    isright = true;
                    transform.rotation = Quaternion.Euler(0, 0, 37.6f);
                }

                if (danger <= 0)
                    isemergency = false;
            }
            else
            {
                CloggedUI.SetActive(false);
                cloggedSoundPlayed = false;

                if (demoMode && demoReadyText != null)
                {
                    demoReadyText.text = "Not Ready";
                    demoReadyText.gameObject.SetActive(true);
                }
            }
        }

        if (score >= scoreCap1 && score < scoreCap2 && !sickomode)
        {
            CurrentsubtractionInterval = subtractionInterval;
            ChurnedWell1.sprite = ChurnWIMG1;
            ChurnedWell2.sprite = ChurnWIMG2;

            if (!speed1)
            {
                StartCoroutine(ActivateSpriteCoroutine());
                speed1 = true;
            }
        }

        if (score >= scoreCap2 && !sickomode)
        {
            CurrentsubtractionInterval = subtractionInterval2;

            if (!speed2)
            {
                StartCoroutine(ActivateSpriteCoroutine());
                speed2 = true;
            }
        }

        if (sickomode)
        {
            CurrentsubtractionInterval = subtractionInterval3;

            if (!speed3)
            {
                StartCoroutine(ActivateSpriteCoroutine());
                speed3 = true;
            }
        }

        if (sickomode)
        {
            if (sickoBlinkRoutine == null)
                sickoBlinkRoutine = StartCoroutine(SickoBlink());
        }
        else
        {
            if (sickoBlinkRoutine != null)
            {
                StopCoroutine(sickoBlinkRoutine);
                sickoBlinkRoutine = null;
            }

            if (SickoModeUI != null)
                SickoModeUI.SetActive(false);
        }

        DangerUI.SetActive(danger >= 35);

        if (!demoMode && !canInteract && winningChurn != null && endHoldFillUI != null)
        {
            if (Input.GetKey(winningChurn.churnKey))
            {
                holdTimer += Time.deltaTime;
                endHoldFillUI.fillAmount = holdTimer / holdDuration;

                if (holdTimer >= holdDuration)
                {
                    if (!string.IsNullOrEmpty(nextSceneName))
                        SceneManager.LoadScene(nextSceneName);
                }
            }
            else
            {
                holdTimer = Mathf.Max(0, holdTimer - Time.deltaTime);
                endHoldFillUI.fillAmount = holdTimer / holdDuration;
            }
        }
    }

    private void EnterEndState(ButterChurn winner)
    {
        winningChurn = winner;
        canInteract = false;
        holdTimer = 0f;

        if (endHoldFillUI != null)
            endHoldFillUI.fillAmount = 0f;
    }

    private IEnumerator SickoBlink()
    {
        while (sickomode)
        {
            if (SickoModeUI != null)
                SickoModeUI.SetActive(!SickoModeUI.activeSelf);

            yield return new WaitForSeconds(0.5f);
        }

        if (SickoModeUI != null)
            SickoModeUI.SetActive(false);
    }

    public void AddScore(int amount)
    {
        if (sickomode)
            amount *= 3;
        else if (FillUI != null)
        {
            float fill = FillUI.fillAmount;
            if (fill > 0.685f) amount *= 2;
            else if (fill < 0.316f) amount /= 2;
        }

        score += amount;
        UpdateScoreText();
    }

    public void SubtractScore(int amount)
    {
        score -= amount;
        if (score < 0) score = 0;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
        FinalscoreText.text = score.ToString();
    }

    private IEnumerator SubtractVariable()
    {
        while (true)
        {
            yield return new WaitForSeconds(CurrentsubtractionInterval);
            if (danger < 50)
                danger -= subtractionAmount;
        }
    }

    IEnumerator ActivateSpriteCoroutine()
    {
        SpeedUP.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        SpeedUP.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        SpeedUP.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        SpeedUP.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        SpeedUP.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        SpeedUP.SetActive(false);
    }

    private IEnumerator DemoEndSequence()
    {
        ButterChurn[] churns = FindObjectsOfType<ButterChurn>();
        ButterChurn player1 = null;
        ButterChurn player2 = null;

        foreach (var c in churns)
        {
            if (c.playerID == PlayerID.Player1) player1 = c;
            else if (c.playerID == PlayerID.Player2) player2 = c;
        }

        if (player1 != null && player2 != null)
        {
            ButterChurn winner = (player1.score >= player2.score) ? player1 : player2;

            foreach (var c in churns)
                c.EnterEndState(winner);

            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.RecordScore(player1.score);
                scoreManager.RecordScore(player2.score);
            }
        }

        gameEnded = true;

        if (Camera.main != null)
            Camera.main.transform.position += Random.insideUnitSphere * 0.15f;

        if (fadeImage != null)
        {
            float t = 0;
            Color c = fadeImage.color;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(0, 1, t / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
        }

        if (!string.IsNullOrEmpty(demoNextSceneName))
        {
            yield return new WaitForSeconds(sceneLoadDelay);
            SceneManager.LoadScene(demoNextSceneName);
        }
    }
}
