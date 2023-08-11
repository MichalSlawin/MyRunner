using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pressAnyKeyText;
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;

    public void SwitchPressAnyKeyText(bool visible)
    {
        _pressAnyKeyText.gameObject.SetActive(visible);
    }

    public void SetCurrentScoreText(float value)
    {
        _currentScoreText.text = value.ToString();
    }

    public void SetBestScoreText(float value)
    {
        _bestScoreText.text = value.ToString();
    }
}
