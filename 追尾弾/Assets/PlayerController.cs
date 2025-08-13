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
        float maxAngle = 60f; // ���E�ɍő�}20�x���炷

        for (int i = 0; i < missileCount; i++)
        {
            // ���ˏ�Ɋp�x������i���S�̓v���C���[�O���j
            float angleStep = (maxAngle * 2) / (missileCount - 1);
            float angle = -maxAngle + angleStep * i;

            // �v���C���[��forward�Ɋp�x��������
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            Quaternion rotation = Quaternion.LookRotation(direction);
            GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, rotation);


            // �~�T�C�������i�ʒu�̓~�T�C�������|�C���g�j

            HomingMissile hm = missile.GetComponent<HomingMissile>();

            // ���b�N�I�����̃^�[�Q�b�g������΃^�[�Q�b�g�����蓖�āi�ԍ��������ꍇ�̓��[�v�Ŋ��蓖�ĂĂ��悢�j
            if (i < targets.Count)
                hm.SetTarget(targets[i]);
            else
                hm.SetTarget(null); // �^�[�Q�b�g�Ȃ��ł���΂��ꍇ�i���̂܂ܒ��i�j

            // �� ����ő�p�x�Ń^�[�Q�b�g�֐��񂵂Ȃ����Ԏd�l�� HomingMissile �ɔC����
        }
    }

}
