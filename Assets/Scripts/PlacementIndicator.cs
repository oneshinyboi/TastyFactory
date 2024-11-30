using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlacementIndicator : MonoBehaviour
{
    private bool isEnabled;
    private GameObject placementIndicator;
    public bool IsEnabled
    {
        get => isEnabled;
        set
        {
            if (!value)
            {
                HideSelf(); // Hide with animation
            }
            else
            {
                ShowSelf(); // Show with animation
            }
        }
    }

    private Vector3 _offset;
    private Vector3 _targetScale;
    private float _linearAnimationSpeed = 10f;
    private float _sinAnimatoinSpeed = 5f;
    private bool _animating;
    private bool _isLinearScaling;

    private void Awake()
    {
        placementIndicator = transform.Find("Indicator").gameObject;
    }
    private void Update()
    {
        if (!isEnabled) return;

        Vector3 pos = BuildingSystem.GetMouseWorldPosition() + _offset;
        transform.position = BuildingSystem.Current.SnapCoordinateToGrid(pos);

        if (_isLinearScaling)
        {
            placementIndicator.transform.localScale =
                Vector3.Lerp(placementIndicator.transform.localScale, _targetScale, Time.deltaTime * _linearAnimationSpeed);
            if (placementIndicator.transform.localScale == _targetScale)
                _isLinearScaling = false;
        }
        else //animate
        {
            float pulse = Mathf.Sin(Time.time * _linearAnimationSpeed) * 0.1f + 1f;
            placementIndicator.transform.localScale = new Vector3(
                _targetScale.x * pulse,
                placementIndicator.transform.localScale.y,
                _targetScale.z * pulse
            );
        }
    }

    private void HideSelf()
    {
        _isLinearScaling = true;
        _targetScale = new Vector3(0f, transform.localScale.y, 0f);
        Invoke(nameof(DeactivateGameObject), 0.2f);
    }

    private void ShowSelf()
    {
        isEnabled = true;
        _isLinearScaling = true;
        gameObject.SetActive(true);
        _targetScale = new Vector3(1f, transform.localScale.y, 1f);
    }

    private void DeactivateGameObject()
    {
        gameObject.SetActive(false);
        isEnabled = false;
    }
}