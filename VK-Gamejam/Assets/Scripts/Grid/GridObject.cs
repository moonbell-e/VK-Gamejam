using UnityEngine;

namespace Grid
{
    public class GridObject : MonoBehaviour
    {
        [SerializeField] private Transform _topCorner;
        [SerializeField] private Transform _bottomCorner;
        [SerializeField] private Transform _leftCorner;
        [SerializeField] private Transform _rightCorner;

        [SerializeField] private int _x;
        [SerializeField] private int _y;

        public Vector2 TopCorner => _topCorner.position;
        public Vector2 BottomCorner => _bottomCorner.position;
        public Vector2 LeftCorner => _leftCorner.position;
        public Vector2 RightCorner => _rightCorner.position;

        public bool IsInBorders(Vector2 point)
        {
            if (IsBetweenLines(point, new Line(LeftCorner, BottomCorner), new Line(TopCorner, RightCorner))
                && IsBetweenLines(point, new Line(LeftCorner, TopCorner), new Line(BottomCorner, RightCorner))) return true;
            return false;
        }

        private bool IsBetweenLines(Vector2 point, Line leftLine, Line rightLine)
        {
            if (point.x < leftLine.XforY(point.y)) return false;
            if (point.x > rightLine.XforY(point.y)) return false;
            return true;
        }

        private void OnDrawGizmos()
        {
            for(int x = 0; x < _x; x++)
            {
                for(int y = 0; y <_y; y++)
                {
                    
                }
            }
        }
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
    }
}
