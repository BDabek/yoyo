using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace YoYo
{
    public class YoYo
    {
        public Model model { get; set; }
        public float mass { get; set; }
        public float velocity { get; set; }
        public float acceleration { get; set; }
        public float g { get; set; }
        public float radius { get; set; }
        public float tension { get; set; }
        public float rot { get; set; }
        public float s { get; set; }
        public float length { get; set; }
        public Vector3 position { get; set; }
        public Vector3 rotation { get; set; }
        float inertia;
        float bigRadius;
        float angularAcceleration;
        float upForce;
        double elapsedTime;
        float firstAcc;
        public YoYo(Model model, float mass)
        {
            this.model = model;
            this.mass = 0.1f;

            this.velocity = 0.001f;
            this.s = 0;
            this.length = 30;
            this.position = new Vector3(0, length, 0);
            this.rotation = new Vector3(0);
            this.radius = 0.3f;
            this.bigRadius = 0.5f;
            this.g = 9.81f;
            this.elapsedTime = 0;

            inertia = 1 / 2 * (mass / 3) * radius * radius + (mass / 3) * bigRadius * bigRadius;

            tension = mass * g * (inertia / (mass * radius * radius)) / ((1 + inertia) / ((mass * radius * radius)));
            this.acceleration = g / (1 + (inertia / (mass * radius * radius)));
            firstAcc = acceleration;
            angularAcceleration = acceleration / radius;
            upForce = inertia * angularAcceleration / radius;
            // this.acceleration = (float)(0.1f /(1+ (1/2*this.mass*Math.Pow(radius,2))/this.mass*))
            System.Console.WriteLine("prędkość {0}", tension);
        }
        public void Draw(Camera camera)
        {
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    Effect effect = meshPart.Effect;
                    ((BasicEffect)effect).World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
                    * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                    * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z)) * Matrix.CreateTranslation(position);

                    ((BasicEffect)effect).View = camera.View;

                    ((BasicEffect)effect).Projection = camera.Projection;
                }

                mesh.Draw();
            }

        }
        public void push(float strength)
        {
            this.velocity += strength/2;
        }
        public void pull(float strength)
        {
            this.acceleration -= strength*10;
        }
        public void UpdatePosition(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            System.Console.WriteLine("prędkość {0}, s {1}, przyspieszenie {2}, czas {3}", velocity, s, acceleration, gameTime.TotalGameTime.TotalSeconds);


            // double xd = 2 / 3f * (float)g * ((float)length - (float)s);
            // velocity += acceleration;// (float)Math.Sqrt(Math.Abs(xd));
            if (s > length)
                s = length;
            if (length - s <= 0)
            {
                velocity *= -1;
                acceleration = upForce / mass - acceleration;
                //acceleration = 0;
            }

            if (velocity <= 0f)
            {
                acceleration -= velocity / 15 + 0.0001f;
                //  if acceleration()
            }
            else if (acceleration < firstAcc)
            {

                acceleration += Math.Abs(acceleration) / 8 + 0.0001f;
            }
            else
                acceleration = firstAcc;
            //if (acceleration > firstAcc)
            //    acceleration = firstAcc;

            velocity += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            s += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds / 2;


            position = new Vector3(position.X, length / 2 - s, position.Z);

            if (acceleration > firstAcc)
                rot += firstAcc / radius;
            else
                rot += (acceleration / radius);
          //  rot= (float)Math.Sqrt((2*mass*(g*(length-s)-(velocity*velocity/2)))/(inertia));
            rotation = new Vector3(0, 0, rot);
           





        }
    }
}
