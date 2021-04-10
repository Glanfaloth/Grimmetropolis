using System;

internal class AttackMove : EnemyMove
{

    public override Type MovementType => Type.Attack;

    public Building Target { get; }

    public override float Cost { get; }

    public AttackMove(Location from, Location to, float cost, Building target) : base(from, to)
    {
        Target = target;
        Cost = cost;
    }
}
