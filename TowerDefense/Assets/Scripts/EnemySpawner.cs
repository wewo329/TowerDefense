using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update

    //[SerializeField]
    //private GameObject enemyPrefab;  // �� ������
    [SerializeField]
    private GameObject enemyHPSliderPrefab; //�� ü���� ��Ÿ���� sILDER ui ������
    [SerializeField]
    private Transform canvasTransform;     //UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform

    //[SerializeField]
    //private float spawnTime;         // �� ���� �ֱ�
    [SerializeField]
    private Transform[] wayPoints;   // ���� ���������� �̵� ���
    [SerializeField]
    private PlayerHP playerHP;   // �÷��̾��� ü�� ������Ʈ
    [SerializeField]
    private PlayerGold playerGold;    //�÷��̾��� ��� ������Ʈ
    private Wave currentWave;
    private int currentEnemyCount;
    private List<Enemy> enemyList;   //���� �ʿ� �����ϴ� ��� ���� ����

    //���� ������ ������ EnemySpawner���� �ϱ� ������ Set�� �ʿ� ����.
    public List<Enemy> EnemyList => enemyList;
    //���� ���̺��� �����ִ� ��, �ִ� �� ����
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;
    private void Awake()
    {
        //�� ����Ʈ �޸� �Ҵ�
        enemyList = new List<Enemy>();

        //�� ���� �ڷ�ƾ �Լ� ȣ��
        //StartCoroutine("SpawnEnemy");
    }

    public void StartWave(Wave wave)
    {
        //�Ű������� �޾ƿ� ���̺� ���� ����
        currentWave = wave;
        //���� ���̺��� �ִ� �� ���ڸ� ����
        currentEnemyCount = currentWave.maxEnemyCount;
        //���� ���̺� ����
       StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        //���� ���̺꿡�� �����Ǿ�� �ϴ� ���� ���ڸ�ŭ ���� �����ϰ� �ڷ�ƾ ����
        int spawnEnemyCount = 0;
        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {
            // GameObject clone = Instantiate(enemyPrefab);    // �� ������Ʈ ����
            // Enemy enemy = clone.GetComponent<Enemy>();      // ��� ������ ���� Enemy ������Ʈ
            
            //���̺꿡 �����ϴ� ���� ������ ���� ������ �� ������ ���� �����ϵ��� �����ϰ�, �� ������Ʈ ����
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>(); //��� ������ ���� Enemy ������Ʈ
            // this�� �� �ڽ� (�ڽ��� EnemySpawner ����)
            enemy.Setup(this, wayPoints);                         // wayPoint ������ �Ű������� Setup() ȣ��

            enemyList.Add(enemy);                           //����Ʈ�� ��� ������ �� ���� ����

            SpawnEnemyHPSlider(clone);  //�� ü���� ��Ÿ���� Silder UI ���� �� ����

            //���� ���̺꿡�� ������ ���� ���� +1
            spawnEnemyCount++;
            //yield return new WaitForSeconds(spawnTime);     // spawnTime �ð� ���� ���
            //�� ���̺긶�� spawnTIme�� �ٸ� �� �ֱ� ������ ���� ���̺�(currentWave)�� spawnTime ���
            yield return new WaitForSeconds(currentWave.spawnTime); //spawnTime �ð� ���� ���

        }
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {

        //���� ��ǥ�������� ����������
        if(type == EnemyDestroyType.Arrive)
        {
            //�÷��̾��� ü�� -1
            playerHP.TakeDamage(1);
        }
        //���� �÷��̾��� �߻�ü���� ������� ��
        else if(type == EnemyDestroyType.kill)
        {
            //���� ������ ���� ��� �� ��� ȹ��
            playerGold.CurrentGold += gold;
        }
        //���� ����� ������ ���� ���̺��� ���� �� ���� ����(UI ǥ�ÿ�)

        currentEnemyCount --;

        //����Ʈ���� ����ϴ� �� ���� ����
        enemyList.Remove(enemy);
        //�� ������Ʈ ����
        Destroy(enemy.gameObject);
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        //�� ü���� ��Ÿ���� Slider UI ����
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        //Slider UI ������Ʈ�� parent("Canvas" ������Ʈ)dml wktlrdmfh tjfwjd
        //Tip. UI�� ĵ������ �ڽĿ�����Ʈ�� �����Ǿ� �־��� ȭ�鿡 ���δ�.
        sliderClone.transform.SetParent(canvasTransform);
        //���� �������� �ٲ� ũ�⸦ �ٽ�(1, 1, 1)�� ����
        sliderClone.transform.localScale = Vector3.one;

        //Slider UI�� �i�Ƶ�� ����� �������� ����
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        //Slider UI�� �ڽ��� ü�� ������ ǥ���ϵ��� ����
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }

}
