using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeartRateUI : MonoBehaviour
{
    public Text heartRateText;
    public RectTransform heartImage;

    private float _heartRate = 0;

    public float ampAtMin = 0.1f;
    public float ampAtMax = 0.3f;
    public float bpmMin = 60f;
    public float bpmMax = 180f;
    public float beatDuration = 0.15f;
    private Coroutine _beatLoop;

    void Start()
    {
        _beatLoop = StartCoroutine(BeatLoop());
    }

    void OnDisable()
    {
        if (_beatLoop != null) StopCoroutine(_beatLoop);
    }

    void Update()
    {
        _heartRate = SanityManager.instance.heartRate;
        heartRateText.text = _heartRate.ToString() + " bpm";
        TextColorChanger();
    }

    IEnumerator BeatLoop()
    {
        while (true)
        {
            float bpm = SanityManager.instance.heartRate;
            bpm = Mathf.Max(1f, bpm);

            float interval = 60f / bpm;

            yield return StartCoroutine(BeatOnce(bpm));

            yield return new WaitForSeconds(interval - beatDuration);
        }
    }

    IEnumerator BeatOnce(float bpm)
    {
        float t = Mathf.InverseLerp(bpmMin, bpmMax, bpm);
        float amp = Mathf.Lerp(ampAtMin, ampAtMax, t);

        float half = beatDuration * 0.5f;
        Vector3 original = Vector3.one;
        Vector3 target = Vector3.one * (1f + amp);

        float timer = 0f;
        while (timer < half)
        {
            heartImage.localScale = Vector3.Lerp(original, target, timer / half);
            timer += Time.deltaTime;
            yield return null;
        }

        heartImage.localScale = target;

        timer = 0f;
        while (timer < half)
        {
            heartImage.localScale = Vector3.Lerp(target, original, timer / half);
            timer += Time.deltaTime;
            yield return null;
        }

        heartImage.localScale = original;
    }

    void TextColorChanger()
    {
        if(_heartRate < 90)
        {
            heartRateText.color = Color.green;
        }

        if(_heartRate > 90 && _heartRate < 110)
        {
            heartRateText.color = new Color(1f, 0.64f, 0f);
        }

        if(_heartRate > 110)
        {
            heartRateText.color = Color.red;
        }
    }
}
