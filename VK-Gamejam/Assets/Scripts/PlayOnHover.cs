using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

namespace SoundPlayers
{
    public class PlayOnHover : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private EventReference _soundPath;

        public void OnPointerEnter(PointerEventData eventData)
        {
            RuntimeManager.PlayOneShot(_soundPath);    
        } 
    }
}