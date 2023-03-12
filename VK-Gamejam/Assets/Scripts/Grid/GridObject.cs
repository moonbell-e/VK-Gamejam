using UnityEngine;
using System.Collections.Generic;

namespace Grid
{
    public class GridObject : MonoBehaviour
    {
        [SerializeField] private bool _showGrid;

        [Header ("Corners")]
        [SerializeField] private Transform _topCorner;
        [SerializeField] private Transform _bottomCorner;
        [SerializeField] private Transform _leftCorner;
        [SerializeField] private Transform _rightCorner;

        [Header ("GridParameters")]
        [SerializeField] private int _gridLayer;
        [SerializeField] private GridTypes _type;

        private int _additionalLayer;
        private CellKeeper _cells;

        public Vector2 TopCorner => _topCorner.position;
        public Vector2 BottomCorner => _bottomCorner.position;
        public Vector2 LeftCorner => _leftCorner.position;
        public Vector2 RightCorner => _rightCorner.position;
        public int Layer => _gridLayer;
        public GridTypes Type => _type;
        public bool CanBeMoved => _cells.CanBeMoved;
        public int Size => _cells.X + _cells.Y;

        private void Awake()
        {
            GenerateCells();
        }

        public void Rotate()
        {
            Vector2 leftTop = _topCorner.position - _leftCorner.position;
            Vector2 leftBottom = _leftCorner.position - _bottomCorner.position;
            Vector2 rightBottom = _rightCorner.position - _bottomCorner.position;
            Vector3 pos = Vector3.zero;

            pos = rightBottom * (leftBottom.magnitude / rightBottom.magnitude);
            _rightCorner.position = _bottomCorner.position + pos;
            pos = leftBottom * (rightBottom.magnitude / leftBottom.magnitude);
            _leftCorner.position = _bottomCorner.position + pos;
            pos = leftTop * (leftBottom.magnitude / rightBottom.magnitude);
            _topCorner.position = _leftCorner.position + pos;
            GenerateCells();
        }

        public void GenerateCells()
        {
            _cells = new CellKeeper(TopCorner, BottomCorner, LeftCorner, RightCorner);
        }

        public void AddAdditionalLayer(int layer)
        {
            _additionalLayer = layer;
            _gridLayer += _additionalLayer;
        }

        public void RemoveAdditionalLayer()
        {
            _gridLayer -= _additionalLayer;
            _additionalLayer = 0;
        }

        public bool TryPlaceObject(Vector2 point, PlaceableObject obj)
        {
            if (IsInBorders(point))
            {
                Vector2Int[,] cellsPoints = new Vector2Int[obj.X, obj.Y];

                var cellPoint = _cells.PointOnCell(point);

                if (cellPoint.x + obj.X > _cells.X || cellPoint.y + obj.Y > _cells.Y) return false;
                
                for (int x = 0; x < obj.X; x++)
                {
                    for (int y = 0; y < obj.Y; y++)
                    {
                        cellsPoints[x, y] = new Vector2Int(cellPoint.x + x, cellPoint.y + y);
                        if (_cells.IsEmpty[cellPoint.x + x, cellPoint.y + y] == false) return false;
                    }
                }

                if (obj.Grids != null && obj.Grids.Length > 0)
                {
                    if (_gridLayer >= obj.Grids[0].Layer)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (var grid in obj.Grids)
                            grid.AddAdditionalLayer(Layer);
                    }
                }

                foreach (Vector2Int cell in cellsPoints)
                    _cells.PlaceObject(cell.x, cell.y, obj);
                obj.Place(_cells.Pivots[cellPoint.x, cellPoint.y], cellPoint.x, cellPoint.y, Layer);
                return true;
            }
            return false;
        }

        public bool TryTakeObject(Vector2 point, out PlaceableObject obj)
        {
            obj = null;
            if (IsInBorders(point))
            {
                var cellPoint = _cells.PointOnCell(point);
                if (_cells.IsEmpty[cellPoint.x, cellPoint.y]) return false;

                obj = _cells.TakeObject(cellPoint.x, cellPoint.y);
                if (obj.Grids != null && obj.Grids.Length > 0)
                {
                    foreach (var grid in obj.Grids)
                        if (grid.CanBeMoved == false) return false;

                    foreach (var grid in obj.Grids)
                        grid.RemoveAdditionalLayer();
                }

                for (int x = 0; x < obj.X; x++)
                {
                    for (int y = 0; y < obj.Y; y++)
                    {
                        _cells.CleareCell(obj.PivotPoint.x + x, obj.PivotPoint.y + y);
                    }
                }
                return true;
            }
            return false;
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
            if (_showGrid == false) return;

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
            
            Gizmos.color = Color.green;
            if (_cells.Pivots != null)
            {
                for (int x = 0; x < _cells.X; x++)
                {
                    for (int y = 0; y < _cells.Y; y++)
                    {
                        if (_cells.IsEmpty[x, y] == false) continue;

                        Vector3 point = _cells.Pivots[x, y];
                        point.y += GridGlobalParameters.SellSizeY;
                        Gizmos.DrawSphere(point, 0.025f);
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
        private bool _canBeMoved;
        private int _cellsTaken;

        private Vector2[,] _cellsPivots;

        private bool[,] _cellsStatus;
        private PlaceableObject[,] _placedObjects;

        public Vector2[,] Pivots => _cellsPivots;
        public bool[,] IsEmpty => _cellsStatus;
        public int X => _x;
        public int Y => _y;
        public bool CanBeMoved => _canBeMoved;

        public CellKeeper(Vector2 topCorner,
            Vector2 bottomCorner, Vector2 leftCorner, Vector2 rightCorner)
        {
            _topCorner = topCorner;
            _bottomCorner = bottomCorner;
            _leftCorner = leftCorner;
            _rightCorner = rightCorner;

            _x = Mathf.RoundToInt((_rightCorner.x - _bottomCorner.x) / GridGlobalParameters.SellSizeX);
            _y = Mathf.RoundToInt((_leftCorner.y - _bottomCorner.y) / GridGlobalParameters.SellSizeY);

            _canBeMoved = true;
            _cellsTaken = 0;
            _cellsPivots = new Vector2[_x,_y];
            _cellsStatus = new bool[_x,_y];
            _placedObjects = new PlaceableObject[_x, _y];

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

        public void PlaceObject(int x, int y, PlaceableObject obj)
        {
            _cellsStatus[x, y] = false;
            _cellsTaken++;
            if (_canBeMoved) _canBeMoved = false;
            _placedObjects[x, y] = obj;
        }

        public PlaceableObject TakeObject(int x, int y)
        {
            return _placedObjects[x, y];
        }

        public void CleareCell(int x, int y)
        {
            _cellsStatus[x, y] = true;
            _cellsTaken--;
            if (_cellsTaken == 0) _canBeMoved = true;
            _placedObjects[x, y] = null;
        }
    }

    public enum GridTypes
    { 
        Floor,
        Table
    }
}