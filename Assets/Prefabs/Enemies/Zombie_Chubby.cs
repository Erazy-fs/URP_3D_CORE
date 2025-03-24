using UnityEngine;

public class Zombie_Chubby : Enemy
{
    public bool isRunner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        movingAnimationName = isRunner ? "Run" : "Walk";
        base.Start();
    }
}
