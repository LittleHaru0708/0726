using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;
    public Renderer rend;
    private Color defaultColor;
    private bool isLockedOn = false;
    private float startY;

    void Start()
    {
        if (!rend) rend = GetComponent<Renderer>();
        defaultColor = rend.material.color;
        startY = transform.position.y;
    }

    void Update()
    {
        // Ç”ÇÌÇ”ÇÌìÆçÏ
        float newY = startY + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void OnLockOn(bool state)
    {
        isLockedOn = state;
        rend.material.color = state ? Color.red : defaultColor;
    }

    public void OnHit()
    {
        StartCoroutine(HitReaction());
    }

    private IEnumerator HitReaction()
    {
        rend.material.color = Color.yellow;
        Vector3 originalPos = transform.position;

        float shakeDuration = 0.5f;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            transform.position = originalPos + Random.insideUnitSphere * 0.1f;
            elapsed += Time.deltaTime;
            yield return null;
        }

        rend.material.color = isLockedOn ? Color.red : defaultColor;
        transform.position = originalPos;
    }
}
