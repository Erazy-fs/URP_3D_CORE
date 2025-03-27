using System.Collections;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    public float delay = 1f;

    public GameObject bulletSingle;
    public GameObject bulletBurst;
    public float velocity = 7f;
    private int burstCount = 3;
    public Vector3 bulletOffset    = new Vector3(-2.255f, .163f, .026f);
    public Vector3 bulletDirection = new Vector3(1, 0, 1);

    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private IEnumerator Shooting;
    private bool trigger = false;
    private int curBurst = 0;
    IEnumerator ShootCoroutine(bool wait, bool auto){
        trigger = true;
        if (wait) yield return new WaitForSeconds(.3f);



        if (auto) {
            animator.SetBool("burst", true);
            while (trigger) {
                if (curBurst < burstCount) {
                    var pos = transform.position + transform.TransformDirection(bulletOffset);
                    var bullet = Instantiate(bulletBurst);
                    bullet.transform.position = pos;
                    var rb = bullet.GetComponent<Rigidbody>();                
                    //Я ненавижу вектора Я ненавижу вектора Я ненавижу вектора
                    rb.linearVelocity = new Vector3(-transform.forward.z, transform.forward.y, transform.forward.x) * velocity;
                    curBurst++;

                    if (curBurst==burstCount) {
                        animator.SetBool("burst", false);
                        yield return new WaitForSeconds(delay);
                        curBurst = 0;
                        animator.SetBool("burst", true);
                    } else {
                        yield return new WaitForSeconds(.5f/burstCount);
                    }
                }
            }
            animator.SetBool("burst", false);
        } else {
            var pos = transform.position + transform.TransformDirection(bulletOffset);
            var bullet = Instantiate(bulletSingle);
            bullet.transform.position = pos;
            var rb = bullet.GetComponent<Rigidbody>();            
            rb.linearVelocity = new Vector3(-transform.forward.z, transform.forward.y, transform.forward.x) * velocity;
            animator.SetTrigger("shoot");
            yield return new WaitForSeconds(delay);
        }

        Shooting = null;
    }

    public void Shoot(bool wait, bool auto){
        if (Shooting is not null) return;
        curBurst = 0;
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
