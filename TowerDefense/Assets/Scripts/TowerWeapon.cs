using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState { SearchTarget =0, AttackToTarget }
public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate towerTemplate;                             // Ÿ�� ���� (���ݷ�, ���ݼӵ� ��)
    [SerializeField]
    private GameObject projectilePrefab;                            //�߻�ü ������
    [SerializeField]
    private Transform spawnPoint;                                   //�߻�ü ���� ��ġ
  /*  [SerializeField]
    private float attackRate = 0.5f;                                //���� �ӵ�
    [SerializeField]
    private float attackRange = 2.0f;                                //���ݹ���
    [SerializeField]
    private int attackDamage = 1;                                   //���ݷ�*/
    private int level = 0;                                          //Ÿ�� ����
    private WeaponState weaponState = WeaponState.SearchTarget;     //Ÿ�� ������ ����
    private Transform attackTarget = null;                          //���� ���
    private SpriteRenderer spriteRenderer;                           //Ÿ�� ������Ʈ �̹��� �����
    private EnemySpawner enemySpawner;                              //���ӿ�  �����ϴ� �� ���� ȹ���
    private PlayerGold playerGold;                                   //�÷��̾��� ��� ���� ȹ�� �� ����

    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;
    public float Damage => towerTemplate.weapon[level].damage;
    //public float Damage => towerTemplate
    public float Rate => towerTemplate.weapon[level].rate;
    //public float Rate => attackRate;
    public float Range => towerTemplate.weapon[level].range;
    //public float Range => attackRange;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;



    public void Setup(EnemySpawner enemySpawner, PlayerGold playerGold)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.enemySpawner = enemySpawner;
        this.playerGold = playerGold;
        //���� ���¸� WeaponState.SearchTarget���� ����
        ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        //������ ������̴� ���� ����
        StopCoroutine(weaponState.ToString());
        //���� ����
        weaponState = newState;
        //���ο� ���� ���
        StartCoroutine(weaponState.ToString());
    }

    private void Update()
    {
        if(attackTarget != null)
        {
            RotateToTarget();
        }
    }

    private void RotateToTarget()
    {
        //�������κ����� �Ÿ��� ���������κ����� ������ �̿��� ��ġ�� ���ϴ� �� ��ǥ�� �̿�
        // ���� = arctan(y/x)
        // x,y ������ ���ϱ�
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;

        //x,y �������� �������� ���� ���ϱ�
        //������ radian �����̱� ������ Mathf.Rad2Deg�� ���� �� ������ ����
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    private IEnumerator SearchTarget()
    {
        while(true)
        {
            //���� ������ �ִ� ���� ã�� ���� ���� �Ÿ��� �ִ��� ũ�� ����
            float closestDistSqr = Mathf.Infinity;
            //EmemySpawner�� EnemyList�� �̾� ���� �ʿ� �����ϴ� ��� �� �˻�
            for (int i = 0; i < enemySpawner.EnemyList.Count; ++i)
            {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
                //���� �˻����� ������ �Ÿ��� ���ݹ��� ���� �ְ�, ������� �˻��� ������ �Ÿ��� ������
                //if(distance <= attackRange && distance <= closestDistSqr)
                if(distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
                   {
                    closestDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i].transform;
                }
            }
            
            if(attackTarget !=null)
            {
                ChangeState(WeaponState.AttackToTarget);
            }

            yield return null;
        }
    }
    private IEnumerator AttackToTarget()
    {
        while (true)
        {
            //1.target�� �ִ��� �˻� (�ٸ� �߻�ü�� ���� ����, Goal �������� �̵��� ������)
            if (attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 2. target�� ���� ���� �ȿ� �ִ��� �˻�(���ݹ����� ����� ���ο� �� Ž��)
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            //if (distance > attackRange)
            if(distance > towerTemplate.weapon[level].range)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //3. attackRate �ð���ŭ ���
            //yield return new WaitForSeconds(attackRate);
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);
            //4. ���� (�߻�ü ����)
            SpawnProjectile();
        }
    }

    //��������Ʈ ��ġ���� �߻�ü�� ����
    private void SpawnProjectile()
    {
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        //������ �߻�ü���� ���ݴ��(attackTarget)��������
        //clone.GetComponent<Projectile>().Setup(attackTarget, attackDamage);
        clone.GetComponent<Projectile>().Setup(attackTarget, towerTemplate.weapon[level].damage);
    }

    public bool Upgrade()
    {
        //Ÿ�� ���׷��̵忡 �ʿ��� ��尡 ������� �˻�
        if(playerGold.CurrentGold < towerTemplate.weapon[level+1].cost)
        {
            return false;
        }

        //Ÿ�� ���� ����
        level++;
        //Ÿ�� ���� ���� (Sprite)
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        //��� ����
        playerGold.CurrentGold -= towerTemplate.weapon[level].cost;

        return true;
    }
}
