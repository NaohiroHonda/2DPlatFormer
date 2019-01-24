using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUISC : MonoBehaviour {

    private float oldValue, value, maxValue;

    [SerializeField]
    public float moveTime = 0.5f;

    private float time;
    private bool isMoving;

    [SerializeField]
    private Color maxColor = new Color(0f, 1f, 0f, 0.5f), zeroColor = new Color(1f, 0f, 0f, 0.5f);

    private Color colorNow, oldColor;

    private Image uiImage;

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        uiImage = gameObject.GetComponent<Image>();

        colorNow = maxColor;
        uiImage.color = maxColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) MoveBar();
    }

    public void Init(float maxValue)
    {
        this.maxValue = maxValue;

        oldValue = maxValue;
        value = maxValue;
    }

    public void SetValue(float value)
    {
        oldValue = this.value;
        this.value = value;

        oldColor = colorNow;

        colorNow = Color.Lerp(zeroColor, maxColor, value / maxValue);
        isMoving = true;
        time = 0f;
    }

    private void MoveBar()
    {
        time += Time.deltaTime;

        float timeRatio = time / moveTime;
        float fillRatio = value;


        uiImage.fillAmount = fillRatio;
        SetColor(timeRatio);


        if (time >= moveTime)
        {
            isMoving = false;
        }
    }

    private void SetColor(float ratio)
    {
        Color newColor = Color.Lerp(oldColor, colorNow, ratio);
        newColor.r = Mathf.Sin(newColor.r * Mathf.PI / 2f);
        newColor.g = Mathf.Sin(newColor.g * Mathf.PI / 2f);

        uiImage.color = newColor;
    }
}
