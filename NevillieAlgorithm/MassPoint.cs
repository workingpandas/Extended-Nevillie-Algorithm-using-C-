using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NevillieAlgorithm
{
    public class MassPoint
    {

        public float Mass { get => mass; private set => mass = value; }

        public Vector2 position;
        public Vector2 velocity;
        public Vector2 acceleration;
        public float mass;

        public MassPoint()
        {
            position = new Vector2();
            velocity = new Vector2();
            acceleration = new Vector2();
            mass = 1.0f;
        }

        public MassPoint(Vector2 _position, float _mass)
        {
            position = _position;
            velocity = new Vector2(0.0f, 0.0f);
            acceleration = new Vector2(0.0f, 0.0f);
            mass = _mass;
        }

        public void ChangeMass(float newMass)
        {
            if (Math.Abs(mass - newMass) > Single.Epsilon)
            {
                position = (position * (1.0f / mass)) * newMass;
                velocity = (velocity * (1.0f / mass)) * newMass;
                acceleration = (acceleration * (1.0f / mass)) * newMass;
            }
        }

        public static MassPoint operator *(MassPoint masspoint, float t)
        {
            return new MassPoint(masspoint.position * t, masspoint.mass * t);
        }

        public static MassPoint operator +(MassPoint lhs, MassPoint rhs)
        {
            return new MassPoint(lhs.position + rhs.position, lhs.mass + rhs.mass);
        }

        public Vector2 GetTransformedPosition()
        {
            return new Vector2(position.x / mass, position.y / mass);
        }
    }
}
