using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;

public class UIFlashing : MonoBehaviour
{
    private Image _image = default;
    private WaitForSeconds _flashWait = new WaitForSeconds(0.1f);

    private MapController _mapController = default;

    private void Awake()
    {
        _image = this.GetComponent<Image>();

        _mapController = FindObjectOfType<MapController>();

        // InGameであればイベント購読。OutGameであれば即実行
        if (_mapController is null)
        {
            Flash();
        }
        else
        {
            _mapController.OnClear.Subscribe(_ => Flash());
        }
    }


    public void Flash()
    {
        StartCoroutine(FlashAsync());
    }

    private IEnumerator FlashAsync()
    {
        yield return new WaitForSeconds(1f);
        float colorA = _image.color.a;

        while (true)
        {
            while (colorA >= 0.2f)
            {
                colorA -= 0.1f;
                _image.color = new(_image.color.r, _image.color.g, _image.color.b, colorA);
                yield return _flashWait;
            }

            while (colorA <= 1f)
            {
                colorA += 0.1f;
                _image.color = new(_image.color.r, _image.color.g, _image.color.b, colorA);
                yield return _flashWait;
            }
        }
    }
}
