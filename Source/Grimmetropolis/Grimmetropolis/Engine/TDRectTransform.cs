using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class TDRectTransform : TDComponent
{
    private TDRectTransform _parent;
    public TDRectTransform Parent
    {
        get => _parent;
        set
        {
            _parent = value;
            AddChildToParent();
        }
    }

    public List<TDRectTransform> Children = new List<TDRectTransform>();

    public Matrix TransformMatrix { get; private set; }

    public Vector2 Origin = Vector2.Zero;

    private Vector2 _localPosition = Vector2.Zero;
    public Vector2 LocalPosition
    {
        get => _localPosition;
        set
        {
            _localPosition = value;
            CalculatePosition();
            CalculateTransform();

            CalculateChildrenPositionTransform();
        }
    }

    private float _localRotation = 0f;
    public float LocalRotation
    {
        get => -_localRotation;
        set
        {
            _localRotation = -value;
            CalculateRotation();
            CalculateTransform();

            CalculateChildrenRotationTransform();
        }
    }

    private Vector2 _localScale = Vector2.One;
    public Vector2 LocalScale
    {
        get => _localScale;
        set
        {
            _localScale = value;
            CalculateScale();
            CalculateTransform();

            CalculateChildrenScaleTransform();
        }
    }

    private Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;
            CalculateLocalPosition();
            CalculateTransform();

            CalculateChildrenPositionTransform();
        }
    }

    private float _rotation;
    public float Rotation
    {
        get => -_rotation;
        set
        {
            _rotation = -value;
            CalculateLocalRotation();
            CalculateTransform();

            CalculateChildrenRotationTransform();
        }
    }

    private Vector2 _scale;
    public Vector2 Scale
    {
        get => _scale;
        set
        {
            _scale = value;
            CalculateLocalScale();
            CalculateTransform();

            CalculateChildrenScaleTransform();
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        _parent = TDObject.Transform.Parent?.TDObject.RectTransform;
    }

    public override void Destroy()
    {
        for (int i = Children.Count - 1; i >= 0; i--)
        {
            Children[i].TDObject.Destroy();
        }

        _parent?.Children.Remove(this);
        TDObject = null;
    }

    private void AddChildToParent()
    {
        _parent?.Children.Add(this);
    }

    private void CalculateLocalPosition()
    {
        _localPosition = _parent == null ? _position : Vector2.Transform(_position, Matrix.Invert(_parent.TransformMatrix));
    }

    private void CalculateLocalRotation()
    {
        _localRotation = Parent == null ? _localRotation : _localRotation + _parent.Rotation;
    }

    private void CalculateLocalScale()
    {
        _localScale = _parent == null ? _localScale : Vector2.Divide(_localScale, _parent.Scale);
    }

    private void CalculatePosition()
    {
        _position = _parent == null ? _localPosition : Vector2.Transform(_localPosition, _parent.TransformMatrix);
    }

    private void CalculateRotation()
    {
        _rotation = _parent == null ? _localRotation : _localRotation - _parent.Rotation;
    }

    private void CalculateScale()
    {
        _scale = _parent == null ? _localScale : Vector2.Multiply(_localScale, _parent.Scale);
    }

    private void CalculateTransform()
    {
        TransformMatrix = Matrix.CreateScale(new Vector3(_scale, 1f)) * Matrix.CreateFromYawPitchRoll(0f, 0f, _rotation) * Matrix.CreateTranslation(new Vector3(_position, 0f));
    }

    private void CalculateChildrenPositionTransform()
    {
        foreach (TDRectTransform child in Children)
        {
            child.CalculatePosition();
            child.CalculateTransform();

            child.CalculateChildrenPositionTransform();
        }
    }

    private void CalculateChildrenRotationTransform()
    {
        foreach (TDRectTransform child in Children)
        {
            child.CalculatePosition();
            child.CalculateRotation();
            child.CalculateTransform();

            child.CalculateChildrenRotationTransform();
        }
    }

    private void CalculateChildrenScaleTransform()
    {
        foreach (TDRectTransform child in Children)
        {
            child.CalculatePosition();
            child.CalculateScale();
            child.CalculateTransform();

            child.CalculateChildrenScaleTransform();
        }
    }
}
