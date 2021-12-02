using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Script : MonoBehaviour
{
    // Start is called before the first frame update
    private Image HealthBar;
    private float CurrentHealth;
    private float MaxHealth;
    PlayerController Player;
    float lerpSpeed;
    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    private void Start()
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
        Player = FindObjectOfType<PlayerController>();
        lerpSpeed = 3f * Time.deltaTime;
        //Nota: Esto puede dar lugar a fallos si de alguna manera la vida máxima de Pawn aumenta durante el juego
        MaxHealth = Player.max_health;
    }

    private void Update()
    {
        CurrentHealth = Player.cur_health;
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, CurrentHealth / MaxHealth, lerpSpeed);
        HealthBar.color = gradient.Evaluate(CurrentHealth / MaxHealth);
        //HealthBar.color = Color.Lerp(new Color(0.8f, 0.3f, 0.3f), new Color(0.1f, 0.9f, 0.4f), CurrentHealth / MaxHealth);
        //HealthBar.fillAmount = Player.cur_health / Player.max_health;
    }
}
