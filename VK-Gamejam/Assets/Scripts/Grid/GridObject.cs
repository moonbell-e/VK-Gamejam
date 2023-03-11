using UnityEngine;

namespace Grid
{
    public class GridObject : MonoBehaviour
    {
        [Header ("Corners")]
        [SerializeField] private Transform _topCorner;
        [SerializeField] private Transform _bottomCorner;
        [SerializeField] private Transform _leftCorner;
        [SerializeField] private Transform _rightCorner;

        private CellKeeper _cells;
        private Camera _camera;
        private Vector2Int _curCell;
        private bool _cellSelected;

        public Vector2 TopCorner => _topCorner.position;
        public Vector2 BottomCorner => _bottomCorner.position;
        public Vector2 LeftCorner => _leftCorner.position;
        public Vector2 RightCorner => _rightCorner.position;

        private void Awake()
        {
            _camera = Camera.main;
            _cells = new CellKeeper(TopCorner, BottomCorner, LeftCorner, RightCorner);
            _cellSelected = false;
            _curCell = Vector2Int.zero;
        }

        public void Update()
        {
            var point = _camera.ScreenToWorldPoint(Input.mousePosition);
            if(IsInBorders(point))
            {
                if (_cellSelected == false)
                {
                    Debug.Log("Target locked!");
                    _cellSelected = true;
                }
                else _cells.IsEmpty[_curCell.x, _curCell.y] = true;

                _curCell = _cells.PointOnCell(point);
                _cells.IsEmpty[_curCell.x, _curCell.y] = false;

                Debug.Log("Coordinate: X " + _curCell.x + " Y " + _curCell.y);
                return;
            }

            if (_cellSelected)
            {
                _cells.IsEmpty[_curCell.x, _curCell.y] = true;
                _cellSelected = false;
            }    
            Debug.Log("Lost target!");
        }

        public bool IsInBorders(Vector2 point)
        {
            if (IsBetweenLinesX(point, new Line(LeftCorner, BottomCorner), new Line(TopCorner, RightCorner))
                && IsBetweenLinesX(point, new Line(LeftCorner, TopCorner), new Line(BottomCorner, RightCorner))) return true;
            return false;
        }

        public static bool IsBetweenLinesX(Vector2 point, Line leftLine, Line rightLine)
        {
            if (point.x < leftLine.XforY(point.y)) return false;
            if (point.x > rightLine.XforY(point.y)) return false;
            return true;
        }

        public static bool IsBetweenLinesY(Vector2 point, Line leftLine, Line rightLine)
        {
            if (point.y < leftLine.YforX(point.x)) return false;
            if (point.y > rightLine.YforX(point.x)) return false;
            return true;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            int X = Mathf.RoundToInt((RightCorner.x - BottomCorner.x) / GridGlobalParameters.SellSizeX);
            int Y = Mathf.RoundToInt((LeftCorner.y - BottomCorner.y) / GridGlobalParameters.SellSizeY);

            if (X == 0 || Y == 0) return;

            Gizmos.color = Color.grey;

            Vector3 point1 = Vector3.zero;
            Vector3 point2 = Vector3.zero;
            
            Line hor1 = new Line(BottomCorner, RightCorner);
            Line hor2 = new Line(LeftCorner, TopCorner);

            for (int x = 0; x <= X; x++)
            {
                point1.x = BottomCorner.x + (RightCorner.x - BottomCorner.x) * x / X;
                point2.x = LeftCorner.x + (TopCorner.x - LeftCorner.x) * x / X;

                point1.y = hor1.YforX(point1.x);
                point2.y = hor2.YforX(point2.x);

                Gizmos.DrawLine(point1, point2);
            }

            Line ver1 = new Line(BottomCorner, LeftCorner);
            Line ver2 = new Line(RightCorner, TopCorner);

            for (int y = 0; y <= Y; y++)
            {
                point1.y = BottomCorner.y + (LeftCorner.y - BottomCorner.y) * y / Y;
                point2.y = RightCorner.y + (TopCorner.y - RightCorner.y) * y / Y;

                point1.x = ver1.XforY(point1.y);
                point2.x = ver2.XforY(point2.y);

                Gizmos.DrawLine(point1, point2);
            }

            if (_cells.Pivots != null)
            {
                for (int x = 0; x < _cells.X; x++)
                {
                    for (int y = 0; y < _cells.Y; y++)
                    {
                        if (_cells.IsEmpty[x, y]) Gizmos.color = Color.green;
                        else Gizmos.color = Color.red;
                        Gizmos.DrawSphere(_cells.Pivots[x, y], 0.025f);
                    }
                }
            }
        }
#endif
    }

