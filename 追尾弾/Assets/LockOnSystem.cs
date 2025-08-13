using UnityEngine;
using System.Collections.Generic;

public class LockOnSystem : MonoBehaviour
{
    public float lockOnRange = 30f;
    public int maxTargets = 8;
    private List<Transform> targets = new List<Transform>();

    public void StartLockOn()
    {
        targets.Clear();

        // �͈͓��̓G�������X�g�Ɋi�[
        List<Transform> candidates = new List<Transform>();
        Collider[] hits = Physics.OverlapSphere(transform.position, lockOnRange);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector3 dir = hit.transform.position - transform.position;
                if (Vector3.Dot(transform.forward, dir.normalized) > 0.5f) // �O���̂�
                {
                    candidates.Add(hit.transform);
                }
            }
        }

        // ��␔��maxTargets�ȉ��Ȃ�S�����b�N�I��
        if (candidates.Count <= maxTargets)
        {
            targets.AddRange(candidates);
        }
        else
        {
            // ��₪�����ꍇ�̓����_����maxTargets�I��
            for (int i = 0; i < maxTargets; i++)
            {
                int index = Random.Range(0, candidates.Count);
                targets.Add(candidates[index]);
                candidates.RemoveAt(index);
            }
        }

        foreach (var t in targets)
        {
            t.GetComponent<Enemy>().OnLockOn(true);
        }
    }

    public void UpdateLockOn()
    {
        // �K�v�Ȃ炱���Ƀ^�[�Q�b�g�؂�ւ�����������
    }

    public void ReleaseLockOn()
    {
        foreach (var t in targets)
        {
            t.GetComponent<Enemy>().OnLockOn(false);
        }
        targets.Clear();
    }

    public List<Transform> GetTargets()
    {
        return targets;
    }
}
