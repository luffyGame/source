using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class Jetpack : MonoBehaviour
    {
        private JetPackPath path;
        private float speed = 1f, height = 0.5f, decayStart = .75f, decayAmount = .25f;
        private bool jetPackOn;

        void Update()
        {
            MoveWithJetPack();
        }

        private void MoveWithJetPack()
        {
            if (jetPackOn)
            {
                transform.position = path.GetPosition();
                if (path.IsFinished())
                {
                    transform.position = path.GetEndPosition();
                    jetPackOn = false;
                }
            }
        }

        public void GetJetPackOn(Vector3 end)
        {
            if (!jetPackOn)
            {
                path = new JetPackPath(transform.position, end, speed, height, decayStart, decayAmount);
                jetPackOn = true;
            }
        }
    }

    public class JetPackPath
    {
        private Vector3 startPosition, endPosition, distance;
        private float height, width;
        private float position, speed;
        private float decayAmount, decayStart, decayWindow;

        public JetPackPath(Vector3 startPosition, Vector3 endPosition, float speed, float height, float decayStart, float decayAmount)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.speed = speed;
            this.height = height;
            this.decayStart = decayStart;
            this.decayAmount = decayAmount;
            distance = endPosition - startPosition;
            width = .5f;
            decayWindow = 1 - decayStart;
        }

        public Vector3 GetPosition()
        {
            position += GetSpeed() * Time.fixedDeltaTime;
            return startPosition + new Vector3(GetXPosition(), GetYPosition(), GetZPosition());
        }

        public float GetSpeed()
        {
            if (position >= decayStart)
            {
                float decay = position - decayWindow;
                float proportion = decay / decayWindow;
                return speed - (proportion * speed * decayAmount);
            }
            return speed;
        }

        public Vector3 GetEndPosition()
        {
            return endPosition;
        }

        public bool IsFinished()
        {
            return position >= 1.0;
        }

        private float GetXPosition()
        {
            return position * distance.x;
        }

        private float GetYPosition()
        {
            float yOffset = distance.y * position;
            float yPosition = GetSquareRoot();
            if (float.IsNaN(yPosition)) yPosition = 0;
            return yPosition + yOffset;
        }

        private float GetZPosition()
        {
            return position * distance.z;
        }

        private float GetSquareRoot()
        {
            float fraction = Squared(position - width) / Squared(width);
            float toBeSquareRooted = (1 - fraction) * Squared(height);
            return (float)Math.Sqrt(toBeSquareRooted);
        }

        private float Squared(float value)
        {
            return value * value;
        }
    }
}
