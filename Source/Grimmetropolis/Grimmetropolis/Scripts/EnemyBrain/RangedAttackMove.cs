using System;
using System.Collections.Generic;
using System.Text;

class RangedAttackMove : EnemyMove
{
    private readonly float _attackRange;

    public override Type MovementType => Type.RangedAttack;

    public Structure Target { get; }
    public override float Cost { get; }

    public RangedAttackMove(Location from, Location to, float cost, Structure target, float attackRange) : base(from, to)
    {
        _attackRange = attackRange;
        Cost = cost;
        Target = target;
    }

    public override bool IsMoveAllowed(Type actions, float attackRange)
    {
        return base.IsMoveAllowed(actions, attackRange) && _attackRange <= attackRange;
    }
}

