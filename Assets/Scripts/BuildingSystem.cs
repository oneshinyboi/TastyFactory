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
            if (_placementIndicator == null)
                _placementIndicator = InitializeObject(placementIndicatorPrefab, GetMouseWorldPosition()).GetComponent<PlacementIndicator>();
            _placementIndicator.IsEnabled = !_placementIndicator.IsEnabled;
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

    private void ShowPlacementIndicator()
    {
        
    }
    public GameObject InitializeObject(GameObject prefab, Vector3 worldPosition)
    {
        Vector3 position = SnapCoordinateToGrid(worldPosition);
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        _objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
        return obj;
    }
    #endregion
    
}
