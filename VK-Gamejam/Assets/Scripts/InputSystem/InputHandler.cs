using UnityEngine;
using Grid;
using FMODUnity;

namespace InputSystem
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private EventReference _pickupSound;
        [SerializeField] private EventReference _placeSound;
        [SerializeField] private EventReference _rotateSound;

        private GridsKeeper _gridsKeeper;
        private Camera _camera;
        private static InputHandler _instance;
        private PlaceableObject _objectInHand;

        public PlaceableObject ObjectInHand => _objectInHand;
        public static InputHandler Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            _gridsKeeper = FindObjectOfType<GridsKeeper>();
            _camera = Camera.main;
        }

        private void Update()
        {
            Vector2 curcourPoint = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (_objectInHand != null)
            {
                _objectInHand.Move(curcourPoint);
                if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Mouse1))
                {
                    RotateObjectInHand();
                }
            }
                

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
                    {
                        _objectInHand.TakeItem();
                        RuntimeManager.PlayOneShot(_pickupSound);
                    } 
                }
            }
        }

        public void RotateObjectInHand()
        {
            if (_objectInHand.RotateObject())
                RuntimeManager.PlayOneShot(_rotateSound);
        }

        public bool TakeObjectInHand(PlaceableObject obj)
        {
            if (_objectInHand != null)
            {
                Debug.Log("fail to take item");
                return false;
            }
            obj.TakeItem();
            _objectInHand = obj;
            return true;
        }
    }
}