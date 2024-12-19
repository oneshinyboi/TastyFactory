using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    public static MainCameraController Current;
    private Camera _camera;

    // Start is called before the first frame update
    #region UnityMethds 
    
    void Awake()
    {
        Current = this;
        _camera = GetComponent<Camera>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

   #region PublicMethods

   /// <summary>
   /// Shakes the camera for a specified duration and magnitude.
   /// </summary>
   /// <param name="duration">The duration of the shake effect in seconds.</param>
   /// <param name="magnitude">The magnitude of the shake effect as a vector.</param>
   /// <param name="shakeFrequency">The number of times the camera changes position per second</param>
   /// <returns>An IEnumerator for use with StartCoroutine.</returns>
   public IEnumerator CameraShake(float duration, Vector2 magnitude, float shakeFrequency)
   {
       Vector3 originalPosition = transform.localPosition;
       float elapsed = 0f;
       float shakeInterval = 1f / shakeFrequency;
       float nextShakeTime = 0f;

       while (elapsed < duration)
       {
           if (elapsed >= nextShakeTime)
           {
               float x = Random.Range(-1f, 1f) * magnitude.x;
               float y = Random.Range(-1f, 1f) * magnitude.y;

               transform.localPosition = new Vector3(x, y, originalPosition.z);
               nextShakeTime += shakeInterval;
           }
           elapsed += Time.deltaTime;
           yield return null;
       }

       float returnDuration = 0.2f;
       elapsed = 0.0f;
       
       while (elapsed < returnDuration)
       {
           transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, elapsed / returnDuration);
           elapsed += Time.deltaTime;
           yield return null;
       }
       
       transform.localPosition = originalPosition;
   }
    
#endregion
}