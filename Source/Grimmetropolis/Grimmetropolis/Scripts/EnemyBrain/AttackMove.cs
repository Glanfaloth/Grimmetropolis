using System;

internal class AttackMove : EnemyMove
{

    public override Type MovementType => Type.Attack;

    public Structure Target { get; }
    public AttackMove(Location from, Location to, float cost, Structure target) : base(from, to, cost)
    {
        Target = target;
    }
}
