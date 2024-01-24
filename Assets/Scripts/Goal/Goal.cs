using UnityEngine;

public class Goal : MonoBehaviour
{
    private MapController _mapController = default;
    private bool _isGoal = false;


    private void Awake()
    {
        _mapController = FindObjectOfType<MapController>();
    }

    public void OnGoal()
    {
        _isGoal = true;
    }
}
