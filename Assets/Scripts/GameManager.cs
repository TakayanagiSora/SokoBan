using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Image _clearImage = default;
    [SerializeField]
    private Image _retryImage = default;
    [SerializeField]
    private Text _movesCountText = default;
    [SerializeField]
    private Text _playTimeText = default;

    private MapController _mapController = default;

    private void Awake()
    {
        _mapController = FindObjectOfType<MapController>();
        _mapController.OnClear.Subscribe(_ => GameClear());

        _clearImage.color = new(_clearImage.color.r, _clearImage.color.g, _clearImage.color.b, 0f);
        _retryImage.color = new(_retryImage.color.r, _retryImage.color.g, _retryImage.color.b, 0f);
        _movesCountText.enabled = false;
        _playTimeText.enabled = false;
    }

    private void GameClear()
    {
        _clearImage.color = new(_clearImage.color.r, _clearImage.color.g, _clearImage.color.b, 1f);
        _movesCountText.text += _mapController.NumberOfMoves.ToString();
        _movesCountText.enabled = true;

        int time_m = Mathf.FloorToInt(_mapController.PlayTime / 60);
        int time_s = Mathf.FloorToInt(_mapController.PlayTime - time_m * 60);
        _playTimeText.text += $"{time_m}•ª{time_s}•b";
        _playTimeText.enabled = true;
    }
}
