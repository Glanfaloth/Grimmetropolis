using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class TDTransformComponent : TDComponent
{
    private TDObject _parent;

    public TDObject Parent
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

    public List<TDObject> Children = new List<TDObject>();

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
        }
    }

    public TDTransformComponent(TDObject tdObject, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, TDObject parent)
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
        _parent?.Transform.Children.Add(TDObject);
    }

    private void CalculateLocalPosition()
    {
        _localPosition = Parent == null ? _position : Vector3.Transform(_position, Matrix.Invert(Parent.Transform.TransformMatrix));
    }

    private void CalculateLocalRotation()
    {
        _localRotation = Parent == null ? _localRotation : _localRotation * Quaternion.Inverse(Parent.Transform.Rotation);
    }

    private void CalculateLocalScale()
    {
        _localScale = Parent == null ? _localScale : Vector3.Divide(_localScale, Parent.Transform.Scale);
    }

    private void CalculatePosition()
    {
        _position = Parent == null ? _localPosition : Vector3.Transform(_localPosition, Parent.Transform.TransformMatrix);
    }

    private void CalculateRotation()
    {
        _rotation = Parent == null ? _localRotation : _localRotation * Parent.Transform.Rotation;
    }

    private void CalculateScale()
    {
        _scale = Parent == null ? _localScale : Vector3.Multiply(_localScale, Parent.Transform.Scale);
    }

    private void CalculateTransform()
    {
        TransformMatrix = Matrix.CreateScale(_scale) * Matrix.CreateFromQuaternion(_rotation) * Matrix.CreateTranslation(_position);
    }
}
