using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlacementIndicator : MonoBehaviour
{
    private bool _isEnabled;
    private GameObject _placementIndicator;
    private GameObject _arrow;
    public bool IsEnabled
    {
        get => _isEnabled;
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

    public void UsePlacementArrow(float minHeight)
    {
        _showDirectionArrow = true;
        _arrow.SetActive(true);
        Vector3 pos = _arrow.transform.position;
        pos.y = minHeight + 0.5f;
        _arrow.transform.position = pos;
    }

    private Vector3 _offset;
    private Vector3 _targetScale;
    private Vector3 _arrowTargetScale;

    private const float LinearAnimationSpeed = 10f;
    private const float SinAnimatoinSpeed = 5f;
    private bool _animating;
    private bool _isLinearScaling;
    private bool _showDirectionArrow;

    private void Awake()
    {
        _placementIndicator = transform.Find("Indicator").gameObject;
        _arrow = transform.Find("Arrow").gameObject;
        
        _arrow.SetActive(false);
    }
    private void Update()
    {
        if (!_isEnabled) return;

        Vector3 pos = BuildingSystem.GetMouseWorldPosition() + _offset;
        transform.position = BuildingSystem.Current.SnapCoordinateToGrid(pos);

        if (_isLinearScaling)
        {
            _placementIndicator.transform.localScale =
                Vector3.Lerp(_placementIndicator.transform.localScale, _targetScale, Time.deltaTime * LinearAnimationSpeed);
            
                if (_showDirectionArrow)
                    _arrow.transform.localScale =
                        Vector3.Lerp(_arrow.transform.localScale, _arrowTargetScale, Time.deltaTime * LinearAnimationSpeed);
                
            if (Math.Abs(_placementIndicator.transform.localScale.magnitude - _targetScale.magnitude) < 0.1)
                _isLinearScaling = false;
        }
        else //animate
        {
            float pulse = Mathf.Sin(Time.time * SinAnimatoinSpeed) * 0.1f + 1f;
            Vector3 animated = 
            _placementIndicator.transform.localScale = new Vector3(
                _targetScale.x * pulse,
                _placementIndicator.transform.localScale.y,
                _targetScale.z * pulse
            );
            if (_showDirectionArrow)
                _arrow.transform.localScale = new Vector3(
                    _arrowTargetScale.x * pulse,
                    _arrow.transform.localScale.y,
                    _arrowTargetScale.z * pulse
                );
        }
    }

    private void HideSelf()
    {
        _isLinearScaling = true;
        _targetScale = new Vector3(0f, transform.localScale.y, 0f);
        _arrowTargetScale = new Vector3(0f, transform.localScale.y, 0f);
        Invoke(nameof(DeactivateGameObject), 0.2f);
    }

    private void ShowSelf()
    {
        _isEnabled = true;
        _isLinearScaling = true;
        gameObject.SetActive(true);
        _targetScale = new Vector3(1.5f, transform.localScale.y, 1.5f);
        _arrowTargetScale = new Vector3(0.7f, 0.4f, 0.7f);
    }

    private void DeactivateGameObject()
    {
        gameObject.SetActive(false);
        _arrow.SetActive(false);
        _isEnabled = false;
        _showDirectionArrow = false;
    }
}