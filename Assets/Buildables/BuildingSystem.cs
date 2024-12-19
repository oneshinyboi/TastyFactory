using System;
using Buildables.vfx;
using Unity.VisualScripting;
using UnityEngine;

namespace Buildables
{
    public class BuildingSystem : MonoBehaviour
    {
        public static BuildingSystem Current;
        public GridLayout gridLayout;
        public GameObject placementIndicatorPrefab;
        public GameObject placementDustEffectPrefab;

        private Grid _grid;
        private static LayerMask _buildableGround;
        private bool _placementMode = false;
    
    
        private PlacementIndicator _placementIndicator;
        private placementDustEffect _placementDustEffect;
        private PlaceableObject _placeableObjectToPlace;
        private GameObject _objectToPlace;

        #region UnityMethods

        private void Awake()
        {
            _buildableGround = LayerMask.GetMask("BuildableGround");
            Current = this;
            _grid = gridLayout.GameObject().GetComponent<Grid>();
            _placementIndicator = InitializeObject(placementIndicatorPrefab, GetMouseBuildingPlanePosition())
                .GetComponent<PlacementIndicator>();
            _placementIndicator.IsEnabled = false;
            _placementDustEffect = InitializeObject(placementDustEffectPrefab, Vector3.zero)
                .GetComponent<placementDustEffect>();
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (_placementMode)
            {
                // Have it follow the mouse
                Vector3 pos = GetMouseBuildingPlanePosition();
                _objectToPlace.transform.position = Current.SnapCoordinateToGrid(pos);
                if (Input.GetKeyDown(KeyCode.R))
                {
                    _objectToPlace.transform.Rotate(0, 90, 0);
                    _placementIndicator.transform.Rotate(0, 90, 0);
                }
            }

            if (_placementMode && _placeableObjectToPlace.isPlaced)
            {
                _placementMode = false;
                _placementIndicator.IsEnabled = false;
                _placementDustEffect.transform.position = _objectToPlace.transform.position;
                _placementDustEffect.vfx.Play();
                StartCoroutine(MainCameraController.Current.CameraShake(0.05f, new Vector2(0f, 0.2f), 15f));
            }

            if (_placementMode && _placeableObjectToPlace.isCanceled)
            {
                _placementMode = false;
                _placementIndicator.IsEnabled = false;
                Destroy(_objectToPlace);
            }
        }
    
        #endregion
    
        #region Utils
        public static Vector3 GetMouseBuildingPlanePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Int32.MaxValue, _buildableGround))
                return hit.point;
            else return Vector3.zero;
        }

        public Vector3 SnapCoordinateToGrid(Vector3 position)
        {
            Vector3Int cellPos = gridLayout.WorldToCell(position);
            position = _grid.GetCellCenterWorld(cellPos);
            return position;
        }
        #endregion
    
        #region BuildingPlacement
    
        public GameObject InitializeObject(GameObject prefab, Vector3 worldPosition)
        {
            Vector3 position = SnapCoordinateToGrid(worldPosition);
            GameObject obj = Instantiate(prefab, position, Quaternion.identity);
            return obj;
        }

        public void ActivatePlacementMode(GameObject prefab, bool doPlacementArrow = false)
        {
            GameObject obj = InitializeObject(prefab, GetMouseBuildingPlanePosition());
            _placeableObjectToPlace = obj.GetComponent<PlaceableObject>();
            _objectToPlace = obj;
            if (doPlacementArrow)
                _placementIndicator.UsePlacementArrow(prefab.GetComponent<Collider>().bounds.size.y);
            _placementIndicator.IsEnabled = true;
            _placementMode = true;
        }

        #endregion

    }
}
