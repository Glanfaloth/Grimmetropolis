﻿using Microsoft.Xna.Framework;

public class TDCamera : TDComponent
{ 
    public Vector3 CameraTarget { get; private set; }
    public Vector3 CameraUpVector { get; private set; }

    public float FieldOfView { get; set; }
    public float NearPlaneDistance { get; set; }
    public float FarPlaneDistance { get; set; }

    public Matrix ViewMatrix { get; private set; }
    public Matrix ProjectionMatrix { get; private set; }
    public Matrix ViewProjectionMatrix { get; private set; }

    public TDCamera(TDObject tdObject, float fieldOfView, float nearPlaneDistance, float farPlaneDistance)
        : base(tdObject)
    {
        FieldOfView = fieldOfView;
        NearPlaneDistance = nearPlaneDistance;
        FarPlaneDistance = farPlaneDistance;

        UpdateCamera();

        TDSceneManager.ActiveScene.CameraObject = this;
    }

    public void UpdateCamera()
    {
        CalculateCameraTarget();
        CalculateCameraUpVector();
        CalculateViewMatrix();
        CalculateProjectionMatrix();
        CalculateProjectionViewMatrix();
    }

    private void CalculateCameraTarget()
    {
        CameraTarget = TDObject.Transform.Position + Vector3.Transform(Vector3.Right, TDObject.Transform.Rotation);
    }

    private void CalculateCameraUpVector()
    {
        CameraUpVector = Vector3.Transform(Vector3.Backward, TDObject.Transform.Rotation);
    }

    private void CalculateViewMatrix()
    {
        ViewMatrix = Matrix.CreateLookAt(TDObject.Transform.Position, CameraTarget, CameraUpVector);
    }

    private void CalculateProjectionMatrix()
    {
        ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(FieldOfView, TDSceneManager.Graphics.GraphicsDevice.Viewport.AspectRatio, NearPlaneDistance, FarPlaneDistance);
    }

    private void CalculateProjectionViewMatrix()
    {
        ViewProjectionMatrix = ViewMatrix * ProjectionMatrix;
    }
}
