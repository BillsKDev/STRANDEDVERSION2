using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{

	[SerializeField] private GameObject healthBar;
	[SerializeField] private float healthBarWidth;
	private float healthBarWidthSmooth;
	[SerializeField] private float healthBarWidthEase;
	public Animator animator;
	public int health;
	public int maxHealth;

	private static HealthUI instance;
	public static HealthUI Instance
	{
		get
		{
			if (instance == null) instance = GameObject.FindObjectOfType<HealthUI>();
			return instance;
		}
	}

	void Start()
	{
		healthBarWidth = 1;
		healthBarWidthSmooth = healthBarWidth;
	}

	void Update()
	{
		healthBarWidth = (float)HealthUI.Instance.health / (float)HealthUI.Instance.maxHealth;
		healthBarWidthSmooth += (healthBarWidth - healthBarWidthSmooth) * Time.deltaTime * healthBarWidthEase;
		healthBar.transform.localScale = new Vector2(healthBarWidthSmooth, transform.localScale.y);
	}

    public void HealthBarHurt() => animator.SetTrigger("hurt");
}

