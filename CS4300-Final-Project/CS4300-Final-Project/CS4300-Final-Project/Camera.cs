using System;
using System.Collections;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CS4300_Final_Project
{
    class Camera
    {
        // Has any movement been made?
        public Boolean m_movementMade = false;

        // The position vector of this camera
        public Vector3 m_position;
        // The target vector of this camera
        private Vector3 m_target;
        // The up vector of this camera
        private Vector3 m_upVector;

        public Matrix m_LookAtMatrix;
        public Matrix m_ProjectionMatrix;

        // The terrain for the world that this camera will "walk" on
        private Terrain mTerrain;

        // Window height and width 
        private int m_windowHeight;
        private int m_windowWidth;

        // The default vectors for forward and right, relative to this camera
        public static readonly Vector3 DefaultForward = new Vector3(0.0f, 0.0f, 1.0f);
        public static readonly Vector3 DefaultRight = new Vector3(1.0f, 0.0f, 0.0f);

        // The current forward and right vectors for this camera
        Vector3 camForward = new Vector3(0.0f, 0.0f, 1.0f);
        Vector3 camRight = new Vector3(1.0f, 0.0f, 0.0f);

        // The rotation matrix of this camera
        Matrix m_camRotationMatrix;

        // The current amount to move in different directions
        float m_moveLeftRight = 0.0f;
        float m_moveBackForward = 0.0f;

        // The current yaw and pitch of the camera 
        float m_Yaw = 0.0f;
        float m_Pitch = 0.0f;

        // The last position of mouse
        MouseState m_previousMouseState;
        MouseState m_currentMouseState;

        private float m_aspectRatio;

        // The movement and look speeds for this camera
        private static readonly float MOVEMENTSPEED = 0.09f;
        private static readonly float LOOKSPEED = 0.03f;

        /// <summary>
        /// Constructor for the Camera class. 
        /// </summary>
        /// <param name="device">The device that will camera will operate in.</param>
        /// <param name="position">The initial position vector of the camera</param>
        /// <param name="m_target">The initial target vector</param>
        /// <param name="m_upVector">The initial up vector</param>
        /// <param name="windowWidth">The width of the application window</param>
        /// <param name="windowHeight">The height of the application window</param>
        /// <param name="aspectRatio">The aspect ratio of this game window</param>
        /// <param name="terrain">The terrain object that this camera will "walk" on</param>
        public Camera(Vector3 position, Vector3 target, Vector3 upVector, int windowWidth, int windowHeight, float aspectRatio, Terrain terrain)
        {
            m_position = position;
            m_target = target;
            m_upVector = upVector;
            m_windowWidth = windowWidth;
            m_windowHeight = windowHeight;
            m_aspectRatio = aspectRatio;
            mTerrain = terrain;
        }

        public void processInput()
        {
            ///// Handle the position of the camera with the w,a,s,d keys or arrow keys.
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                m_moveBackForward += MOVEMENTSPEED;
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                m_moveBackForward -= MOVEMENTSPEED;
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                m_moveLeftRight -= MOVEMENTSPEED;
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                m_moveLeftRight += MOVEMENTSPEED;

            m_currentMouseState = Mouse.GetState();

            int centerX = m_windowWidth / 2;
            int centerY = m_windowHeight / 2;
            int deltaX = centerX - m_currentMouseState.X;
            int deltaY = centerY - m_currentMouseState.Y;

            // If the mouse has moved right, make the camera look right
            if (m_currentMouseState.X > m_previousMouseState.X)
            {
                m_Yaw -= LOOKSPEED;
            }
            // If the mouse has moved left, make the camera look left
            if (m_currentMouseState.X < m_previousMouseState.X)
            {
                m_Yaw += LOOKSPEED;
            }
            // If the mouse has moved up, make the camera look up
            if (m_currentMouseState.Y > m_previousMouseState.Y)
            {
                m_Pitch += LOOKSPEED;
            }
            // If the mouse has moved down, make the camera look down
            if (m_currentMouseState.Y < m_previousMouseState.Y)
            {
                m_Pitch -= LOOKSPEED;
            }

            Mouse.SetPosition(centerX, centerY);
            m_previousMouseState = Mouse.GetState();
            renderCamera();
        }
       
        /// <summary>
        /// Using the information obtained from inputs, update the camera's position, target, and up vectors.
        /// </summary>
        private void UpdateCamera()
        {
            // Since the pitch and yaw just increase constantly as the inputs are given (in radians),
            // they need to be reduced to the relative rotation between 0 and 2 * PI. This 
            // is accomplished simply by using modulo 2 * PI on each value.
            m_Pitch = m_Pitch % (float)(2 * Math.PI);
            m_Yaw = m_Yaw % (float)(2 * Math.PI);

            // From the pitch and yaw, create a rotation matrix and transform the target vector to the new value.
            m_camRotationMatrix = Matrix.CreateFromYawPitchRoll(m_Yaw, m_Pitch, 0);
            m_target = Vector3.Transform(DefaultForward, m_camRotationMatrix);
            m_target = Vector3.Normalize(m_target);

            // Find a matrix representing the transform needed to get the forward vector
            Matrix RotateYTempMatrix;
            RotateYTempMatrix = Matrix.CreateRotationY(m_Yaw);

            // Calculate new right, up, and forward vectors using the temporary rotation matrix
            camRight = Vector3.Transform(DefaultRight, RotateYTempMatrix);
            m_upVector = Vector3.Transform(m_upVector, RotateYTempMatrix);
            camForward = Vector3.Transform(DefaultForward, RotateYTempMatrix);

            // Save the original position
            Vector3 originalPosition = m_position;

            // Change the position based on the new right and forward vectors
            m_position += m_moveLeftRight * camRight;
            m_position += m_moveBackForward * camForward;

            // If there is no height for this position, we're not walking on the terrain. Stop the camera from strafing that way.
            try
            {
                // Change the height to the new value
                m_position.Y = getHeightOnTerrain();
            }
            catch (IndexOutOfRangeException e)
            {
                // Stop the camera by returning it to its original position
                m_position = originalPosition;
            }

            // reset the moveleftright and movebackforward values
            m_moveLeftRight = 0.0f;
            m_moveBackForward = 0.0f;

            // update the target vector relative to the new position
            m_target = m_position + m_target;
        }

        /// <summary>
        /// For the current X and Z position of this camera, find the height of the terrain, interpolated for the specific position
        /// </summary>
        /// <returns>An interpolated height at the current X,Z position of the camera</returns>
        private float getHeightOnTerrain()
        {
            // Current X and Z positions, casted to the nearest int value
            int x0 = (int)m_position.X;
            int y0 = (int)m_position.Z;

            // The difference between the current position and its closest int value
            double dx = m_position.X - x0;
            double dy = m_position.Z - y0;

            // Subtract from 1 to find the distance from the next hightest integer
            double omdx = 1 - dx;
            double omdy = 1 - dy;

            // find the bilinear interpolation between the height values at the data points in the height map
            // closest to the current position of the camera.
            double bilinear = omdx * omdy * mTerrain.getHeight(x0, y0) +
            omdx * dy * mTerrain.getHeight(x0, y0 + 1) +
            dx * omdy * mTerrain.getHeight(x0 + 1, y0) +
            dx * dy * mTerrain.getHeight(x0 + 1, y0 + 1);

            // Add 2 to the terrain height so that the camera is above it
            return (float)bilinear + 2.0f;
        }

        
        /// <summary>
        /// Update the camera using the latest input, and render it with the device
        /// </summary>
        public void renderCamera()
        {
            // Update the camera
            UpdateCamera();

            // Render it
            m_LookAtMatrix =
                Matrix.CreateLookAt(m_position,
                    m_target, m_upVector);
             m_ProjectionMatrix =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, m_aspectRatio, 1.0f, 300.0f);
        }
    }
}
