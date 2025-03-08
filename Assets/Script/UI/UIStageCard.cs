using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.Characters;
using TowerDefence.Module.Gameplay;
using TowerDefence.Observer;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace TowerDefence.Module.StageUI {
    public class UIStageCard : StageSubject
    {
        public Image splash;
        [SerializeField] private TextMeshProUGUI _dpcostText;
        [SerializeField] private TextMeshProUGUI _cooldownText;

        public GameObject OperatorPrefab;
        private GameObject _operatorGameobject;

        private int _current_dp_cost;

        private bool _isCd;
        private bool _isActive;
        private UIStageDP _stageDP;

        private enum CardState
        {
            active,
            cooldown,
        }

        private void Start()
        {
            _stageDP = FindObjectOfType<UIStageDP>();
            if (_stageDP != null)
            {
                _stageDP.OnDpChanged += OnDpValueChanged;
                AddObserver(_stageDP);
            }
            else
            {
                Debug.LogError("StageDP not found!");
            }

            var op_colliders = OperatorPrefab.GetComponentsInChildren<BoxCollider>();
            foreach (var collider in op_colliders)
            {
                collider.enabled = false;
            }

            _operatorGameobject = Instantiate(OperatorPrefab, transform.position, Quaternion.identity);
            var _operator = _operatorGameobject.GetComponentInChildren<Operator>();
            _operator.SetStageCard(this);
            _current_dp_cost = _operator.CharacterStats.cost;
            _operatorGameobject.SetActive(false);
            _dpcostText.SetText(_current_dp_cost.ToString());
        }

        private void Update()
        {
            if (!_isActive || _isCd)
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
            if (_isActive && !_isCd)
            {
                NotifyOperatorEvents(StageCharacterEvents.CharacterPreDeployed, _operatorGameobject.GetComponentInChildren<Operator>());
            }
        }

        public void OnDeployed()
        {
            gameObject.SetActive(false);
            NotifyUseDP(_current_dp_cost);
        }

        public void OnStandby(float cd)
        {
            gameObject.SetActive(true);
            StartCoroutine(Cooldown(cd));
        }

        private void OnDpValueChanged(float newDpValue)
        {
            _isActive = _current_dp_cost <= newDpValue;
        }

        private IEnumerator Cooldown(float cd)
        {
            float timer = cd;
            _isCd = true;
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                _cooldownText.text = string.Format("{0:0.0}", timer);
                yield return new WaitForFixedUpdate();
            }
            _isCd = false;
        }
    }

}
