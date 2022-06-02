using UnityEngine;

public class TowerSpawner: MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private int towerBuildGold = 50; //Ÿ�� �Ǽ��� ���Ǵ� ���
    [SerializeField]
    private EnemySpawner enemySpawner; //���� �ʿ� �����ϴ� �� ����Ʈ ������ ��� ����..
    [SerializeField]
    private PlayerGold playerGold;// Ÿ�� �Ǽ� �� ��� ���Ҹ� ����..

    public void SpawnTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();

        //Ÿ�� �Ǽ� ���� ���� Ȯ��
        // 1. Ÿ���� �Ǽ��� ��ŭ ���� ������ Ÿ�� �Ǽ� X
        if(towerBuildGold > playerGold.CurrentGold)
        {
            return;
        }
        
        //1. ���� Ÿ���� ��ġ�� �̹� Ÿ���� �Ǽ��Ǿ� ������ Ÿ�� �Ǽ� X
        if(tile.IsBuildTower == true)
        {
            return;
        }
        //Ÿ���� �Ǽ��Ǿ� �������� ����
        tile.IsBuildTower = true;
        //Ÿ�� �Ǽ��� �ʿ��� ��常ŭ ����
        playerGold.CurrentGold -= towerBuildGold;
        //������ Ÿ���� ���ǿ� Ÿ�� �Ǽ�
        Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        //������ Ÿ���� ��ġ�� Ÿ�� �Ǽ�
        GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        //Ÿ�����⿡ enemySpawner ���� ����
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
    }

}
