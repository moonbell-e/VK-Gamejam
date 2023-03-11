using UnityEngine;

namespace Grid
{
    public class GreedsKeeper : MonoBehaviour
    {
        [SerializeField] private GridObject[] _grids;

        public bool TryPlaceObject(Vector2 point, PlaceableObject obj)
        {
            return _grids[0].TryPlaceObject(point, obj);
        }

        public bool TryTakeObject(Vector2 point, out PlaceableObject obj)
        {
            return _grids[0].TryTakeObject(point, out obj);
        }
    }
}