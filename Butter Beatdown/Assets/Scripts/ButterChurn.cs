using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButterChurn : MonoBehaviour
{
    public int score = 0;
    public int scoreCap1;
    public int scoreCap2;
    public TMP_Text scoreText;
    public TMP_Text FinalscoreText;
    public Image FillUI;
    public bool isStarted;
    public ParticleSystem part;

    public float danger; // Starting value of the variable
    public float maxdanger;
    public float subtractionAmount; // Amount to subtract from the variable
    public float CurrentsubtractionInterval; // Interval between each subtraction
    public float subtractionInterval; // Interval between each subtraction
    public float subtractionInterval2; // Interval between each subtraction
    public float subtractionInterval3; // Interval between each subtraction

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

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
        StartCoroutine(SubtractVariable());
    }

    // Update is called once per frame
    void Update()
    {
        FillUI.fillAmount = (danger / maxdanger);

        if (isStarted == true)
        {
            if (danger < maxdanger)
            {
                if (Input.GetKeyDown(KeyCode.Space) && isemergency == false)
                {
                    this.transform.position = new Vector3(0.2f, -1, 0);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    if (danger < maxdanger)
                    {
                        danger = danger + 2;
                        AddScore(5);
                    }
                }

                if (danger <= 25)
                {
                    DangerButt.sprite = NormalChurn;
                }

                if (danger > 25 && danger <= 35)
                {
                    DangerButt.sprite = MidChurn;
                }

                if (danger > 35)
                {
                    DangerButt.sprite = DangerChurn;
                }
            }
            else
            {
                isemergency = true;
            }

            if (Input.GetKeyUp(KeyCode.Space) && isemergency == false)
            {
                this.transform.position = new Vector3(0.2f, 2.19f, 0);
                //AddScore(5);
                part.Play();
            }

            if (danger < 0)
            {
                danger = 0;
            }

            if (danger > 50)
            {
                danger = 50;
            }

            if (isemergency == true)
            {
                CloggedUI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.RightArrow) & isright == true)
                {
                    danger = danger - 2;
                    isright = false;
                    this.transform.position = new Vector3(0.51f, -0.03f, 0);
                    transform.rotation = Quaternion.Euler(0, 0, -37.6f);
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow) & isright == false)
                {
                    danger = danger - 2;
                    isright = true;
                    this.transform.position = new Vector3(-0.14f, -0.03f, 0);
                    transform.rotation = Quaternion.Euler(0, 0, 37.6f);
                }

                if (danger <= 0)
                {
                    isemergency = false;
                }
            }
            else
            {
                CloggedUI.SetActive(false);
            }

            if (score >= scoreCap1 && score < scoreCap2 && sickomode == false)
            {
                CurrentsubtractionInterval = subtractionInterval;
                ChurnedWell1.sprite = ChurnWIMG1;
                ChurnedWell2.sprite = ChurnWIMG2;
                if(speed1 != true)
                {
                    StartCoroutine(ActivateSpriteCoroutine());
                    speed1 = true;
                }
            }
            if (score >= scoreCap2 && sickomode == false)
            {
                CurrentsubtractionInterval = subtractionInterval2;
                if (speed2 != true)
                {
                    StartCoroutine(ActivateSpriteCoroutine());
                    speed2 = true;
                }
            }
            if (sickomode == true)
            {
                CurrentsubtractionInterval = subtractionInterval3;
                if (speed3 != true)
                {
                    StartCoroutine(ActivateSpriteCoroutine());
                    speed3 = true;
                }
            }

            if (danger >= 35)
            {
                DangerUI.SetActive(true);
            }
            else
            {
                DangerUI.SetActive(false);
            }
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    public void SubtractScore(int amount)
    {
        score -= amount;
        if (score < 0)
        {
            score = 0;
        }
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "" + score.ToString();
        FinalscoreText.text = "" + score.ToString();
    }

    private IEnumerator SubtractVariable()
    {
        while (true)
        {
            yield return new WaitForSeconds(CurrentsubtractionInterval);
            if (danger < 50)
            {
                danger -= subtractionAmount;
                Debug.Log("Variable value: " + danger);
            }
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
