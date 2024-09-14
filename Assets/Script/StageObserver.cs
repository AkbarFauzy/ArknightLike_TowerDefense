using System.Collections.Generic;
using TowerDefence.Module.Characters;
using TowerDefence.Module.Spawner;
using TowerDefence.Module.StageUI;
using TowerDefence.Module.Gameplay;
using UnityEngine;
using TMPro;

namespace TowerDefence.Observer {
    [System.Serializable]
    public struct OperatorCard
    {
        public Character OP;
        public StageCard Card;

        public OperatorCard(Character op, StageCard card)
        {
            this.OP = op;
            this.Card = card;
        }
    }

    public class StageObserver : MonoBehaviour, IStageObserver
    {
        public static StageObserver StageObserverInstance;

        private PlaceObjectOnGrid _placeObjectOnGrid;
        private List<OperatorCard> _operatorCard;
        public CinemachineSwitcher cameraManager;

        [Header("Stage Stats")]
        private int _enemyKilled;
        private int _maxEnemy;
        private float _timer;
        public float StageSpeed;
        public int StageHP;
        public StageDP DP;

        [Header("Stages UI")]
        [SerializeField] private TextMeshProUGUI _stageHPText;
        [SerializeField] private TextMeshProUGUI _enemyNumberText;

        [Header("Operator Details UI")]
        [SerializeField] private OperatorStageDetails _operatorDetailPanel;
        public GameObject UIOperatorPanelPrefab;
        public GameObject UIOperatorContainer;

        public float Timer {get => _timer;}
        private bool _isOPplacementState;

        [SerializeField] private List<EnemySpawner> _enemySpawners = new List<EnemySpawner>();
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

            _stageUIEventHandlers = new Dictionary<StageUIEvents, System.Action<Operator>>
            {
                { StageUIEvents.CharacterDetails, (op) => { OnToogleOperatorDetail(op); } },
                { StageUIEvents.CharacterAction, (op) => { OnToogleOperatorAction(op); } },
                { StageUIEvents.CharacterTiles, (op) => { } },
                { StageUIEvents.CharacterRange, (op) => { } }
            };
        }

        private void Start()
        {
            _operatorCard = new List<OperatorCard>();

            if (StageObserverInstance == null)
            {
                StageObserverInstance = this;
            }
            else {
                Destroy(this);
            }

            for (int i = 0; i < GameManager.Instance.Squad.Count; i++) {
                GameObject opPanel = Instantiate(UIOperatorPanelPrefab, UIOperatorContainer.transform.position, Quaternion.identity);
                _operatorCard.Add(new OperatorCard(
                    GameManager.Instance.Squad[i].GetComponentInChildren<Character>(), 
                    opPanel.GetComponent<StageCard>()
                    )
                );
            }

            for (int i = 0; i < _operatorCard.Count; i++)
            {
                CharacterStats stats = _operatorCard[i].OP.CharacterStats;

                _operatorCard[i].Card.transform.parent = UIOperatorContainer.transform;
                _operatorCard[i].Card.splash.sprite = stats.icon;
                _operatorCard[i].Card.Op_Prefab = GameManager.Instance.Squad[i];
                _operatorCard[i].Card.AddObserver(this);
            }

            _placeObjectOnGrid = FindObjectOfType<PlaceObjectOnGrid>();
            _placeObjectOnGrid.AddObserver(this);

            _stageHPText.text = StageHP.ToString();
            _enemySpawners.ForEach(enemySpawners => {
                _maxEnemy += enemySpawners.EnemyNumber;
                enemySpawners.AddObserver(this);
            });
            _enemyNumberText.text = _maxEnemy.ToString();

            //add spawner observer
        }

        private void Update()
        {
            if (StageHP == 0) {
                OnNotify(StageEvents.GameOver);
            }

            if (_enemyKilled == _maxEnemy) {
                OnNotify(StageEvents.Completed);
            }
        }

        private void EnemyLeft() {
            _enemyKilled += 1;
            _enemyNumberText.text = _enemyKilled.ToString() + "/" + _maxEnemy;
        }                  

        private void TakeDamage() {
            StageHP -= 1;
            _stageHPText.text = StageHP.ToString();

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
                _operatorDetailPanel.ToogleOperatorDetails(op);
                if (_operatorDetailPanel.gameObject.activeSelf)
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
            DP.SubstractCurrentDP(value);
        }

        public void OnDPGenerated(int value) {
            DP.AddCurrentDP(value);
        }
    }
}