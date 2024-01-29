using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Image _clearImage = default;
    [SerializeField]
    private Image _retryImage = default;
    private MapController _mapController = default;

    private void Awake()
    {
        _mapController = FindObjectOfType<MapController>();
        _mapController.OnClear.Subscribe(_ => GameClear());

        _clearImage.color = new(_clearImage.color.r, _clearImage.color.g, _clearImage.color.b, 0f);
        _retryImage.color = new(_retryImage.color.r, _retryImage.color.g, _retryImage.color.b, 0f);
    }

    private void GameStart()
    {

    }

    private void GameClear()
    {
        _clearImage.color = new(_clearImage.color.r, _clearImage.color.g, _clearImage.color.b, 1f);
    }
}