    public struct Line
    {
        private Vector2 _leftPoint;
        private Vector2 _rightPoint;

        public Line(Vector2 leftPoint, Vector2 rightPoint)
        {
            _leftPoint = leftPoint;
            _rightPoint = rightPoint;
        }

        public float XforY(float y)
        {
            float x = (y - _leftPoint.y) * (_rightPoint.x - _leftPoint.x) / (_rightPoint.y - _leftPoint.y) + _leftPoint.x;
            return x;
        }

        public float YforX(float x)
        {
            float y = (x - _leftPoint.x) * (_rightPoint.y - _leftPoint.y) / (_rightPoint.x - _leftPoint.x) + _leftPoint.y;
            return y;
        }
    }

    public struct CellKeeper
    {
        private Vector2 _topCorner, _bottomCorner, _leftCorner, _rightCorner;
        private int _x, _y;

        private Vector2[,] _cellsPivots;

        private bool[,] _cellsStatus;

        public Vector2[,] Pivots => _cellsPivots;
        public bool[,] IsEmpty => _cellsStatus;
        public int X => _x;
        public int Y => _y;

        public CellKeeper(Vector2 topCorner,
            Vector2 bottomCorner, Vector2 leftCorner, Vector2 rightCorner)
        {
            _topCorner = topCorner;
            _bottomCorner = bottomCorner;
            _leftCorner = leftCorner;
            _rightCorner = rightCorner;

            _x = Mathf.RoundToInt((_rightCorner.x - _bottomCorner.x) / GridGlobalParameters.SellSizeX);
            _y = Mathf.RoundToInt((_leftCorner.y - _bottomCorner.y) / GridGlobalParameters.SellSizeY);
            
            _cellsPivots = new Vector2[_x,_y];
            _cellsStatus = new bool[_x,_y];
            
            Vector3 point1 = Vector3.zero;
            Vector3 point2 = Vector3.zero;
            Vector3 point3 = Vector3.zero;
            Vector3 point4 = Vector3.zero;

            Line hor1 = new Line(_bottomCorner, _rightCorner);
            Line hor2 = new Line(_leftCorner, _topCorner);
            Line ver1 = new Line(_bottomCorner, _leftCorner);
            Line ver2 = new Line(_rightCorner, _topCorner);

            for (int x = 0; x < _x; x++)
            {
                point1.x = _bottomCorner.x + (_rightCorner.x - _bottomCorner.x) * x / _x;
                point2.x = _leftCorner.x + (_topCorner.x - _leftCorner.x) * x / _x;

                point1.y = hor1.YforX(point1.x);
                point2.y = hor2.YforX(point2.x);

                for (int y = 0; y < _y; y++)
                {
                    point3.y = _bottomCorner.y + (_leftCorner.y - _bottomCorner.y) * y / _y;
                    point4.y = _rightCorner.y + (_topCorner.y - _rightCorner.y) * y / _y;

                    point3.x = ver1.XforY(point3.y);
                    point4.x = ver2.XforY(point4.y);

                    float q = (point2.x - point1.x) / (point1.y - point2.y);
                    float sn = (point3.x - point4.x) + (point3.y - point4.y) * q;
                    float fn = (point3.x - point1.x) + (point3.y - point1.y) * q;
                    float n = fn / sn;

                    Vector2 pivot = new Vector2((point3.x + (point4.x - point3.x) * n),
                        (point3.y + (point4.y - point3.y) * n));

                    _cellsPivots[x, y] = new Vector2(pivot.x, pivot.y);
                    _cellsStatus[x, y] = true;
                }
            }
        }

        public Vector2Int PointOnCell(Vector2 point)
        {
            Vector2Int result = new Vector2Int (-1, -1);

            Line rightTop = new Line(_topCorner, _rightCorner);
            Line leftTop = new Line(_leftCorner, _topCorner);
            for (int x = 1; x < _x; x++)
            {
                if (GridObject.IsBetweenLinesX(point, new Line(_cellsPivots[x, 0],
                    _cellsPivots[x, _y - 1]), rightTop) == false)
                {
                    result.x = x - 1;
                    break;
                }
            }
            if (result.x == -1) result.x = _x - 1;

            for (int y = 1; y < _y; y++)
            {
                if (GridObject.IsBetweenLinesY(point, new Line(_cellsPivots[0, y],
                    _cellsPivots[_x - 1, y]), leftTop) == false)
                {
                    result.y = y - 1;
                    break;
                }
            }
            if (result.y == -1) result.y = _y - 1;

            return result;
        }
    }
}