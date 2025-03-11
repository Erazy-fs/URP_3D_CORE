using UnityEngine;

public class follow_script : MonoBehaviour
{
    [SerializeField] GameObject Target;
    [SerializeField] float Radius = 0.1f;
    
    private float TargetHeight = 1;
    void Start()
    {
        var collider = GetComponent<Collider>();
        TargetHeight = collider.bounds.size.y + 1f;
        Debug.Log(TargetHeight);
    }

    // Update is called once per frame
    void Update()
    {
        var d = Vector3.Distance(gameObject.transform.position, Target.transform.position);
        Debug.Log($"{d}, {Target.name}, {Target.transform.position}, {gameObject.transform.position}");
        if (d > Radius) {
            var newPosition = Vector3.MoveTowards(gameObject.transform.position, Target.transform.position, 0.02f);
            gameObject.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
        } 
        // else 
        // {
        //     var angle = Time.time * 1f;
        //     gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(
        //         Target.transform.position.x + Mathf.Cos(angle) * Radius,
        //         Target.transform.position.y + TargetHeight,
        //         Target.transform.position.z + Mathf.Sin(angle) * Radius
        //     ), 0.01f);
        // }
    }
}
