using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

public class ButtonClickFMOD : MonoBehaviour, IPointerClickHandler
{
    [EventRef]
    public string clickSoundEvent; // ������ �� ������� FMOD

    public void OnPointerClick(PointerEventData eventData)
    {
        // ����������� ���� ��� ������� �� ������
        RuntimeManager.PlayOneShot(clickSoundEvent);
    }
}
