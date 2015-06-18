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


        }



        //set camera's position and rotation
        private void MoveTo(Vector3 pos, Vector3 rot)
        {
            Position = pos;
            Rotation = rot;
        }

     
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
    }


}