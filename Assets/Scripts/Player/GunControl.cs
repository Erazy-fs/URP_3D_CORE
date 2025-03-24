using System.Collections;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    public float delay = .07f;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private IEnumerator Shooting;
    private bool trigger = false;
    IEnumerator ShootCoroutine(bool wait, bool auto){
        trigger = true;
        Debug.Log(wait);
        if (wait) yield return new WaitForSeconds(.2f);
        var mode = auto ? "auto" : "fire";
        animator.SetBool(mode, true);
        if (auto) while (trigger)
            yield return null;
        else 
            yield return new WaitForSeconds(delay);
        animator.SetBool(mode, false);
        Shooting = null;
    }

    public void Shoot(bool wait, bool auto){
        if (Shooting is not null) return;
        Shooting = ShootCoroutine(wait, auto);
        StartCoroutine(Shooting);
    }

    public void StopShooting(){
        trigger = false;
    }

    void Update()
    {
        
    }
}
