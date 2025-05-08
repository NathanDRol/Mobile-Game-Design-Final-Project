using UnityEngine;
using UnityEngine.PlayerLoop;

public class TouchRaycaster : MonoBehaviour
{
    [SerializeField] private InputHandler _input;
    [SerializeField] private GameObject _touchVisual;

    private void Awake()
    {
        _touchVisual.SetActive(false);
    }

    private void OnEnable()
    {
        _input.TouchStarted += OnTouchStarted;
        _input.TouchEnded += OnTouchEnded;
    }

    private void OnDisable()
    {
        _input.TouchStarted -= OnTouchStarted;
        _input.TouchEnded -= OnTouchEnded;
    }

    private void Update()
    {
        if (_input.TouchHeld)
        {
            RepositionVisual(_input.TouchCurrentPosition);
        }
    }

    private void OnTouchStarted(Vector2 position)
    {
        DetectWorldCollider(position);
        RepositionVisual(_input.TouchCurrentPosition);
    }

    private void DetectWorldCollider(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if(Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Touchable touchable = hitInfo.collider.gameObject.GetComponent<Touchable>();
            if(touchable != null)
            {
                touchable.Touch();
            }
        }
    }

    private void RepositionVisual(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if(Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Debug.Log("Touched: " + hitInfo.transform.gameObject.name);
            _touchVisual.transform.position = hitInfo.point;
            _touchVisual.SetActive(true);
        }
    }

    private void OnTouchEnded(Vector2 position)
    {
        _touchVisual.SetActive(false);
    }

    private void OnTapped(Vector2 position)
    {
        DetectWorldCollider(position);
    }
}
