using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

public class ButtonClickFMOD : MonoBehaviour, IPointerClickHandler
{
    [EventRef]
    public string clickSoundEvent; // Ссылка на событие FMOD

    public void OnPointerClick(PointerEventData eventData)
    {
        // Проигрываем звук при нажатии на кнопку
        RuntimeManager.PlayOneShot(clickSoundEvent);
    }
}
