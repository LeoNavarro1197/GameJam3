using UnityEngine;
using TMPro;
using System.Collections;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    [SerializeField] private TextMeshProUGUI tooltipText;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ShowTemporarily(string message, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ShowCoroutine(message, duration));
    }

    private IEnumerator ShowCoroutine(string message, float duration)
    {
        tooltipText.text = message;
        gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
