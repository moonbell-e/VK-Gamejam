using UnityEngine;
using Grid;

namespace InputSystem
{
    public class TakeRandomInHand : MonoBehaviour
    {
        [SerializeField] private GameObject[] placeablePrefabs;
        [SerializeField] private InputHandler _handler;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
                ChooseRandomItem();
        }

        private void ChooseRandomItem()
        {
            var selected = Instantiate(placeablePrefabs[Random.Range(0, placeablePrefabs.Length)]).GetComponent<PlaceableObject>();
            _handler.TakeObjectInHand(selected);
        }
    }
}