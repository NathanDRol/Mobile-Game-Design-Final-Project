using UnityEngine;
using UnityEngine.Events;

public class Touchable : MonoBehaviour
{
    public UnityEvent Touched;
    public void Touch()
    {
        Touched?.Invoke();
    }
}
