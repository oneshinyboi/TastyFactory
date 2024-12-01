using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class BuildMenuController : MonoBehaviour
{
    public VisualElement ui;
    public GameObject conveyorPrefab;
    public GameObject ovenPrefab;
    
    private Button _conveyorButton;
    private Button _ovenButton;

    #region Unity functions
    private void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
        
    }

    private void OnEnable()
    {
        Debug.Log("gotHere");
        _conveyorButton = ui.Q<Button>("conveyorButton");
        _conveyorButton.RegisterCallback<ClickEvent>(evt => Buy(conveyorPrefab, true));

        _ovenButton = ui.Q<Button>("ovenButton");
        _ovenButton.RegisterCallback<ClickEvent>(evt => Buy(ovenPrefab));
    }

    

    #endregion
    #region Private functions
    private void Buy(GameObject obj, bool doPlacementArrow = false)
    {
        Debug.Log("Gotcha");
        PlaceableObject placeableObject = obj.GetComponent<PlaceableObject>();
        if (Player.Instance.CanBuy(placeableObject.price))
        {
            BuildingSystem.Current.ActivatePlacementMode(obj, doPlacementArrow);
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
