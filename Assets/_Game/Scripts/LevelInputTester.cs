using UnityEngine;

public class LevelInputTester : MonoBehaviour
{
    [SerializeField] private InputHandler _input;

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

    private void OnTouchStarted(Vector2 position)
    {
        Debug.Log("Level: Started: " + position);
    }

    private void OnTouchEnded(Vector2 position)
    {
        Debug.Log("Level: Ended: " + position);
    }
}
