using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityManager : MonoBehaviour
{
    public static SanityManager instance { get; private set; }

    public ConnectionStatus _connectionStatus { get; private set; }
    public float heartRate { get => spoofingEnabled ? _spoofHeartRate : _realHeartRate; }

    //timeout
    public float signalTimeout = 5f;
    private float _lastPacketTime;

    //recived heart rate
    private float _realHeartRate = 0;

    //spoof
    private SpoofMode _spoofMode = SpoofMode.Normal;
    private bool spoofingEnabled = false;
    private float _spoofHeartRate = 90;
    private float _spoofBaseRate = 90;
    private List<SpoofEvent> _spoofEvents = new List<SpoofEvent>();
    Coroutine _spoofRoutine;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _connectionStatus = ConnectionStatus.Disconnected;
    }

    void Update()
    {
        if (!spoofingEnabled)
        {
            if (_connectionStatus == ConnectionStatus.Connected &&
                Time.time - _lastPacketTime > signalTimeout)
            {
                _connectionStatus = ConnectionStatus.Disconnected;
                Debug.Log("DEVICE DISCONNECTED -> THROW AND STOP IMP.");
            }
        }
    }

    /*--- HEART RATE ---*/

    public void PushHeartRate(float bpm)
    {
        _realHeartRate = bpm;
        _lastPacketTime = Time.time;
        if (!spoofingEnabled && _connectionStatus != ConnectionStatus.Connected)
        {
            _connectionStatus = ConnectionStatus.Connected;
        }
    }

    /*--- END OF HEART RATE*/


    /*--- START OF SPOOFING (DEV ONLY) ---*/

    public void EnableSpoof(SpoofMode? mode)
    {
        spoofingEnabled = true;
        _spoofMode = mode ?? SpoofMode.Normal;
        _spoofEvents.Clear();

        switch (_spoofMode)
        {
            case SpoofMode.Normal: _spoofBaseRate = 95; break;
            case SpoofMode.Elevated: _spoofBaseRate = 105; break;
            case SpoofMode.Degradeded: _spoofBaseRate = 75; break;
        }

        if (_spoofRoutine != null) StopCoroutine(_spoofRoutine);

        _spoofRoutine = StartCoroutine(SpoofRoutine());

        Debug.Log("ENABLED SPOOFING");
    }

    public void ChangeSpoofMode(SpoofMode newMode)
    {
        _spoofMode = newMode;
        switch (_spoofMode)
        {
            case SpoofMode.Normal:
                _spoofBaseRate = 95;
                break;
            case SpoofMode.Elevated:
                _spoofBaseRate = 105;
                break;
            case SpoofMode.Degradeded:
                _spoofBaseRate = 75;
                break;
        }
    }

    public void SetSpoofingEvenet(SpoofEventType eventType, int seconds, int? value)
    {
        var newEvent = new SpoofEvent()
        {
            Type = eventType,
            Value = value,
            Time = (Time.time + seconds),
        };

        _spoofEvents.Add(newEvent);
    }

    public void DisableSpoof()
    {
        spoofingEnabled = false;
        _spoofEvents.Clear();

        if (_spoofRoutine != null)
        {
            StopCoroutine(_spoofRoutine);
            _spoofRoutine = null;
        }
    }

    private IEnumerator SpoofRoutine()
    {
        while (spoofingEnabled)
        {
            for (int i = _spoofEvents.Count - 1; i >= 0; i--)
            {
                var e = _spoofEvents[i];
                if (Time.time >= e.Time)
                {
                    if (e.Value.HasValue)
                    {
                        _spoofHeartRate = e.Value.Value;
                    }
                    else
                    {
                        _spoofHeartRate += (e.Type == SpoofEventType.Increase ? +10 : -10);
                    }

                    Debug.Log($"SPOOF EVENT APPLIED");
                    _spoofEvents.RemoveAt(i);
                }
            }

            if (_spoofEvents.Count == 0)
            {
                _spoofHeartRate = _spoofBaseRate + Random.Range(-8, +8);
            }

            _connectionStatus = ConnectionStatus.Connected;
            yield return new WaitForSeconds(1f);
        }
    }

    /*--- END OF SPOOFING ---*/
}
