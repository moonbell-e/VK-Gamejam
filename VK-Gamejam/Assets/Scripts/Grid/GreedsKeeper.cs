using UnityEngine;

namespace Grid
{
    public class GreedsKeeper : MonoBehaviour
    {
        [SerializeField] private GridObject[] _grids;

        public bool TryPlaceObject(Vector2 point, PlaceableObject obj)
        {
            foreach (var grid in _grids)
                if(grid.TryPlaceObject(point, obj) == true) return true;
            return false;
        }

        public bool TryTakeObject(Vector2 point, out PlaceableObject obj)
        {
            obj = null;
            foreach (var grid in _grids)
                if (grid.TryTakeObject(point, out obj) == true) return true;
            return false;
        }
    }
}