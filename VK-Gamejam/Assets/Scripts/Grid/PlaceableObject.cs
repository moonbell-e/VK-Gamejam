using UnityEngine;

namespace Grid
{
    public class PlaceableObject : MonoBehaviour
    {
        public bool _showGrid;

        [Header("Size")]
        [SerializeField] private int _x;
        [SerializeField] private int _y;

        [SerializeField] private GridObject _grid;

        private Transform _transform;
        private GridsKeeper _keeper;
        private Vector2Int _pivotPoint;

        public int X => _x;
        public int Y => _y;
        public Vector2Int PivotPoint => _pivotPoint;
        public GridObject Grid => _grid;

        private void Awake()
        {
            _transform = transform;
            _keeper = FindObjectOfType<GridsKeeper>();
        }

        public void Place(Vector2 point, int x, int y)
        {
            _transform.position = point;
            _pivotPoint = new Vector2Int(x, y);

            if (_grid == null) return;
            _keeper.AddGrid(_grid);
            _grid.GenerateCells();
        }

        public void Place(Vector2 point)
        {
            point.y -= 0.1f;
            _transform.position = point;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_showGrid == false) return;

            if (_x == 0 || _y == 0) return;
            if (_transform == null) _transform = transform;

            Vector3 pivot = _transform.position;
            pivot.z = 0;

            Gizmos.color = Color.black;

            Vector3 point1 = Vector3.zero;
            Vector3 point2 = Vector3.zero;

            for (int x = 0; x <= _x; x++)
            {
                point1.x = pivot.x + GridGlobalParameters.SellSizeX * x;
                point2.x = pivot.x + GridGlobalParameters.SellSizeX * (x - _x);

                point1.y = pivot.y + GridGlobalParameters.SellSizeY * x;
                point2.y = pivot.y + GridGlobalParameters.SellSizeY * (x + _x);

                point2 = point1 + (point2 - point1) * _y / _x;

                Gizmos.DrawLine(point1, point2);
            }

            for (int y = 0; y <= _y; y++)
            {
                point1.x = pivot.x - GridGlobalParameters.SellSizeX * y;
                point2.x = pivot.x + GridGlobalParameters.SellSizeX * (_y - y);

                point1.y = pivot.y + GridGlobalParameters.SellSizeY * y;
                point2.y = pivot.y + GridGlobalParameters.SellSizeY * (y + _y);

                point2 = point1 + (point2 - point1) * _x / _y;

                Gizmos.DrawLine(point1, point2);
            }
        }
#endif
    }
}