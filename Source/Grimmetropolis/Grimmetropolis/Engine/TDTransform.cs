using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class TDTransform : TDComponent
{
    private TDTransform _parent;

    public TDTransform Parent
    {
        get { return _parent; }

        set
        {
            _parent = value;
            AddChildToParent();

            CalculateLocalPosition();
            CalculateLocalRotation();
            CalculateLocalScale();
        }
    }

    public List<TDTransform> Children = new List<TDTransform>();

    public Matrix TransformMatrix { get; private set; }

    private Vector3 _localPosition;
    public Vector3 LocalPosition
    {
        get { return _localPosition; }
        set
        {
            _localPosition = value;
            CalculatePosition();
            CalculateTransform();

            CalculateChildrenPosition();
            CalculateChildrenTransform();
        }
    }

    private Quaternion _localRotation;
    public Quaternion LocalRotation
    {
        get { return _localRotation; }
        set
        {
            _localRotation = value;
            CalculateRotation();
            CalculateTransform();

            CalculateChildrenRotation();
            CalculateChildrenTransform();
        }
    }

    private Vector3 _localScale;
    public Vector3 LocalScale
    {
        get { return _localScale; }
        set
        {
            _localScale = value;
            CalculateScale();
            CalculateTransform();

            CalculateChildrenScale();
            CalculateChildrenTransform();
        }
    }

    private Vector3 _position;
    public Vector3 Position
    {
        get { return _position; }
        set
        {
            _position = value;
            CalculateLocalPosition();
            CalculateTransform();

            CalculateChildrenPosition();
            CalculateChildrenTransform();
        }
    }

    private Quaternion _rotation;
    public Quaternion Rotation
    {
        get { return _rotation; }
        set
        {
            _rotation = value;
            CalculateLocalRotation();
            CalculateTransform();

            CalculateChildrenRotation();
            CalculateChildrenTransform();
        }
    }

    private Vector3 _scale;
    public Vector3 Scale
    {
        get { return _scale; }
        set
        {
            _scale = value;
            CalculateLocalScale();
            CalculateTransform();

            CalculateChildrenScale();
            CalculateChildrenTransform();
        }
    }

    public TDTransform(TDObject tdObject, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, TDTransform parent)
        : base(tdObject)
    {
        _parent = parent;
        AddChildToParent();

        _localPosition = localPosition;
        _localRotation = localRotation;
        _localScale = localScale;
        CalculatePosition();
        CalculateRotation();
        CalculateScale();
        CalculateTransform();
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
        _localRotation = Parent == null ? _localRotation : _localRotation * Quaternion.Inverse(_parent.Rotation);
    }

    private void CalculateLocalScale()
    {
        _localScale = _parent == null ? _localScale : Vector3.Divide(_localScale, _parent.Scale);
    }

    private void CalculatePosition()
    {
        _position = _parent == null ? _localPosition : Vector3.Transform(_localPosition, _parent.TransformMatrix);
    }

    private void CalculateRotation()
    {
        _rotation = _parent == null ? _localRotation : _localRotation * _parent.Rotation;
    }

    private void CalculateScale()
    {
        _scale = _parent == null ? _localScale : Vector3.Multiply(_localScale, _parent.Scale);
    }

    private void CalculateTransform()
    {
        TransformMatrix = Matrix.CreateScale(_scale) * Matrix.CreateFromQuaternion(_rotation) * Matrix.CreateTranslation(_position);
    }

    private void CalculateChildrenPosition()
    {
        foreach (TDTransform child in Children)
        {
            child.CalculatePosition();
        }
    }

    private void CalculateChildrenRotation()
    {
        foreach (TDTransform child in Children)
        {
            child.CalculateRotation();
            child.CalculatePosition();
        }
    }

    private void CalculateChildrenScale()
    {
        foreach (TDTransform child in Children)
        {
            child.CalculateScale();
            child.CalculatePosition();
        }
    }

    private void CalculateChildrenTransform()
    {
        foreach (TDTransform child in Children)
        {
            child.CalculateTransform();
        }
    }
}
