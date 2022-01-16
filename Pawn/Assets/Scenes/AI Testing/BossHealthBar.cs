using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private Image HealthBar;
    private float CurrentHealth;
    private float MaxHealth;
    BossAI Boss;
    float lerpSpeed;
    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;
    [HideInInspector]
    public GameObject Cristal;

    void Start()
    {
        gradient = new Gradient();
        colorKey = new GradientColorKey[3];
        colorKey[0].color = new Color(0.8f, 0.3f, 0.3f);
        colorKey[0].time = 0.2f;
        colorKey[1].color = new Color(0.9f, 0.9f, 0.3f);
        colorKey[1].time = 0.5f;
        colorKey[2].color = new Color(0.25f, 0.8f, 0.2f);
        colorKey[2].time = 1.0f;

        alphaKey = new GradientAlphaKey[3];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 0.5f;
        alphaKey[2].alpha = 1.0f;
        alphaKey[2].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

        HealthBar = GetComponent<Image>();
        Boss = FindObjectOfType<BossAI>();
        lerpSpeed = 3f * Time.deltaTime;
        //Nota: Esto puede dar lugar a fallos si de alguna manera la vida máxima de Pawn aumenta durante el juego
        MaxHealth = Boss.max_health;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentHealth = Boss.cur_health;
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, CurrentHealth / MaxHealth, lerpSpeed);
        //HealthBar.color = gradient.Evaluate(CurrentHealth / MaxHealth);
    }
}
