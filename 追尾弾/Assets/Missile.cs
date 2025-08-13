using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float speed = 25f;           // 移動速度
    public float turnSpeed = 180f;      // 旋回速度（度/秒）
    public float hitDistance = 1.0f;    // 当たり判定距離
    private Transform target;

    private Vector3 previousPosition;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        previousPosition = transform.position;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // ターゲット方向
        Vector3 dirToTarget = (target.position - transform.position).normalized;

        // 現在の向きからターゲット方向へ旋回
        Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        // 移動
        Vector3 moveVector = transform.forward * speed * Time.deltaTime;
        transform.position += moveVector;

        // 当たり判定（移動経路上の距離判定で見逃し防止）
        if (IsHit(target.position, previousPosition, transform.position, hitDistance))
        {
            // 敵のリアクション呼び出し
            var enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.OnHit();
            }

            Destroy(gameObject);
            return;
        }

        previousPosition = transform.position;
    }

    // 直線区間P1→P2に点CがhitRange内にあるか判定
    bool IsHit(Vector3 targetPos, Vector3 startPos, Vector3 endPos, float hitRange)
    {
        Vector3 line = endPos - startPos;
        Vector3 toTarget = targetPos - startPos;

        float lineLength = line.magnitude;
        if (lineLength < 0.0001f) return false;

        float t = Vector3.Dot(toTarget, line.normalized);
        t = Mathf.Clamp(t, 0f, lineLength);

        Vector3 closestPoint = startPos + line.normalized * t;
        float distToTarget = Vector3.Distance(closestPoint, targetPos);

        return distToTarget <= hitRange;
    }
}
