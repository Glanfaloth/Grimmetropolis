using System;

internal class AttackMove : EnemyMove
{

    public override Type MovementType => Type.Attack;

    public TDObject Target { get; }
    public AttackMove(Location from, Location to, float cost, TDObject target) : base(from, to, cost)
    {
        Target = target;
    }
}
