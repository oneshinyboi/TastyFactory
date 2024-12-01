using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Current;
    public GridLayout gridLayout;

    private Grid _grid;
    private bool _buildMode = false;
    private bool _placementMode = false;
    
    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private TileBase whiteTile;

    public GameObject placementIndicatorPrefab;
    public GameObject conveyerPrefab;
    
    private PlacementIndicator _placementIndicator;
    private PlaceableObject _objectToPlace;

    #region UnityMethods

    private void Awake()
    {
            Current = this;
            _grid = gridLayout.GameObject().GetComponent<Grid>();
            _placementIndicator = InitializeObject(placementIndicatorPrefab, GetMouseWorldPosition()).GetComponent<PlacementIndicator>();
            _placementIndicator.IsEnabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            InitializeObject(conveyerPrefab, GetMouseWorldPosition());

        if (Input.GetKeyDown(KeyCode.B))
        {
            _placementIndicator.IsEnabled = !_placementIndicator.IsEnabled;
        }

        if (_placementMode && !_objectToPlace.isPlaced)
        {
            // Have it follow the mouse
            Vector3 pos = GetMouseWorldPosition();
            _objectToPlace.transform.position = BuildingSystem.Current.SnapCoordinateToGrid(pos);
            if (Input.GetKeyDown(KeyCode.R))
            {
                _objectToPlace.transform.Rotate(0, 90, 0);
                _placementIndicator.transform.Rotate(0, 90, 0);
            }
        }

        if (_placementMode && _objectToPlace.isPlaced)
        {
            _placementMode = false;
            _placementIndicator.IsEnabled = false;
        }
    }
    
    #endregion
    
    #region Utils
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
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
        GameObject obj = InitializeObject(prefab, GetMouseWorldPosition());
        _objectToPlace = obj.GetComponent<PlaceableObject>();
        if (doPlacementArrow)
            _placementIndicator.UsePlacementArrow(prefab.GetComponent<Collider>().bounds.size.y);
        _placementIndicator.IsEnabled = true;
        _placementMode = true;
    }

    #endregion

}
