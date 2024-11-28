using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

namespace SoundPlayers
{
    public class PlayOnClick : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private EventReference _soundPath;

        public void OnPointerClick(PointerEventData eventData)
        {
            RuntimeManager.PlayOneShot(_soundPath);
        }
    }
}

