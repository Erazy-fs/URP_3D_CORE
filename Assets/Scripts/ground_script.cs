using UnityEngine;

public class ground_script : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created



    void Start()
    {
        for (var i=0; i<transform.childCount; i++){
            Debug.Log(transform.GetChild(i).name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
