﻿using Microsoft.Xna.Framework;

public class TDLight : TDComponent
{
    // public static GraphicsDeviceManager Graphics { private get; set; }

    public Vector3 LightTarget { get; private set; }
    public Vector3 LightUpTarget { get; private set; }

    public float FieldOfView { get; set; }
    public float NearPlaneDistance { get; set; }
    public float FarPlaneDistance { get; set; }

    public Matrix ViewMatrix { get; private set; }
    public Matrix ProjectionMatrix { get; private set; }
    public Matrix ViewProjectionMatrix { get; private set; }

    public TDLight(TDObject tdObject, float fieldOfView, float nearPlaneDistance, float farPlaneDistance)
        : base(tdObject)
    {
        FieldOfView = fieldOfView;
        NearPlaneDistance = nearPlaneDistance;
        FarPlaneDistance = farPlaneDistance;

        UpdateLight();

        TDSceneManager.ActiveScene.LightObject = this;
    }

    public void UpdateLight()
    {
        CalculateLightTarget();
        CalculateLightUpVector();
        CalculateViewMatrix();
        CalculateProjectionMatrix();
        CalculateProjectionViewMatrix();
    }

    private void CalculateLightTarget()
    {
        LightTarget = TDObject.Transform.Position + Vector3.Transform(Vector3.Right, TDObject.Transform.Rotation);
    }

    private void CalculateLightUpVector()
    {
        LightUpTarget = Vector3.Transform(Vector3.Backward, TDObject.Transform.Rotation);
    }

    private void CalculateViewMatrix()
    {
        ViewMatrix = Matrix.CreateLookAt(TDObject.Transform.Position, LightTarget, LightUpTarget);
    }

    private void CalculateProjectionMatrix()
    {
        ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(FieldOfView, 2.0f, NearPlaneDistance, FarPlaneDistance);
    }

    private void CalculateProjectionViewMatrix()
    {
        ViewProjectionMatrix = ViewMatrix * ProjectionMatrix;
    }
}