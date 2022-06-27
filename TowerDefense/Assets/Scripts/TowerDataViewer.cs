using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;
    [SerializeField]
    private TextMeshProUGUI textDamage ;
    [SerializeField]
    private TextMeshProUGUI textRate;
    [SerializeField]
    private TextMeshProUGUI textRange;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private TowerAttackRange towerAttackRange;
    [SerializeField]
    private Button buttonUpgrade;
    [SerializeField]
    private SystemTextViewer systemTextViewer;

    private TowerWeapon currentTower;

    private void Awake()
    {
        OffPanel();
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel(Transform towerWeapon)
    {
        //����ؾ��ϴ� Ÿ�� ������ �޾ƿͼ� ����
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        //Ÿ�� ���� Panel On
        gameObject.SetActive(true);
        //Ÿ�� ������ ����
        UpdateTowerData();
        //Ÿ�� ������Ʈ �ֺ��� ǥ�õǴ� Ÿ�� ���ݹ��� Sprite On
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }

    public void OffPanel()
    {
        //Ÿ�� ���� Panel off
        gameObject.SetActive(false);
        //Ÿ�� ���� ���� Sprite off
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData()
    {
        imageTower.sprite = currentTower.TowerSprite;
        textDamage.text = "Damage : " + currentTower.Damage;
        textRate.text = "Rate : " + currentTower.Rate;
        textRange.text = "Range : " + currentTower.Range;
        textLevel.text = "Level : " + currentTower.Level;

        //���׷��̵尡 �Ұ��������� ��ư ��Ȱ��ȭ
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }

    public void OnclickEventTowerUpgrade()
    {
        //Ÿ�� ���׷��̵� �õ� (����:true, ����:false)
        bool isSuccess = currentTower.Upgrade();

        if(isSuccess == true)
        {
            //Ÿ���� ���׷��̵� �Ǿ��� ������ Ÿ�� ���� ����
            UpdateTowerData();
            //Ÿ�� �ֺ��� ���̴� ���ݹ����� ����
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            //Ÿ�� ���׷��̵忡 �ʿ��� ����� �����ϴٰ� ���
            systemTextViewer.PrintText(SystemType.Money);
        }

    }

    public void OnclickEventTowerSell()
    {
        //Ÿ�� �Ǹ�
        currentTower.Sell();
        //������ Ÿ���� ������� Panel, ���ݹ��� off
        OffPanel();
    }
}
