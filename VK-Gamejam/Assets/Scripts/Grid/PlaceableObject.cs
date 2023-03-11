using UnityEngine;

namespace Grid
{
    public class PlaceableObject : MonoBehaviour
    {
        [Header ("Size")]
        [SerializeField] private int _x;
        [SerializeField] private int _y;

        private Transform _transform;
        private Vector2Int _pivotPoint;

        public int X => _x;
        public int Y => _y;
        public Vector2Int PivotPoint => _pivotPoint;

        private void Awake()
        {
            _transform = transform;
        }

        public void Place(Vector2 point, int x, int y)
        {
            _transform.position = point;
            _pivotPoint = new Vector2Int(x, y);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
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