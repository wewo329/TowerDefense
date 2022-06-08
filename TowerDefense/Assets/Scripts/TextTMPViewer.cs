using UnityEngine;
using TMPro;

public class TextTMPViewer: MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPlayerHP;
    [SerializeField]
    private TextMeshProUGUI textPlayerGold;
    [SerializeField]
    public TextMeshProUGUI textWave;
    [SerializeField]
    private TextMeshProUGUI textEnemyCount;
    [SerializeField]
    public PlayerHP PlayerHP;
    [SerializeField]
    public PlayerGold PlayerGold;
    [SerializeField]
    private WaveSystem waveSystem;
    [SerializeField]
    private EnemySpawner enemySpawner;

    private void Update()
    {
        textPlayerHP.text = PlayerHP.CurrentHP + "/" + PlayerHP.MaxHP;
        textPlayerGold.text = PlayerGold.CurrentGold.ToString();
        textWave.text = waveSystem.CurrentWave + "/" + waveSystem.MaxWave;
        textEnemyCount.text = enemySpawner.CurrentEnemyCount + "/" + enemySpawner.MaxEnemyCount;
    }
}
