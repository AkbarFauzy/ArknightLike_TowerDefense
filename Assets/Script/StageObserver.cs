using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;


public class StageObserver : MonoBehaviour, IStageObserver
{
    [System.Serializable]
    private struct OperatorCard
    {
        public Character OP;
        public StageCard Card;

        public OperatorCard(Character op, StageCard card)
        {
            this.OP = op;
            this.Card = card;
        }
    }

    public static StageObserver _stageObserver;
    private PlaceObjectOnGrid _placeObjectOnGrid;
    private List<OperatorCard> operatorCard;
    public CinemachineSwitcher cameraManager;

    [Header("Stage Stats")]
    private int EnemyKilled;
    private int MaxEnemy;
    private float timer;
    public float StageSpeed;
    public int StageHP;
    public StageDP dp;

    [Header("Stages UI")]
    [SerializeField] private TextMeshProUGUI StageHPText;
    [SerializeField] private TextMeshProUGUI EnemyNumberText;

    [Header("Operator Details UI")]
    [SerializeField] private OperatorStageDetails OperatorDetailPanel;
    public GameObject UIOperatorPanelPrefab;
    public GameObject UIOperatorContainer;

    public float Timer {get => timer;}
    private bool _isOPplacementState;

    [SerializeField] private List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    private Dictionary<StageEvents, System.Action> _stageEventHandlers;
    private Dictionary<StageUIEvents, System.Action<Operator>> _stageUIEventHandlers;

    private void Awake()
    {
        _stageEventHandlers = new Dictionary<StageEvents, System.Action>()
        {  
            {StageEvents.Completed, StageCompleted},
            {StageEvents.OperatorPreDeployed, OperatorPreDeployed},
            {StageEvents.OperatorDeployed, OperatorDeployed},
            {StageEvents.EnemyDied, EnemyLeft},
            {StageEvents.GameOver, GameOver},
            {StageEvents.TakeBaseDamage, TakeDamage}
        };

        _stageUIEventHandlers = new Dictionary<StageUIEvents, System.Action<Operator>>();
        _stageUIEventHandlers.Add(StageUIEvents.CharacterDetails, (op) => { OnToogleOperatorDetail(op); });
        _stageUIEventHandlers.Add(StageUIEvents.CharacterAction, (op) => { OnToogleOperatorAction(op); });
        _stageUIEventHandlers.Add(StageUIEvents.CharacterTiles, (op) => { });
        _stageUIEventHandlers.Add(StageUIEvents.CharacterRange, (op) => { });
    }

    private void Start()
    {
        operatorCard = new List<OperatorCard>();

        if (_stageObserver == null)
        {
            _stageObserver = this;
        }
        else {
            Destroy(this);
        }

        for (int i = 0; i < GameManager.Instance.Squad.Count; i++) {
            GameObject opPanel = Instantiate(UIOperatorPanelPrefab, UIOperatorContainer.transform.position, Quaternion.identity);
            operatorCard.Add(new OperatorCard(
                GameManager.Instance.Squad[i].GetComponentInChildren<Character>(), 
                opPanel.GetComponent<StageCard>()
                )
            );
        }

        for (int i = 0; i < operatorCard.Count; i++)
        {
            CharacterStats stats = operatorCard[i].OP.characterStats;

            operatorCard[i].Card.transform.parent = UIOperatorContainer.transform;
            operatorCard[i].Card.splash.sprite = stats.icon;
            operatorCard[i].Card.Op_Prefab = GameManager.Instance.Squad[i];
            operatorCard[i].Card.AddObserver(this);
        }

        _placeObjectOnGrid = FindObjectOfType<PlaceObjectOnGrid>();
        _placeObjectOnGrid.AddObserver(this);

        StageHPText.text = StageHP.ToString();
        enemySpawners.ForEach(enemySpawners => {
            MaxEnemy += enemySpawners.EnemyNumber;
            enemySpawners.AddObserver(this);
        });
        EnemyNumberText.text = MaxEnemy.ToString();

        //add spawner observer
    }

    private void Update()
    {
        if (StageHP == 0) {
            OnNotify(StageEvents.GameOver);
        }

        if (EnemyKilled == MaxEnemy) {
            OnNotify(StageEvents.Completed);
        }
    }

    private void EnemyLeft() {
        EnemyKilled += 1;
        EnemyNumberText.text = EnemyKilled.ToString() + "/" + MaxEnemy;
    }                  

    private void TakeDamage() {
        StageHP -= 1;
        StageHPText.text = StageHP.ToString();

        if (StageHP == 0) {
            OnNotify(StageEvents.GameOver);
        }
    }

    private void StageCompleted() {
        Debug.Log("Completed");
    }

    private void GameOver() {
        Debug.Log("You Lose!");
    }

    private void OperatorPreDeployed() {
        _isOPplacementState = true;
    }

    private void OperatorDeployed() {
        _isOPplacementState = false;
    }

    private void OnToogleOperatorDetail(Operator op) {
        if (!_isOPplacementState)
        {
            OperatorDetailPanel.ToogleOperatorDetails(op);
            if (OperatorDetailPanel.gameObject.activeSelf)
            {
                cameraManager.SwitchState(op.gameObject);
            }
            else
            {
                cameraManager.SwitchState();
            }
        }
    }

    private void OnToogleOperatorAction(Operator op) {
        if (!_isOPplacementState) {
            op.UIOperatorAction.ToogleOperatorActions();
        }
    }

    public void OnNotify(StageEvents stageEvent)
    {
        if (_stageEventHandlers.ContainsKey(stageEvent))
        {
            _stageEventHandlers[stageEvent]();
        }
    }

    public void OnUIEvent(StageUIEvents stageUIEvent, Operator op) {
        if (_stageUIEventHandlers.ContainsKey(stageUIEvent))
        {
            _stageUIEventHandlers[stageUIEvent](op);
        }
    }


    public void OnNotifyCharacterSpawn(Character character)
    {
        Debug.Log("Adding Stage Observer to " + character);
        character.AddObserver(this);
    }

    public void OnDPUsed(int value) {
        dp.SubstractCurrentDP(value);
    }

    public void OnDPGenerated(int value) {
        dp.AddCurrentDP(value);
    }
}
