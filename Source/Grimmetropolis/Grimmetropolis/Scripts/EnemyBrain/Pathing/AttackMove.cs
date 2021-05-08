using System;

internal class AttackMove : EnemyMove
{

    public override Type MovementType => Type.Attack;

    public ITarget Target { get; }

    public override float Cost { get; }

    public AttackMove(Location from, Location to, float cost, ITarget target) : base(from, to)
    {
        Target = target;
        Cost = cost;
    }

    public override NextMoveInfo CreateInfo()
    {
        return new NextMoveInfo(Target, MovementType, Target.TDObject.Transform.LocalPosition.GetXY());
    }
}
