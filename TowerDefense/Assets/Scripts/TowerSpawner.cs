using UnityEngine;

public class TowerSpawner: MonoBehaviour
{
    [SerializeField]
    private TowerTemplate towerTemplate;     //차워 정보(공격력, 공격속도 등)
    /*[SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private int towerBuildGold = 50; //타워 건설에 사용되는 골드*/
    [SerializeField]
    private EnemySpawner enemySpawner; //현재 맵에 존재하는 적 리스트 정보를 얻기 위해..
    [SerializeField]
    private PlayerGold playerGold;// 타워 건설 시 골드 감소를 위해..
    [SerializeField]
    private SystemTextViewer systemTextViewer;    //돈 부족, 건설 불가와 같은 시스템 메세지 출력
    private bool isOnTowerButton = false;        //타워 건설 버튼 눌렀는지 체크

    public void ReadyToSpawnTower()
    {
        //타워 건설 가능 여부 확인
        //타워를 건설할 만큼 돈이 없으면 타워 건설 X
        if(towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        {
            //골드가 부족해서 타워 건설이 불가능하다고 출력
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        //타워 건설 버튼을 눌렀다고 설정
        isOnTowerButton = true;
    }

    public void SpawnTower(Transform tileTransform)
    {

        //타워 건설 버튼을 눌렀을 때만 타워 건설 가능
        if (isOnTowerButton == false)
        {
            return;
        }
        
      /*  //타워 건설 가능 여부 확인
        // 1. 타워를 건설할 만큼 돈이 없으면 타워 건설 X
        //if(towerBuildGold > playerGold.CurrentGold)
        if(towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        {
            //골드가 부족해서 타워 건설이 불가능하다고 출력
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }*/
        Tile tile = tileTransform.GetComponent<Tile>();

        //2. 현재 타일의 위치에 이미 타워가 건설되어 있으면 타워 건설 X
        if (tile.IsBuildTower == true)
        {
            //현재 위치에 타워 건설에 불가능하다고 출력
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        //다시 타워 건설 버튼을 눌러서 타워를 건설하도록 변수 설정
        isOnTowerButton = false;
        //타워가 건설되어 있음으로 설정
        tile.IsBuildTower = true;
        //타워 건설에 필요한 골드만큼 감소
        //playerGold.CurrentGold -= towerBuildGold;
        playerGold.CurrentGold -= towerTemplate.weapon[0].cost;
        //선택한 타일의 위치에 타워 건설(타일보다 z축 01의 위치에 배치)
        Vector3 position = tileTransform.position + Vector3.back;
        //GameObject clone = Instantiate(towerPrefab, position, Quaternion.identity);
        GameObject clone = Instantiate(towerTemplate.towerPrefab, position, Quaternion.identity);
        //선택한 타일의 위피에 타워 건설
        //Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        //타워무기에 enemySpawner 정보 전달
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner,playerGold,tile);
    }

}
