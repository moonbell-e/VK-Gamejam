using UnityEngine;
using Grid;
using FMODUnity;

namespace InputSystem
{ 
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private EventReference _pickupSound;
        [SerializeField] private EventReference _placeSound;

        private GridsKeeper _gridsKeeper;
        private Camera _camera;

        private PlaceableObject _objectInHand;

        private void Awake()
        {
            _gridsKeeper = FindObjectOfType<GridsKeeper>();
            _camera = Camera.main;
        }

        private void Update()
        {
            Vector2 curcourPoint = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (_objectInHand != null)
                _objectInHand.Move(curcourPoint);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (_objectInHand != null)
                {
                    if (_gridsKeeper.TryPlaceObject(curcourPoint, _objectInHand))
                    {
                        _objectInHand = null;
                        RuntimeManager.PlayOneShot(_placeSound);
                    }
                } 
                else
                {
                    if (_gridsKeeper.TryTakeObject(curcourPoint, out _objectInHand) == false)
                        _objectInHand = null;
                    else
                        RuntimeManager.PlayOneShot(_pickupSound);
                }
            }
        }

        public bool TakeObjectInHand(PlaceableObject obj)
        {
            if (_objectInHand != null)
            {
                Debug.Log("fail to take item");
                return false;
            }
            _objectInHand = obj;
            return true;
        }
    }
}