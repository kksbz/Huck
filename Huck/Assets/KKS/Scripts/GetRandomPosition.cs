using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GetRandomPosition
{
    //! ������ �ȿ��� ������ ��ǥ �������� �Լ�
    public Vector3 GetRandomCirclePos(Vector3 center, int radius, int minDistance)
    {
        // 360�� �������� ���� ���� ���ϱ�
        float angle = Random.Range(0f, Mathf.PI * 2);
        // �Ÿ� 2 ~ ���������̿��� ���� �Ÿ� ���ϱ�
        float distance = Random.Range(minDistance, radius + 1);
        // �ﰢ�Լ� �̿��ؼ� x, z ��ǥ�� ���� �Ÿ� ���ϱ� ���� ������
        float x = center.x + distance * Mathf.Cos(angle);
        float z = center.z + distance * Mathf.Sin(angle);
        RaycastHit hit = default;
        Vector3 pos = center + Vector3.up * 5f;
        // �������� ���� ��ǥ ������ �Ʒ��� �����ɽ�Ʈ���� ������ üũ
        if (Physics.Raycast(pos, Vector3.down, out hit, 15f, LayerMask.GetMask(GData.TERRAIN_MASK)) == true)
        {
            // �浹�� ���� ���̸� ��ǥ�� ���� (��ǥ�� ����� ����Ʈ�� ��ġ�� 0.1f ���� ó��)
            center.y = hit.point.y + 0.1f;
        }
        else
        {
            // ���� ������ǥ�� ��ֹ��� ������ ��ǥ �ٽ� ���� (����Լ�)
            GetRandomCirclePos(center, radius, minDistance);
        }
        return new Vector3(x, center.y, z);
    } // GetRandomPos

    //! Ÿ���� y��
    public Vector3 GetTerrainOfValidPos(Vector3 _targetPos)
    {
        Vector3 targetPos = _targetPos + Vector3.up * 5f;
        RaycastHit[] hits = Physics.RaycastAll(targetPos, Vector3.down, 10f, LayerMask.GetMask(GData.TERRAIN_MASK));
        targetPos = hits[0].point + new Vector3(0f, 0.1f, 0f);
        return targetPos;
    } // GetTerrainOfValidPos
}
