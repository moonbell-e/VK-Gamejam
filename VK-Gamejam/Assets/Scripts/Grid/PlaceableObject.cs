using UnityEngine;
using GameManagers;

namespace Grid
{
    public class PlaceableObject : MonoBehaviour
    {
        [Header ("Parameters")]
        [SerializeField] private bool _showGrid;
        [SerializeField] private bool _noScoreChange;
        [SerializeField] private bool _movable;
        [SerializeField] private bool _rotatetable;
        
        [Header("Score")]
        [SerializeField] private PlaceableTypes _type;
        [SerializeField] private bool _reverse;
        [SerializeField] private GridTypes _typeNeeded;

        [Header("Size")]
        [SerializeField] private int _x;
        [SerializeField] private int _y;
        [SerializeField] private GridObject[] _grids;
        
        private Transform _transform;
        private SpriteRenderer[] _sprites;
        private GridsKeeper _keeper;
        private Vector2Int _pivotPoint;

        public bool Movable => _movable;
        public bool NoScoreChange => _noScoreChange;
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

        public int CountScore(GridTypes type)
        {
            int result, bonus;
            if (_noScoreChange) return 0;
            bonus = GetTypeScore(out result);
            if (_reverse)
            {
                if (type != _typeNeeded)
                    result += bonus;
            }
            else
            {
                if (type == _typeNeeded)
                    result += bonus;
            }
            return result;
        }

        public int GetTypeScore(out int result)
        {
            int bonus = 0;
            switch (_type)
            {
                case PlaceableTypes.Small:
                    result = GlobalParameters.SmallObjectScore;
                    bonus = GlobalParameters.SmallObjectBonus;
                    break;
                case PlaceableTypes.Medium:
                    result = GlobalParameters.MediumObjectScore;
                    bonus = GlobalParameters.MediumObjectBonus;
                    break;
                case PlaceableTypes.Large:
                    result = GlobalParameters.LargeObjectScore;
                    break;
                default:
                    result = 0;
                    break;
            }
            return bonus;
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
            if (_sprites.Length == 1 && _type != PlaceableTypes.Large)
                _sprites[0].sortingLayerName = "10";
        }

        public void Move(Vector2 point)
        {
            point.y -= 2 * GlobalParameters.SellSizeY;
            _transform.position = point;
        }

        [ContextMenu ("Rotate object")]
        public void RotateObject()
        {
            if (_rotatetable == false) return;

            var x = _x;
            _x = _y;
            _y = x;

            if (_sprites == null)
                _sprites = GetComponentsInChildren<SpriteRenderer>();

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
                point1.x = pivot.x + GlobalParameters.SellSizeX * x;
                point2.x = pivot.x + GlobalParameters.SellSizeX * (x - _x);

                point1.y = pivot.y + GlobalParameters.SellSizeY * x;
                point2.y = pivot.y + GlobalParameters.SellSizeY * (x + _x);

                point2 = point1 + (point2 - point1) * _y / _x;

                Gizmos.DrawLine(point1, point2);
            }

            for (int y = 0; y <= _y; y++)
            {
                point1.x = pivot.x - GlobalParameters.SellSizeX * y;
                point2.x = pivot.x + GlobalParameters.SellSizeX * (_y - y);

                point1.y = pivot.y + GlobalParameters.SellSizeY * y;
                point2.y = pivot.y + GlobalParameters.SellSizeY * (y + _y);

                point2 = point1 + (point2 - point1) * _x / _y;

                Gizmos.DrawLine(point1, point2);
            }
        }
#endif
    }

    public enum PlaceableTypes
    {
        Small,
        Medium,
        Large
    }
}