using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private int damage;

    public void Setup(Transform target, int damage)
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target; //Ÿ���� �������� target
        this.damage = damage; //Ÿ���� �������� ���ݷ�
    }

    private void Update()
    {
        if(target != null)//target�� �����ϸ�
        {
            //�߻�ü��  target�� ��ġ�� �̵�
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else    //���� ������ target�� �������
        {
            //�߻�ü ������Ʈ ����
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return; //���� �ƴ� ���� �ε�����
        if (collision.transform != target) return;  //���� target�� ���� �ƴҶ�

        // collision.GetComponent<Enemy>().OnDie();    //�� ��� �Լ� ȣ��
        collision.GetComponent<EnemyHP>().TakeDamage(damage);
        Destroy(gameObject);                        //�߻�ü ������Ʈ ����


    }
}
