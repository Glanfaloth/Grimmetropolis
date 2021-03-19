using Microsoft.Xna.Framework;

public class TDCamera : TDComponent
{
    public static GraphicsDeviceManager Graphics { private get; set; }

    public Vector3 CameraPosition
    {
        get { return TDObject.Transform.Position; }
        set
        {
            TDObject.Transform.Position = value;
            CalculateViewMatrix();
            CalculateCameraTarget();
            CalculateProjectionViewMatrix();
        }
    }

    public Vector3 CameraTarget { get; private set; }
    public Vector3 CameraUpVector { get; private set; }
    public Quaternion CameraRotation
    {
        get { return TDObject.Transform.Rotation; }
        set
        {
            TDObject.Transform.Rotation = value;
            CalculateCameraTarget();
            CalculateCameraUpVector();
            CalculateViewMatrix();
            CalculateProjectionViewMatrix();
        }
    }

    private float _fieldOfView;
    public float FieldOfView
    {
        get { return _fieldOfView; }
        set
        {
            _fieldOfView = value;
            CalculateProjectionMatrix();
            CalculateProjectionViewMatrix();
        }
    }

    private float _nearPlaneDistance;
    public float NearPlaneDistance
    {
        get { return _nearPlaneDistance; }
        set
        {
            _nearPlaneDistance = value;
            CalculateProjectionMatrix();
            CalculateProjectionViewMatrix();
        }
    }

    private float _farPlaneDistance;
    public float FarPlaneDistance
    {
        get { return _farPlaneDistance; }
        set
        {
            _farPlaneDistance = value;
            CalculateProjectionMatrix();
            CalculateProjectionViewMatrix();
        }
    }

    public Matrix ViewMatrix { get; private set; }
    public Matrix ProjectionMatrix { get; private set; }
    public Matrix ViewProjectionMatrix { get; private set; }

    public TDCamera(TDObject tdObject, float fieldOfView, float nearPlaneDistance, float farPlaneDistance)
        : base(tdObject)
    {
        _fieldOfView = fieldOfView;
        _nearPlaneDistance = nearPlaneDistance;
        _farPlaneDistance = farPlaneDistance;

        CalculateCameraTarget();
        CalculateCameraUpVector();
        CalculateViewMatrix();
        CalculateProjectionMatrix();
        CalculateProjectionViewMatrix();

        TDSceneManager.ActiveScene.CameraObject = this;
    }

    private void CalculateCameraTarget()
    {
        CameraTarget = CameraPosition + Vector3.Transform(Vector3.Right, TDObject.Transform.Rotation);
    }

    private void CalculateCameraUpVector()
    {
        CameraUpVector = Vector3.Transform(Vector3.Backward, TDObject.Transform.Rotation);
    }

    private void CalculateViewMatrix()
    {
        ViewMatrix = Matrix.CreateLookAt(CameraPosition, CameraTarget, CameraUpVector);
    }

    private void CalculateProjectionMatrix()
    {
        ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(_fieldOfView, Graphics.GraphicsDevice.Viewport.AspectRatio, _nearPlaneDistance, _farPlaneDistance);
    }

    private void CalculateProjectionViewMatrix()
    {
        ViewProjectionMatrix = ViewMatrix * ProjectionMatrix;
    }
}
