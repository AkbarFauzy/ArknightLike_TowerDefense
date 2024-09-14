using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.Characters;
using TowerDefence.Module.Gameplay;
using TowerDefence.Observer;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace TowerDefence.Module.StageUI {
    public class StageCard : MonoBehaviour
    {
        protected List<IStageObserver> _observers = new List<IStageObserver>();

        public Image splash;
        [SerializeField] private StageObserver StageInstance;
        [SerializeField] private TextMeshProUGUI dpcostText;
        [SerializeField] private TextMeshProUGUI cooldownText;

        public TextMeshProUGUI Text_DPCOST { get => dpcostText; }
        public GameObject Op_Prefab { get; set; }

        private int current_dp_cost;
        private PlaceObjectOnGrid grid_placement;

        private bool isCd;
        private bool isActive;
        private StageDP stageDP;

        private enum CardState
        {
            active,
            cooldown,
        }

        private void Start()
        {
            grid_placement = FindObjectOfType<PlaceObjectOnGrid>();
            var op_colliders = Op_Prefab.GetComponentsInChildren<BoxCollider>();
            foreach (var collider in op_colliders)
            {
                collider.enabled = false;
            }
            current_dp_cost = Op_Prefab.GetComponentInChildren<Operator>().CharacterStats.cost;
            dpcostText.SetText(current_dp_cost.ToString());

            stageDP = FindObjectOfType<StageDP>();
            if (stageDP != null)
            {
                stageDP.OnDpChanged += OnDpValueChanged;
            }
            else
            {
                Debug.LogError("StageDP not found!");
            }
        }

        private void Update()
        {
            if (!isActive || isCd)
            {
                gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 140);
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 140);
            }

        }

        public void OnMouseClick()
        {
            NotifyObserver(StageEvents.OperatorPreDeployed);
            if (grid_placement.latestOP == null && !isCd && isActive)
            {
                grid_placement.latestOP = Instantiate(Op_Prefab, transform.position, Quaternion.identity);
                grid_placement.latestOP.GetComponentInChildren<Operator>().stageCard = this;
                grid_placement.latestOP.GetComponent<ObjFollowMouse>().enabled = true;
            }
        }

        public void OnDeployed()
        {
            gameObject.SetActive(false);
            NotifyObserver(StageEvents.OperatorDeployed);
            NotifyUseDP();
        }

        public void OnStandby(float cd)
        {
            gameObject.SetActive(true);
            StartCoroutine(Cooldown(cd));
        }

        private void OnDpValueChanged(float newDpValue)
        {
            isActive = current_dp_cost <= newDpValue;
        }

        private IEnumerator Cooldown(float cd)
        {
            float timer = cd;
            isCd = true;
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                cooldownText.text = string.Format("{0:0.0}", timer);
                yield return new WaitForFixedUpdate();
            }
            isCd = false;
        }

        public void AddObserver(IStageObserver observer)
        {
            Debug.Log(this + " added observer " + observer);
            _observers.Add(observer);
        }

        public void RemoveObserver(IStageObserver observer)
        {
            _observers.Remove(observer);
        }

        protected void NotifyObserver(StageEvents stageEvent)
        {
            _observers.ForEach((_observers) =>
            {
                _observers.OnNotify(stageEvent);
            });
        }

        protected void NotifyObserverOperatorSpawn(Operator op)
        {
            _observers.ForEach((_observers) =>
            {
                _observers.OnNotifyCharacterSpawn(op);
            });
        }

        protected void NotifyUseDP()
        {
            _observers.ForEach((_observers) =>
            {
                _observers.OnDPUsed(current_dp_cost);
            });
        }
    }

}
