using UnityEngine;

namespace Buildables
{
    public class PlaceableObject : MonoBehaviour
    {
        public float price;
        public bool isPlaced;
        public bool isCanceled;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (!isPlaced && Input.GetMouseButtonDown(0))
            {
                isPlaced = true;
            }
            if (!isPlaced && Input.GetKey(KeyCode.Escape))
            {
                isCanceled = true;
            }
        }
    
    }
}
