using UnityEngine;
using System.Collections.Generic;

namespace Grid
{
    public class GridsKeeper : MonoBehaviour
    {
        [SerializeField] GridObject[] _startGrids;

        [SerializeField] PlaceableObject[] _startPlaceables;

        private List<GridObject>[] _gridsLayers = new List<GridObject>[20];

        private void Start()
        {
            for (int i = 0; i < _gridsLayers.Length; i++)
                _gridsLayers[i] = new List<GridObject>();

            foreach (GridObject grid in _startGrids)
                _gridsLayers[grid.Layer].Add(grid);

            foreach(var obj in _startPlaceables)
                TryPlaceObject(obj.Position, obj);
        }

        public bool TryPlaceObject(Vector2 point, PlaceableObject obj)
        {
            for (int i = _gridsLayers.Length - 1; i >= 0; i--)
            {
                foreach(var grid in _gridsLayers[i])
                    if(grid.TryPlaceObject(point, obj) == true) return true;
            }
            
            return false;
        }

        public bool TryTakeObject(Vector2 point, out PlaceableObject obj)
        {
            obj = null;
            for (int i = _gridsLayers.Length - 1; i >= 0; i--)
            {
                foreach (var grid in _gridsLayers[i])
                    if (grid.TryTakeObject(point, out obj)) return true;
            }
            return false;
        }

        public void AddGrid(GridObject grid)
        {
            _gridsLayers[grid.Layer].Add(grid);
        }
    }
}