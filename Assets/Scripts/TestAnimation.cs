using MyGame.Core;
using UnityEngine;

public class TestAnimation : MonoBehaviour
{
    [SerializeField]
    private Unit[] units;

    public void SetIdle()
    {
        for(int i = 0; i < units.Length; i++)
        {
            units[i].State = UnitState.Idle;
        }
    }

    public void SetMove()
    {
        for (int i = 0; i < units.Length; i++)
        {
            units[i].State = UnitState.Move;
        }
    }

    public void SetAttack()
    {
        for (int i = 0; i < units.Length; i++)
        {
            units[i].State = UnitState.Attack;
        }
    }

    public void SetChop()
    {
        foreach(Unit unit in units)
        {
            if(unit.IsWorker) unit.State = UnitState.Gather;
        }
    }
}
