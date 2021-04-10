using System;
using System.Collections.Generic;
using System.Text;

class RangedAttackMove : EnemyMove
{
    private readonly float _attackRange;

    private readonly float _baseCost;
    private readonly float _rangeFactor;

    public override Type MovementType => Type.RangedAttack;

    public Building Target { get; }
    public override float Cost => _baseCost + _attackRange * _rangeFactor;

    public RangedAttackMove(Location from, Location to, Building target, float attackRange, float baseCost, float rangeFactor) : base(from, to)
    {
        Target = target;
        _attackRange = attackRange;
        _baseCost = baseCost;
        _rangeFactor = rangeFactor;
    }

    public override bool IsMoveAllowed(Type actions, float attackRange)
    {
        return base.IsMoveAllowed(actions, attackRange) && _attackRange <= attackRange;
    }
}

