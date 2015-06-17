using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace YoYo
{
    public class Camera : GameComponent
    {
        // camera
        private Vector3 cameraPosition;
        private Vector3 cameraRotation;
        private float cameraSpeed;
        private Vector3 cameraLookAt;
        //mouse
        private Vector3 mouseRotationBuffer;
        private Vector3 holderForMouseRotation;
        private MouseState currentMouseState;
        private MouseState prevMouseState;
        //private float mouseRotationSpeed;
        private Game game;

        public Vector3 Position
        {
            get { return cameraPosition; }
            set
            {
                cameraPosition = value;
          
            }
        }

        public Vector3 Rotation
        {
            get { return cameraRotation; }
            set
            {
                cameraRotation = value;
             
            }
        }

        public Matrix Projection
        {
            get;
            protected set;
        }



        public Matrix View
        {
            get { return Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up); }
        }


        public Camera(Game game, Vector3 position, Vector3 rotation, float speed)
            : base(game)
        {
            cameraSpeed = speed;

            //setup projection matrix
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                Game.GraphicsDevice.Viewport.AspectRatio,
                0.05f,
                1000.0f);

            MoveTo(position, rotation);
            this.game = game;

            prevMouseState = Mouse.GetState();

        }



        //set camera's position and rotation
        private void MoveTo(Vector3 pos, Vector3 rot)
        {
            Position = pos;
            Rotation = rot;
        }

        //update the look at vector
        private void UpdateLookAt()
        {
            //build a rotation matrix
            Matrix rotationMatrix = Matrix.CreateRotationX(cameraRotation.X) * Matrix.CreateRotationY(cameraRotation.Y);
            //build look at offset vector
            Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitZ, rotationMatrix);
            //update our camera's look at vector
            cameraLookAt = cameraPosition + lookAtOffset;
        }


        //method that simulate movement
        private Vector3 PreviewMove(Vector3 amount)
        {
            Matrix rotate = Matrix.CreateRotationY(cameraRotation.Y);
            Vector3 movement = new Vector3(amount.X, amount.Y, amount.Z);
            movement = Vector3.Transform(movement, rotate);
            return cameraPosition + movement;
        }

        //method that actually moves the camera
        public void Move(Vector3 scale)
        {
            MoveTo(PreviewMove(scale), Rotation);
        }

        public void clearCameraLookAt()
        {
            mouseRotationBuffer = Vector3.Zero;
            cameraLookAt = Vector3.Zero;
            Rotation = Vector3.Zero;
            holderForMouseRotation = Vector3.Zero;
        }

        //update method
        public override void Update(GameTime gameTime)
        {


            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds; // delta time

            currentMouseState = Mouse.GetState();

            //////////////////////////// handle movement ////////////////////////////////////////
            KeyboardState ks = Keyboard.GetState();


            Vector3 moveVector = Vector3.Zero;


            // keyboard and DPad control
            if (ks.IsKeyDown(Keys.W))
                moveVector.Z = 0.5f;
            if (ks.IsKeyDown(Keys.S))
                moveVector.Z = -0.5f;
            if (ks.IsKeyDown(Keys.A))
                moveVector.X = 0.5f;
            if (ks.IsKeyDown(Keys.D))
                moveVector.X = -0.5f;

            if (ks.IsKeyDown(Keys.T))
                moveVector.Y = 100;
            if (ks.IsKeyDown(Keys.Y))
                moveVector.Y = -100;



            if (moveVector != Vector3.Zero)
            {
                //normalize the vector
                moveVector.Normalize();
                moveVector *= dt * cameraSpeed;

                // simulate next move and check for collision

                Move(moveVector);

            }

            base.Update(gameTime);
        }
    }


}