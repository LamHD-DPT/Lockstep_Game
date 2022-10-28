using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Encapsulates various UI Fill-bars and their fill amount.
/// </summary>
internal class FillBar : MonoBehaviour
{
    private Coroutine _interpolate;

    [SerializeField] private Image[] fills;

    [SerializeField, Space(5)]
    private float smoothTime = .25f;


    public void IncreaseFill(int index)
    {
        // Overwrites the existing coroutine instead of waiting for it to finish.
        if (_interpolate != null)
            StopCoroutine(_interpolate);

        _interpolate = StartCoroutine(Interpolate(index));
    }

    public void DecreaseFill(int index)
    {
        StartCoroutine(Interpolate(index, 0));
    }

    /// <summary>
    /// Smoothly interpolates the fill amount value.
    /// See: <see cref="IncreaseFill"/>.
    /// </summary>
    private IEnumerator Interpolate(int index, float amount = 1f)
    {
        if (fills[index].fillAmount.Equals(amount))
            yield break;

        var preChangeAmount = fills[index].fillAmount;

        float elapsed = 0;

        while (elapsed < smoothTime)
        {
            elapsed += Time.deltaTime;

            fills[index].fillAmount = Mathf.Lerp(preChangeAmount, amount, elapsed / smoothTime);

            yield return 0;
        }

        fills[index].fillAmount = amount;
    }
}