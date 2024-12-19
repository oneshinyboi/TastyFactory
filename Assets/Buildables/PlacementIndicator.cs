using System;
using UnityEngine;

namespace Buildables
{
    public class PlacementIndicator : MonoBehaviour
    {
        private bool _isEnabled;
        [SerializeField] private GameObject placementIndicator;
        [SerializeField] private GameObject arrow;
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
            arrow.SetActive(true);
            Vector3 pos = arrow.transform.position;
            pos.y = minHeight + 0.5f;
            arrow.transform.position = pos;
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
            arrow.SetActive(false);
        }
        private void Update()
        {
            if (!_isEnabled) return;

            Vector3 pos = BuildingSystem.GetMouseBuildingPlanePosition() + _offset;
            transform.position = BuildingSystem.Current.SnapCoordinateToGrid(pos);

            if (_isLinearScaling)
            {
                placementIndicator.transform.localScale =
                    Vector3.Lerp(placementIndicator.transform.localScale, _targetScale, Time.deltaTime * LinearAnimationSpeed);
            
                if (_showDirectionArrow)
                    arrow.transform.localScale =
                        Vector3.Lerp(arrow.transform.localScale, _arrowTargetScale, Time.deltaTime * LinearAnimationSpeed);
                
                if (Math.Abs(placementIndicator.transform.localScale.magnitude - _targetScale.magnitude) < 0.1)
                    _isLinearScaling = false;
            }
            else //animate
            {
                float pulse = Mathf.Sin(Time.time * SinAnimatoinSpeed) * 0.1f + 1f;
                Vector3 animated = 
                    placementIndicator.transform.localScale = new Vector3(
                        _targetScale.x * pulse,
                        placementIndicator.transform.localScale.y,
                        _targetScale.z * pulse
                    );
                if (_showDirectionArrow)
                    arrow.transform.localScale = new Vector3(
                        _arrowTargetScale.x * pulse,
                        arrow.transform.localScale.y,
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
            arrow.SetActive(false);
            transform.rotation = Quaternion.identity;
            _isEnabled = false;
            _showDirectionArrow = false;
        }
    }
}