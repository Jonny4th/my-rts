using System;
using UnityEngine;
using UnityEngine.AI;

namespace MyGame.Core
{
    public enum UnitState
    {
        Idle,
        Move,
        Attack,
        MoveToBuild, //builder goes to build
        BuildProgress, //builder builds in progress
        Die,
    }

    [Serializable]
    public struct UnitCost
    {
        public int food;
        public int wood;
        public int gold;
        public int stone;
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : MonoBehaviour
    {
        [Header("Generic Info")]
        [SerializeField] private int id;
        public int ID { get { return id; } set { id = value; } }

        [SerializeField] private string unitName;
        public string UnitName { get { return unitName; } }

        [SerializeField] private Sprite unitPic;
        public Sprite UnitPic { get { return unitPic; } }

        [Header("Status")]
        [SerializeField] private int curHP;
        public int CurHP { get { return curHP; } set { curHP = value; } }

        [SerializeField] private int maxHP = 100;
        public int MaxHP { get { return maxHP; } }

        [SerializeField] private int moveSpeed = 5;
        public int MoveSpeed { get { return moveSpeed; } }

        [SerializeField] private int minWpnDamage;
        public int MinWpnDamage { get { return minWpnDamage; } }

        [SerializeField] private int maxWpnDamage;
        public int MaxWpnDamage { get { return maxWpnDamage; } }

        [SerializeField] private int armour;
        public int Armour { get { return armour; } }

        [SerializeField] private float visualRange;
        public float VisualRange { get { return visualRange; } }

        [SerializeField] private float weaponRange;
        public float WeaponRange { get { return weaponRange; } }

        [SerializeField] private UnitState state;
        public UnitState State { get { return state; } set { state = value; } }

        private NavMeshAgent navAgent;
        public NavMeshAgent NavAgent { get { return navAgent; } }

        [SerializeField] private Faction faction;
        public Faction Faction { get { return faction; } set { faction = value; } }

        [SerializeField] private bool isBuilder;
        public bool IsBuilder { get { return isBuilder; } set { isBuilder = value; } }

        [SerializeField] private Builder builder;
        public Builder Builder { get { return builder; } }

        [Header("Info")]
        [SerializeField] private UnitCost unitCost;
        public UnitCost UnitCost { get { return unitCost; } }

        //time for increasing progress 1% for this unit, less is faster
        [SerializeField] private float unitWaitTime = 0.1f;
        public float UnitWaitTime { get { return unitWaitTime; } }

        void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();

            if(IsBuilder) builder = GetComponent<Builder>();
        }

        // Update is called once per frame
        void Update()
        {
            switch (state)
            {
                case UnitState.Move:
                    MoveUpdate();
                    break;
            }
        }

        public void SetState(UnitState toState)
        {
            state = toState;

            if (state == UnitState.Idle)
            {
                navAgent.isStopped = true;
                navAgent.ResetPath();
            }
        }

        public void MoveToPosition(Vector3 dest)
        {
            if (navAgent != null)
            {
                navAgent.SetDestination(dest);
                navAgent.isStopped = false;
            }

            SetState(UnitState.Move);
        }

        private void MoveUpdate()
        {
            float distance = Vector3.Distance(transform.position, navAgent.destination);

            if (distance <= 1f)
                SetState(UnitState.Idle);
        }

        public void LookAt(Vector3 pos)
        {
            Vector3 dir = (pos - transform.position).normalized;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
}