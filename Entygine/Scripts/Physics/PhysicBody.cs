using Entygine.Mathematics;
using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;

namespace Entygine.Physics
{
    public class PhysicBody
    {
        public float Mass { get; private set; }
        public float InverseMass { get; private set; }

        private Mat3f localInverseInertiaTensor;
        private Mat3f globalInverseInertiaTensor;

        private Vec3f localCentroid;
        private Vec3f globalCentroid;

        private Vec3f position;
        private Mat3f orientation;
        private Mat3f inverseOrientation;

        public Vec3f linearVelocity;
        public Vec3f angularVelocity;

        private Vec3f forceAccumulator;
        private Vec3f torqueAccumulator;

        private Collider[] colliders;

        public bool isStatic;

        private void UpdateGlobalCentroidFromPos()
        {
            globalCentroid = orientation * localCentroid + position;
        }

        private void UpdatePosFromGlobalCentroid()
        {
            position = orientation * -localCentroid + globalCentroid;
        }

        private void UpdateOrientation()
        {
            Quatf q = orientation.GetRotation();
            q.Normalize();
            orientation = Mat3f.CreateRotation(q);
        }

        private void UpdateInertiaTensor()
        {
            globalInverseInertiaTensor = orientation * localInverseInertiaTensor * inverseOrientation;
        }

        public void SetColliders(params Collider[] colls)
        {
            colliders = colls;

            UpdateData();
        }

        private void UpdateData()
        {
            localCentroid = Vec3f.Zero;
            Mass = 0;

            for (int i = 0; i < colliders.Length; i++)
            {
                Collider col = colliders[i];
                col.body = this;
                Mass += col.mass;
                localCentroid += col.mass * col.localCentroid;
            }

            InverseMass = 1 / Mass;
            localCentroid *= InverseMass;

            Mat3f localInertiaTensor = Mat3f.Zero;
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider col = colliders[i];
                Vec3f r = localCentroid - col.localCentroid;
                float rDotR = Vec3f.Dot(r, r);
                Mat3f rOutR = Mat3f.OuterProduct(r, r);

                localInertiaTensor = col.localInertiaTensor + col.mass * (rDotR * Mat3f.Identity - rOutR);
            }

            localInverseInertiaTensor = localInertiaTensor.Inverted();
        }

        public Vec3f LocalToGlobalPos(in Vec3f v)
        {
            return orientation * v + position;
        }
        public Vec3f GlobalToLocalPos(in Vec3f v)
        {
            return inverseOrientation * (v - position);
        }

        public Vec3f LocalToGlobalDir(in Vec3f d)
        {
            return orientation * d;
        }

        public Vec3f GlobalToLocalDir(in Vec3f d)
        {
            return inverseOrientation * d;
        }

        public void ApplyForce(Vec3f force)
        {
            forceAccumulator += force;
        }
        public void ApplyForce(Vec3f force, Vec3f point)
        {
            forceAccumulator += force;
            torqueAccumulator += Vec3f.Cross(point - globalCentroid, force);
        }

        public void Step(float stepTime)
        {
            if (isStatic)
                return;

            linearVelocity += InverseMass * forceAccumulator * stepTime;
            angularVelocity += globalInverseInertiaTensor * torqueAccumulator * stepTime;

            forceAccumulator = Vec3f.Zero;
            angularVelocity = Vec3f.Zero;

            globalCentroid += linearVelocity * stepTime;

            Vec3f axis = angularVelocity.Normalized();
            float angle = angularVelocity.Magnitude * stepTime;
            orientation = Mat3f.CreateRotation(axis, angle) * orientation;

            UpdatePosFromGlobalCentroid();
            UpdateOrientation();
            UpdateInertiaTensor();
        }
    }
}
