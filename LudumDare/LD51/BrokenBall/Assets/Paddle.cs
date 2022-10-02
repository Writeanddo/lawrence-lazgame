using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float Speed = 1;
    public Rigidbody2D Body;
    public Vector2 SqueezedScale;
    public float SqueezeLerpBound = 0.3f;

    private float _lastPositionX;

    private void Start()
    {
        Body = GetComponent<Rigidbody2D>();
        _lastPositionX = transform.position.x;
    }

    private void Update()
    {
        var deltaX = transform.position.x - _lastPositionX;
        transform.localScale = Vector2.Lerp(Vector2.one, SqueezedScale, Mathf.Clamp01(Mathf.Abs(deltaX) / SqueezeLerpBound));
        _lastPositionX = transform.position.x;
    }

    private void FixedUpdate()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.x = Mathf.Clamp(Input.mousePosition.x, 50, Screen.width - 50);
        var mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
        Body.MovePosition(new Vector2(
            mousePositionInWorld.x, 
            Body.position.y));
    }
}
