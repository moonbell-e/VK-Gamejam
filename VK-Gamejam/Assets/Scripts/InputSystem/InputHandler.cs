using UnityEngine;
using Grid;

namespace InputSystem
{ 
    public class InputHandler : MonoBehaviour
    {
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
                _objectInHand.Place(curcourPoint);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (_objectInHand != null)
                {
                    if (_gridsKeeper.TryPlaceObject(curcourPoint, _objectInHand)) _objectInHand = null;
                } 
                else
                {
                    if (_gridsKeeper.TryTakeObject(curcourPoint, out _objectInHand) == false)
                        _objectInHand = null;
                }
            }
        }

        public void TakeObjectInHand(PlaceableObject obj)
        {
            _objectInHand = obj;
        }
    }
}