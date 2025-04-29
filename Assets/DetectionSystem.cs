using UnityEngine;

public class DetectionSystem : MonoBehaviour
{
    public float sightRange = 20f;
    public float hearingRange = 15f;
    public LayerMask playerLayer;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < sightRange)
        {
            return true;
        }
        return false;
    }

    public bool CanHearPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < hearingRange)
        {
            return true;
        }
        return false;
    }

    public Vector3 GetPlayerPosition()
    {
        return player.position;
    }

    public Vector3 GetSoundPosition()
    {
        return player.position;
    }
}
