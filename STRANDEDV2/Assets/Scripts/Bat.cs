using System.Collections;
using UnityEngine;

public class Bat : MonoBehaviour
{
    Animator animator;
    AudioSource audioPlayer;
    public AudioClip Attack;
    bool canSwing = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSwing == true)
            StartCoroutine(SwordSwing());
    }

    IEnumerator SwordSwing()
    {
        animator.Play("Attack");
        audioPlayer.PlayOneShot(Attack);
        canSwing = false;
        yield return new WaitForSeconds(0.8f);
        animator.Play("Idle");
        canSwing = true;
    }

}