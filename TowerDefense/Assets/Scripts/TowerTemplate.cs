using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{

    public GameObject towerPrefab;  //���� ������ ���� ������
    public Weapon[] weapon;         //������ Ÿ�� ���� ����
    
    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite; //�������� Ÿ�� �̹���(UI)
        public float damage; //���ݷ�
        public float rate;   //���� �ӵ�
        public float range;   //���� ����
        public int cost;      //�ʿ� ��� (0���� : �Ǽ�, 1~���� : ���׷��̵�)
    }
}
