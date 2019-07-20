namespace UnityEngine.EventSystems
{
    [RequireComponent(typeof(EventSystem))]
    public class CustomDragThreshold : MonoBehaviour
    {
        private const float inchToCm = 2.54f;

        EventSystem eventSystem = null;

        [SerializeField]
        private float dragThresholdCM = 0.5f;

        void Awake()
        {
            eventSystem = GetComponent<EventSystem>();
            SetDragThreshold();
        }

        void SetDragThreshold()
        {
            eventSystem.pixelDragThreshold = (int)(dragThresholdCM * Screen.dpi / inchToCm);
        }
    }
}