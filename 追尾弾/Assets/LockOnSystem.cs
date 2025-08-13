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

        // 範囲内の敵候補をリストに格納
        List<Transform> candidates = new List<Transform>();
        Collider[] hits = Physics.OverlapSphere(transform.position, lockOnRange);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector3 dir = hit.transform.position - transform.position;
                if (Vector3.Dot(transform.forward, dir.normalized) > 0.5f) // 前方のみ
                {
                    candidates.Add(hit.transform);
                }
            }
        }

        // 候補数がmaxTargets以下なら全部ロックオン
        if (candidates.Count <= maxTargets)
        {
            targets.AddRange(candidates);
        }
        else
        {
            // 候補が多い場合はランダムにmaxTargets個選ぶ
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
        // 必要ならここにターゲット切り替え処理を実装
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
