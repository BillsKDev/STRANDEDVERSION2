using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [SerializeField] Animator anim;

    void Update()
    {
        if (PlayerMovement._playFire == true)
        {
            anim.SetBool("Aiming", true);
        }
        else
        {
            anim.SetBool("Aiming", false);
        }
    }
}