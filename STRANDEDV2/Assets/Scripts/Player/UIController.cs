﻿using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider healthSlider;
    public Text healthText, ammoText;

    public Image damageEffect;
    public float damageAlpha = .25f, damageFadeSpeed = 2f;

    public GameObject pauseScreen;

    public Image blackScreen;
    public float fadeSpeed = 1.5f;

    void Awake() => instance = this;

    void Update()
    {
        if (damageEffect.color.a != 0)
            damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, Mathf.MoveTowards(damageEffect.color.a, 0f, damageFadeSpeed * Time.deltaTime));

            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
    }

    public void ShowDamage() => damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, .25f);
}

