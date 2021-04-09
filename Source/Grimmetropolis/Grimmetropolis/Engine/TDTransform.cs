using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class TDTransform : TDComponent
{
    private TDTransform _parent;

    public TDTransform Parent
    {
        get => _parent;
        set
        {
            _parent?.Children.Remove(this);

            _parent = value;
            AddChildToParent();

            CalculateLocalPosition();
            CalculateLocalRotation();
            CalculateLocalScale();
        }
    }

    public List<TDTransform> Children = new List<TDTransform>();
    public List<TDRectTransform> ChildrenUI = new List<TDRectTransform>();

    public Matrix TransformMatrix { get; private set; }

    private Vector3 _localPosition = Vector3.Zero;
    public Vector3 LocalPosition
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

    private Quaternion _localRotation = Quaternion.Identity;
    public Quaternion LocalRotation
    {
        get => _localRotation;
        set
        {
            _localRotation = value;
            CalculateRotation();
            CalculateTransform();

            CalculateChildrenRotationTransform();
        }
    }

    private Vector3 _localScale = Vector3.One;
    public Vector3 LocalScale
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

    private Vector3 _position = Vector3.Zero;
    public Vector3 Position
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

    private Quaternion _rotation = Quaternion.Identity;
    public Quaternion Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            CalculateLocalRotation();
            CalculateTransform();

            CalculateChildrenRotationTransform();
        }
    }

    private Vector3 _scale = Vector3.One;
    public Vector3 Scale
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

    public override void Destroy()
    {
        for (int i = Children.Count - 1; i >= 0; i--)
        {
            Children[i].TDObject.Destroy();
        }

        for (int i = ChildrenUI.Count - 1; i >= 0; i--)
        {
            ChildrenUI[i].TDObject.Destroy();
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
        _localPosition = _parent == null ? _position : Vector3.Transform(_position, Matrix.Invert(_parent.TransformMatrix));
    }

    private void CalculateLocalRotation()
    {
        _localRotation = Parent == null ? _rotation : Quaternion.Inverse(_parent.Rotation) * _rotation;
    }

    private void CalculateLocalScale()
    {
        _localScale = _parent == null ? _scale : Vector3.Divide(_scale, _parent.Scale);
    }

    private void CalculatePosition()
    {
        _position = _parent == null ? _localPosition : Vector3.Transform(_localPosition, _parent.TransformMatrix);
    }

    private void CalculateRotation()
    {
        _rotation = _parent == null ? _localRotation : _parent.Rotation * _localRotation;
    }

    private void CalculateScale()
    {
        _scale = _parent == null ? _localScale : Vector3.Multiply(_localScale, _parent.Scale);
    }

    private void CalculateTransform()
    {
        TransformMatrix = Matrix.CreateScale(_scale) * Matrix.CreateFromQuaternion(_rotation) * Matrix.CreateTranslation(_position);
    }

    private void CalculateChildrenPositionTransform()
    {
        foreach (TDTransform child in Children)
        {
            child.CalculatePosition();
            child.CalculateTransform();

            child.CalculateChildrenPositionTransform();
        }
    }

    private void CalculateChildrenRotationTransform()
    {
        foreach (TDTransform child in Children)
        {
            child.CalculatePosition();
            child.CalculateRotation();
            child.CalculateTransform();

            child.CalculateChildrenRotationTransform();
        }
    }

    private void CalculateChildrenScaleTransform()
    {
        foreach (TDTransform child in Children)
        {
            child.CalculatePosition();
            child.CalculateScale();
            child.CalculateTransform();

            child.CalculateChildrenScaleTransform();
        }
    }
}
