using UnityEngine;

namespace Buildables
{
    public class ObjectDrag : MonoBehaviour
    {
        private Vector3 _offset;
        public bool dragable = true;
        private void OnMouseDown()
        {
            _offset = transform.position - BuildingSystem.GetMouseBuildingPlanePosition();
        }

        private void OnMouseDrag()
        {
            if (!dragable) return;
            Vector3 pos = BuildingSystem.GetMouseBuildingPlanePosition() + _offset;
            transform.position = BuildingSystem.Current.SnapCoordinateToGrid(pos);
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
