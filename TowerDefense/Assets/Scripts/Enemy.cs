using System.Collections;
using UnityEngine;

public enum EnemyDestroyType { kill =0,Arrive}
public class Enemy : MonoBehaviour
{

    private int wayPointCount;      //�̵� ��� ����
    private Transform[] wayPoints;  //�̵� ��� ����
    private int currentIndex = 0;   //���� ��ǥ���� �ε���
    private Movement2D movement2D;  //������Ʈ �̵� ����
    private EnemySpawner enemySpawner; //���� ������ ������ ���� �ʰ� EnemySpawner�� �˷��� ����
    [SerializeField]
    private int gold = 10;           //�� ��� �� ȹ�� ������ ���

    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        //�� �̵� ��� WayPoints ���� ����
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        //���� ��ġ�� ù��° wayPoint ��ġ�� ����
        transform.position = wayPoints[currentIndex].position;

        //�� �̵�/��ǥ���� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        //���� �̵� ���� ����
        NextMoveTo();

        while(true)
        {
            //�� ������Ʈ ȸ��
            transform.Rotate(Vector3.forward * 10);
            //���� ���� ��ġ�� ��ǥ ��ġ�� �Ÿ��� 0.02 * movement2D.MoveSpeed���� ���� �� if ���ǹ� ����
            //Tip. movement2D.MoveSpeed�� �����ִ� ������ �ӵ��� ¥���� �� �����ӿ� 0.02���� ũ�� �����̱� ������
            //if ���ǹ� �ɸ��� �ʰ� ��θ� Ż���ϴ� ������Ʈ�� �߻��� �� �ִ�.
            if( Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed)
            {
                //���� �̵� ���� ����
                NextMoveTo();
            }

            yield return null;
        }
    }

    private void NextMoveTo()
    {
        //���� �̵��� wayPoints�� �����ִٸ�
        if(currentIndex < wayPointCount - 1)
        {
            //���� ��ġ�� ��Ȯ�ϰ� ��ǥ ��ġ�� ����
            transform.position = wayPoints[currentIndex].position;
            //�̵� ���� ���� => ���� ��ǥ����(wayPoints)
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        //���� ��ġ�� ������ wayPoints�̸�
        else
        {
            //��ǥ������ �����ؼ� ����� ���� ���� ���� �ʵ���
            gold = 0;
            //�� ������Ʈ ����
            //Destroy(gameObject);
            OnDie(EnemyDestroyType.Arrive);
        }

    }

    public void OnDie(EnemyDestroyType type)
    {
        //EnemySpawner���� ����Ʈ�� �� ������ �����ϱ� ������ Destroy()�� �������� �ʰ�
        //EnemySoawner���� ������ ������ �� �ʿ��� ó���� �ϵ��� DestroyEnemy() �Լ� ȣ��
        enemySpawner.DestroyEnemy(type, this, gold);
    }

}
