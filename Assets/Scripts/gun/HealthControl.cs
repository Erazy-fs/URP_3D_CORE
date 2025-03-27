using System.Collections;
using UnityEngine;

public class HealthControl : MonoBehaviour
{

    void Start()
    {
        
    }

    public float health = 10f;
    public void ReceiveDamage(float value){
        if ((health -= value) <= 0) {
            StartCoroutine(Goodbye());
        }
    }
    
    IEnumerator Goodbye(){
        GetComponent<Animator>()?.SetBool("isDead", true);
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }

    void Update()
    {
        
    }
}
