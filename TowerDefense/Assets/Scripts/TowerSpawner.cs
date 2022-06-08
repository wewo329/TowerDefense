using UnityEngine;

public class TowerSpawner: MonoBehaviour
{
    [SerializeField]
    private TowerTemplate towerTemplate;     //���� ����(���ݷ�, ���ݼӵ� ��)
    /*[SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private int towerBuildGold = 50; //Ÿ�� �Ǽ��� ���Ǵ� ���*/
    [SerializeField]
    private EnemySpawner enemySpawner; //���� �ʿ� �����ϴ� �� ����Ʈ ������ ��� ����..
    [SerializeField]
    private PlayerGold playerGold;// Ÿ�� �Ǽ� �� ��� ���Ҹ� ����..
    [SerializeField]
    private SystemTextViewer systemTextViewer;    //�� ����, �Ǽ� �Ұ��� ���� �ý��� �޼��� ���

    public void SpawnTower(Transform tileTransform)
    {
        
        //Ÿ�� �Ǽ� ���� ���� Ȯ��
        // 1. Ÿ���� �Ǽ��� ��ŭ ���� ������ Ÿ�� �Ǽ� X
        //if(towerBuildGold > playerGold.CurrentGold)
        if(towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        {
            //��尡 �����ؼ� Ÿ�� �Ǽ��� �Ұ����ϴٰ� ���
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }
        Tile tile = tileTransform.GetComponent<Tile>();

        //1. ���� Ÿ���� ��ġ�� �̹� Ÿ���� �Ǽ��Ǿ� ������ Ÿ�� �Ǽ� X
        if (tile.IsBuildTower == true)
        {
            //���� ��ġ�� Ÿ�� �Ǽ��� �Ұ����ϴٰ� ���
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }
        //Ÿ���� �Ǽ��Ǿ� �������� ����
        tile.IsBuildTower = true;
        //Ÿ�� �Ǽ��� �ʿ��� ��常ŭ ����
        //playerGold.CurrentGold -= towerBuildGold;
        playerGold.CurrentGold -= towerTemplate.weapon[0].cost;
        //������ Ÿ���� ��ġ�� Ÿ�� �Ǽ�(Ÿ�Ϻ��� z�� 01�� ��ġ�� ��ġ)
        Vector3 position = tileTransform.position + Vector3.back;
        //GameObject clone = Instantiate(towerPrefab, position, Quaternion.identity);
        GameObject clone = Instantiate(towerTemplate.towerPrefab, position, Quaternion.identity);
        //������ Ÿ���� ���ǿ� Ÿ�� �Ǽ�
        //Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        //Ÿ�����⿡ enemySpawner ���� ����
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner,playerGold);
    }

}
