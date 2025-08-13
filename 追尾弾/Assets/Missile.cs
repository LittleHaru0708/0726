using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float speed = 25f;           // �ړ����x
    public float turnSpeed = 180f;      // ���񑬓x�i�x/�b�j
    public float hitDistance = 1.0f;    // �����蔻�苗��
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

        // �^�[�Q�b�g����
        Vector3 dirToTarget = (target.position - transform.position).normalized;

        // ���݂̌�������^�[�Q�b�g�����֐���
        Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        // �ړ�
        Vector3 moveVector = transform.forward * speed * Time.deltaTime;
        transform.position += moveVector;

        // �����蔻��i�ړ��o�H��̋�������Ō������h�~�j
        if (IsHit(target.position, previousPosition, transform.position, hitDistance))
        {
            // �G�̃��A�N�V�����Ăяo��
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

    // �������P1��P2�ɓ_C��hitRange���ɂ��邩����
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
