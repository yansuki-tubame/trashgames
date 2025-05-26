using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 1.0f;

    public bool isLocked = false;
    public float lockingDuration = 1.0f;
    public Vector2 lockPos = new Vector2(1.0f, 1.0f);

    public float scaling = 1.5f;

    private float k => Mathf.PI / (2 * lockingDuration);

    private void Awake()
    {
        TriggerNotifier.OnEnterLockArea += LockCamera;
    }

    private void FixedUpdate()
    {
        if (!isLocked) {
            transform.position = Vector3.Lerp(transform.position, target.position, smoothSpeed);
        }
    }

    private void LockCamera()
    {
        isLocked = true;
        StartCoroutine(StartLocking());
    }

    private IEnumerator StartLocking()
    {
        float time = 0f;
        Vector2 direction = (lockPos - (Vector2)transform.position).normalized;
        float originalSize = Camera.main.orthographicSize;
        float currentSize;
        float currentDistance;

        float deltaX = ((Vector2)transform.position - lockPos).magnitude;

        while (time < lockingDuration) {
            time += Time.deltaTime;

            currentSize = scaling * originalSize * Mathf.Sin(k * time);
            currentDistance = deltaX * (1 - Mathf.Sin(k * time));

            Camera.main.orthographicSize = currentSize;
            transform.position = (Vector3)(direction * currentDistance + lockPos);

            yield return null;
        }
    }
}
