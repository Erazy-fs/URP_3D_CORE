using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MenuClickSound : MonoBehaviour
{
    [SerializeField] private AudioSource _clickSound;

    private void Start()
    {
        foreach (Selectable element in GetComponentsInChildren<Selectable>())
        {
            switch (element)
            {
                case Button button:
                    button.onClick.AddListener(PlayClickSound);
                    break;
                case Slider slider:
                    StartCoroutine(DelayedListener(() => slider.onValueChanged.AddListener(_ => PlayClickSound())));
                    break;
                case Toggle toggle:
                    StartCoroutine(DelayedListener(() => toggle.onValueChanged.AddListener(_ => PlayClickSound())));
                    break;
                case TMP_Dropdown dropdown:
                    StartCoroutine(DelayedListener(() => dropdown.onValueChanged.AddListener(_ => PlayClickSound())));
                    AddClickSoundToDropdown(dropdown);
                    break;
            }
        }
    }

    private void PlayClickSound()
    {
        _clickSound?.Play();
    }

    private IEnumerator DelayedListener(System.Action action)
    {
        yield return null;
        action.Invoke();
    }

    private void AddClickSoundToDropdown(TMP_Dropdown dropdown)
    {
        EventTrigger trigger = dropdown.gameObject.GetComponent<EventTrigger>() ?? dropdown.gameObject.AddComponent<EventTrigger>();
        
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        entry.callback.AddListener(_ => PlayClickSound());
        trigger.triggers.Add(entry);
    }
}
