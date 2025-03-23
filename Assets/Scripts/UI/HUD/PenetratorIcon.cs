using UnityEngine;

public class PenetratorIcon : MonoBehaviour
{
    PenetratorInteraction _penetratorInteraction;
    public PenetratorInteraction PenetratorInteraction 
    {
        set
        {
            _penetratorInteraction = value;
            _penetratorInteraction.OnPenetratorArrived += UpdatePenetratorIcon;
        }
    }

    void Start()
    {
        // if (PenetratorInteraction is not null)
        //     PenetratorInteraction.OnPenetratorArrived += UpdatePenetratorIcon;
    }

    public void UpdatePenetratorIcon(bool isArrived)
    {
        gameObject.SetActive(!isArrived);
    }
}
