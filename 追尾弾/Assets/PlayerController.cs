using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 100f;
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;
    public LockOnSystem lockOnSystem;

    void Update()
    {
        HandleRotation();
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            lockOnSystem.StartLockOn();
        }
        if (Input.GetKey(KeyCode.Z))
        {
            lockOnSystem.UpdateLockOn();
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            FireMissiles();
            lockOnSystem.ReleaseLockOn();
        }
    }

    void HandleRotation()
    {
        float pitch = -Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        float yaw = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(pitch, yaw, 0, Space.Self);
    }

    void HandleMovement()
    {
        float move = 0f;
        if (Input.GetKey(KeyCode.W)) move = 1f;
        else if (Input.GetKey(KeyCode.S)) move = -1f;

        transform.position += transform.forward * move * moveSpeed * Time.deltaTime;
    }

    void FireMissiles()
    {
        List<Transform> targets = lockOnSystem.GetTargets();
        if (targets.Count == 0) return;

        int missileCount = 8;
        float maxAngle = 60f; // 左右に最大±20度ずらす

        for (int i = 0; i < missileCount; i++)
        {
            // 放射状に角度を決定（中心はプレイヤー前方）
            float angleStep = (maxAngle * 2) / (missileCount - 1);
            float angle = -maxAngle + angleStep * i;

            // プレイヤーのforwardに角度を加える
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            Quaternion rotation = Quaternion.LookRotation(direction);
            GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, rotation);


            // ミサイル生成（位置はミサイル生成ポイント）

            HomingMissile hm = missile.GetComponent<HomingMissile>();

            // ロックオン中のターゲットがあればターゲットを割り当て（番号が多い場合はループで割り当ててもよい）
            if (i < targets.Count)
                hm.SetTarget(targets[i]);
            else
                hm.SetTarget(null); // ターゲットなしでも飛ばす場合（そのまま直進）

            // ※ 旋回最大角度でターゲットへ旋回しながら飛ぶ仕様は HomingMissile に任せる
        }
    }

}
