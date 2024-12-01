using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 _offset;
    public bool dragable = true;
    private void OnMouseDown()
    {
        _offset = transform.position - BuildingSystem.GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if (!dragable) return;
        Vector3 pos = BuildingSystem.GetMouseWorldPosition() + _offset;
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
