using UnityEngine;
using Grid;

namespace InputSystem
{ 
    public class InputHandler : MonoBehaviour
    {
        private GreedsKeeper _greedsKeeper;
        private Camera _camera;

        private PlaceableObject _objectInHand;

        private void Awake()
        {
            _greedsKeeper = FindObjectOfType<GreedsKeeper>();
            _camera = Camera.main;
        }

        private void Update()
        {
            Vector2 curcourPoint = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (_objectInHand != null)
                _objectInHand.Place(curcourPoint, 0, 0);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (_objectInHand != null)
                {
                    if (_greedsKeeper.TryPlaceObject(curcourPoint, _objectInHand)) _objectInHand = null;
                } 
                else
                {
                    _greedsKeeper.TryTakeObject(curcourPoint, out _objectInHand);
                }
            }
        }

        public void TakeObjectInHand(PlaceableObject obj)
        {
            _objectInHand = obj;
        }
    }
}