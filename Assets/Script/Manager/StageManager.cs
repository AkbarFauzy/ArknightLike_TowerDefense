using System.Collections.Generic;
using TowerDefence.Module.Characters;
using TowerDefence.Module.Spawner;
using TowerDefence.Module.StageUI;
using TowerDefence.Module.Gameplay;
using TowerDefence.Observer;
using UnityEngine;
using TMPro;

namespace TowerDefence.Manager {
    [System.Serializable]
    public struct OperatorCard
    {
        public Character OP;
        public UIStageCard Card;

        public OperatorCard(Character op, UIStageCard card)
        {
            this.OP = op;
            this.Card = card;
        }
    }

    public class StageManager : StageObserver
    {
        public static StageManager Instance;

        private PlaceObjectOnGrid _placeObjectOnGrid;
        private List<OperatorCard> _operatorCard;
        public CinemachineSwitcher cameraManager;

        [Header("Stage Stats")]
        private int _enemyKilled;
        private int _maxEnemy;
        private float _timer;
        public float StageSpeed;
        public int StageHP;

        [Header("Stages UI")]
        [SerializeField] private TextMeshProUGUI _stageHPText;
        [SerializeField] private TextMeshProUGUI _enemyNumberText;
        [SerializeField] private GameObject _stageFailedPanel;
        [SerializeField] private GameObject _stageCompletedPanel;

        [Header("Operator Details UI")]
        [SerializeField] private OperatorStageDetails _operatorDetail;
        public GameObject UIOperatorPanelPrefab;
        public GameObject UIOperatorContainer;
        public UIOperatorActions UIOperatorAction;

        public float Timer { get => _timer; }
        private bool _isOPplacementState;

        [SerializeField] private List<EnemySpawner> _enemySpawners = new List<EnemySpawner>();

        protected new void Awake()
        {
            base.Awake();
            _stageEventHandlers = new Dictionary<StageEvents, System.Action>()
            {
                {StageEvents.Completed, StageCompleted},
                {StageEvents.EnemyDied, OnEnemyDied},
                {StageEvents.GameOver, GameOver},
                {StageEvents.TakeBaseDamage, TakeDamage}
            };

            _stageOperatorEventHandlers = new Dictionary<StageCharacterEvents, System.Action<Operator>>()
            {
                { StageCharacterEvents.CharacterPreDeployed,  (op) => {OperatorPreDeployed(op); } },
                { StageCharacterEvents.CharacterDeployed, (op) => { OnOperatorDeployed(op); } },
                { StageCharacterEvents.CharacterDetails, (op) => { OnToogleOperatorDetail(op); } },
                { StageCharacterEvents.CharacterDied, (op) => { OnOperatorDied(op); } },
            };

            _stageEnemyEventHandlers = new Dictionary<StageCharacterEvents, System.Action<Enemy>>()
            {
                {StageCharacterEvents.CharacterDeployed, (enemy) => {OnEnemyDeployed(enemy); } },
            };
        }

        protected new void Start()
        {
            base.Start();
            _operatorCard = new List<OperatorCard>();

            if (Instance == null)
            {
                Instance = this;
            }
            else {
                Destroy(this);
            }

            for (int i = 0; i < GameManager.Instance.Squad.Count; i++) {
                GameObject opPanel = Instantiate(UIOperatorPanelPrefab, UIOperatorContainer.transform.position, Quaternion.identity);
                _operatorCard.Add(new OperatorCard(
                    GameManager.Instance.Squad[i].GetComponentInChildren<Character>(),
                    opPanel.GetComponent<UIStageCard>()
                    )
                );
            }

            for (int i = 0; i < _operatorCard.Count; i++)
            {
                CharacterStats stats = _operatorCard[i].OP.CharacterStats;

                _operatorCard[i].Card.transform.SetParent(UIOperatorContainer.transform);
                _operatorCard[i].Card.splash.sprite = stats.icon;
                _operatorCard[i].Card.OperatorPrefab = GameManager.Instance.Squad[i];
                _operatorCard[i].Card.AddObserver(this);
                _operatorCard[i].Card.AddObserver(UIOperatorAction);
            }

            _placeObjectOnGrid = FindObjectOfType<PlaceObjectOnGrid>();
            _placeObjectOnGrid.AddObserver(this);
            _placeObjectOnGrid.AddObserver(UIOperatorAction);

            _stageHPText.text = StageHP.ToString();
            _enemySpawners.ForEach(enemySpawners => {
                _maxEnemy += enemySpawners.EnemyNumber;
                enemySpawners.AddObserver(this);
            });
            _enemyNumberText.text = _maxEnemy.ToString();
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

        public void OnEnemyDied() {
            _enemyKilled += 1;
            _enemyNumberText.text = _enemyKilled.ToString() + "/" + _maxEnemy;
        }

        private void TakeDamage() {
            Debug.Log("taking damage");
            StageHP -= 1;
            _stageHPText.text = StageHP.ToString();

            if (StageHP == 0) {
                OnNotify(StageEvents.GameOver);
            }
        }

        private void OperatorPreDeployed(Operator op) {
          /*  UIOperatorAction.ShowOperatorActionsPanel(op);*/
            _placeObjectOnGrid.SetCurrentOperator(op.transform.parent.gameObject);
            _placeObjectOnGrid.CurrentOperator.GetComponent<ObjFollowMouse>().enabled = true;
            _isOPplacementState = true;
        }

        private void OnOperatorDeployed(Operator op) {
            /*UIOperatorAction.HideOperatorActionsPanel();*/
            _isOPplacementState = false;
            op.AddObserver(this);
            op.AddObserver(_operatorDetail);
        }

        private void OnOperatorDied(Operator op){
            op.RemoveObserver(this);
            op.RemoveObserver(_operatorDetail);
        }

        private void OnEnemyDeployed(Enemy enemy) {
            enemy.AddObserver(this);
        }

        private void OnToogleOperatorDetail(Operator op) {
            if (!_isOPplacementState)
            {
                if (!_operatorDetail.IsOperatorDetailOpen)
                {
                    cameraManager.SwitchState(op.gameObject);
                }
                else
                {
                    cameraManager.SwitchState();
                }
            }
        }

        private void StageCompleted()
        {
            Debug.Log("Completed");
        }

        private void GameOver()
        {
            Debug.Log("You Lose!");
        }

    }
}