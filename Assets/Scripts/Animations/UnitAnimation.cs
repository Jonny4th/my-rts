using MyGame.Core;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Unit))]
public class UnitAnimation : MonoBehaviour
{
    private Animator anim;
    private Unit unit;

    // Start is called before the first frame update
    void Start()
    {
        //why sometime GetComponent at Awake, sometime at start.
        anim = GetComponent<Animator>();
        unit = GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        ChooseAnimation(unit);
    }

    private void ChooseAnimation(Unit unit)
    {
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMove", false);
        anim.SetBool("IsAttack", false);

        if(unit.IsBuilder)
        {
            anim.SetBool("IsBuild", false);
        }

        if(unit.IsWorker)
        {
            anim.SetBool("IsGatherWood", false);
        }
        //can we use trigger instead?

        switch(unit.State)
        {
            case UnitState.Idle:
                anim.SetBool("IsIdle", true);
                break;
            case UnitState.Move:
            case UnitState.MoveToBuild:
            case UnitState.MoveToResource:
            case UnitState.DeliverToHQ:
            case UnitState.MoveToEnemy:
                anim.SetBool("IsMove", true);
                break;
            case UnitState.AttackUnit:
                anim.SetBool("IsAttack", true);
                break;
            case UnitState.BuildProgress:
                anim.SetBool("IsBuild", true);
                break;
            case UnitState.Gather:
                anim.SetBool("IsGatherWood", true);
                break;
        }
    }
}
