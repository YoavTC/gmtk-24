using UnityEngine;

public class Arms : MonoBehaviour
{
    [SerializeField] private int mouseButton;
    [SerializeField] private float speed;
    private Camera mainCamera;
    private Rigidbody2D rb;
    private Vector2 mousePos;
    
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 difference = mousePos - (Vector2) transform.position;
        float rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg;

        if (Input.GetMouseButton(mouseButton))
        {
            //rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ, speed * Time.deltaTime));
            rb.MoveRotation(rotationZ);
        }
    }
}
