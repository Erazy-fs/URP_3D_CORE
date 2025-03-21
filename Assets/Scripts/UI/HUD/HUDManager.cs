using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject _hudPanel;
    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.I) && Input.GetKey(KeyCode.LeftControl))
        {
            if (_hudPanel is not null)
                _hudPanel.SetActive(!_hudPanel.activeSelf);
        }
        #endif
    }
}
