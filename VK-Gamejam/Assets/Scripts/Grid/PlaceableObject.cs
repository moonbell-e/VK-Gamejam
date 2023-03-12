using UnityEngine;

namespace Grid
{
    public class PlaceableObject : MonoBehaviour
    {
        [Header("Score")]
        [SerializeField] private int _score;
        [SerializeField] private bool _reverse;
        [SerializeField] private GridTypes _typeNeeded;
        [SerializeField] private int _bonus;

        [SerializeField] private bool _showGrid;

        [Header("Size")]
        [SerializeField] private int _x;
        [SerializeField] private int _y;

        [SerializeField] private GridObject[] _grids;
        
        private Transform _transform;
        private SpriteRenderer[] _sprites;
        private GridsKeeper _keeper;
        private Vector2Int _pivotPoint;


        public int Score => _score;
        public bool Reverse => _reverse;
        public GridTypes Type => _typeNeeded;
        public int Bonus => _bonus;
        public int X => _x;
        public int Y => _y;
        public Vector2Int PivotPoint => _pivotPoint;
        public GridObject[] Grids => _grids;
        public Vector2 Position => _transform.position;

        private void Awake()
        {
            _transform = transform;
            _sprites = GetComponentsInChildren<SpriteRenderer>();
            _keeper = FindObjectOfType<GridsKeeper>();
        }

        public void Place(Vector2 point, int x, int y, int layer)
        {
            _transform.position = point;
            _pivotPoint = new Vector2Int(x, y);
            _sprites[0].sortingOrder = -x - y;
            _sprites[0].sortingLayerName = layer.ToString();

            if (_grids == null) return;
            foreach (var grid in _grids)
            {
                _keeper.AddGrid(grid);
                grid.GenerateCells();
            }
        }

        public void TakeItem()
        {
            _sprites[0].sortingLayerName = "10";
        }

        public void Move(Vector2 point)
        {
            point.y -= 2 * GridGlobalParameters.SellSizeY;
            _transform.position = point;
        }

        public void RotateObject()
        {
            var x = _x;
            _x = _y;
            _y = x;

            foreach (var sprite in _sprites)
                sprite.flipX = !sprite.flipX;

            foreach (var grid in _grids)
                grid.Rotate();
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