using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject SickoModeUI;   // UI that should blink during sicko mode
    private Coroutine sickoBlinkRoutine;


    void Start()
    {
        AssignControls();
        CurrentsubtractionInterval = subtractionInterval;
        UpdateScoreText();
        StartCoroutine(SubtractVariable());
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

        if (danger < maxdanger)
        {
            if (Input.GetKeyDown(churnKey) && !isemergency)
            {
                float newY = (playerID == PlayerID.Player1) ? downPosY_Player1 : downPosY_Player2;

                transform.position = new Vector3(
                    transform.position.x,
                    newY,
                    transform.position.z
                );

                transform.rotation = Quaternion.Euler(0, 0, 0);

                danger += 2;
                AddScore(5);
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

            transform.position = new Vector3(
                transform.position.x,
                newY,
                transform.position.z
            );

            part.Play();
        }


        danger = Mathf.Clamp(danger, 0, 50);


        if (isemergency)
        {
            CloggedUI.SetActive(true);

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

        // ======= SICKO MODE UI BLINK =======
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
                SickoModeUI.SetActive(false); // ensure UI stays off
        }


        DangerUI.SetActive(danger >= 35);
    }

    private IEnumerator SickoBlink()
    {
        while (sickomode)
        {
            if (SickoModeUI != null)
                SickoModeUI.SetActive(!SickoModeUI.activeSelf);

            yield return new WaitForSeconds(0.5f);
        }

        // Safety: ensure UI is off when exiting
        if (SickoModeUI != null)
            SickoModeUI.SetActive(false);
    }


    // ======= SCORE =======
    public void AddScore(int amount)
    {
        // NEW RULE: Sicko mode overrides everything
        if (sickomode)
        {
            amount = amount * 3;
        }
        else
        {
            // Existing fill-based modifiers
            if (FillUI != null)
            {
                float fill = FillUI.fillAmount;

                if (fill > 0.685f)
                    amount = amount * 2;
                else if (fill < 0.316f)
                    amount = amount / 2;
            }
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
}
