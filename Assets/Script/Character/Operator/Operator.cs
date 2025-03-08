using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.State;
using TowerDefence.Module.Ability;
using TowerDefence.Module.StageUI;
using TowerDefence.Manager;
using UnityEngine;
using System;

namespace TowerDefence.Module.Characters {
    public class Operator : Character
    {
        protected IOperatorState _currentState;

        public IOperatorState CurrentOperatorState { get => _currentState; }

        public OperatorPreDeployedState PreDeployedState = new OperatorPreDeployedState();
        public OperatorDeployedState DeployedState = new OperatorDeployedState();

        private int _blockedEnemy;
        private float _redeployTime;

        private UIStageCard _stageCard;

        private CubeEditor _deployed_grid;

        public bool MaxedOutBlock() => BlockCount <= _blockedEnemy;

        private void Awake()
        {
            _currentState = PreDeployedState;
            _currentState.OperatorEnterState(this);
            if (StageManager.Instance.UIOperatorAction != null)
            {
                Debug.Log("ObserverAdded");
                AddObserver(StageManager.Instance.UIOperatorAction);
            }
        }

        private new void Start()
        {
            base.Start();
        }

        private void Update()
        {
            if (_currentState != null)
            {
                _currentState.OperatorUpdateState(this);
            }
        }


        private void OnDestroy()
        {
            if (StageManager.Instance.UIOperatorAction != null)
            {
                RemoveObserver(StageManager.Instance.UIOperatorAction);
            }
        }

        public void SwitchState(IOperatorState state)
        {
            _currentState.OperatorExitState(this);

            if (state == null)
            {
                return;
            }

            _currentState = state;
            _currentState.OperatorEnterState(this);
        }

        protected void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy") && !MaxedOutBlock())
            {
                _blockedEnemy += other.gameObject.GetComponent<Enemy>().BlockCount;
                other.gameObject.GetComponent<Enemy>().IsBlocked = true;
            }
        }
        protected void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Character enemy = other.gameObject.GetComponentInChildren<Enemy>();
                if (!Targets.Contains(enemy))
                {
                    Targets.Add(enemy);
                }

                if (enemy.CurrentHP <= 0)
                {
                    _blockedEnemy -= enemy.BlockCount;
                    other.enabled = false;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Character enemy = other.gameObject.GetComponentInChildren<Enemy>();
            Targets.Remove(enemy);
        }

        public void SetStageCard(UIStageCard card)
        {
            _stageCard = card;
        }

        public void SetGrid(CubeEditor grid)
        {
            _deployed_grid = grid;
        }

        public void OnPreDeployed() {
            ToogleAttackRangeVisual(true);
        }

        public void OnDeployed()
        {
            Debug.Log("operator deployed");
            var colliders = GetComponentsInChildren<BoxCollider>();
            foreach (var collider in colliders)
            {
                collider.enabled = true;
            }

            var playerColldier = GetComponent<BoxCollider>();
            playerColldier.enabled = true;
            AbilityHolder.enabled = true;
            _stageCard.OnDeployed();
            ToogleAttackRangeVisual(false);
        }


        public override void OnDied()
        {
            _deployed_grid.isPlaceable = true;
            CurrentCost += Mathf.RoundToInt((float)BaseCost * 1.5f);
            var colliders = gameObject.GetComponentsInChildren<BoxCollider>();
            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }
            StartCoroutine(OnDiedAnimation());
        }

        private IEnumerator OnDiedAnimation()
        {
            _anim.SetTrigger("OnDied");
            yield return new WaitForSeconds(2.0f);
            Destroy(transform.parent.gameObject);
        }

        public void OnMouseDown()
        {
            NotifyOperatorEvents(StageCharacterEvents.CharacterDetails, this);
            NotifyOperatorEvents(StageCharacterEvents.CharacterAction, this);
        }

        public void InvokeCancel()
        {
            Destroy(gameObject.transform.parent);
        }

    }

}
