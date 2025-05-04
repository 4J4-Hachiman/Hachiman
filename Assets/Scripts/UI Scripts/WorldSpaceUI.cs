using UnityEngine;

public class WorldSpaceUI : MonoBehaviour
{
    [field: SerializeField] GameObject cam;
    [field: SerializeField] private RectTransform rect;
    public GameObject uiImage;

    void Start()
    {
        cam = Camera.main.gameObject;
    }

    void FixedUpdate()
    {
        
    }

    void OnTriggerEnter()
    {
        uiImage.SetActive(true);
    }

    void OnTriggerStay(Collider collision)
    {
        transform.rotation = Quaternion.LookRotation(cam.transform.forward);
    }

    void OnTriggerExit()
    {
        uiImage.SetActive(false);
    }
}
