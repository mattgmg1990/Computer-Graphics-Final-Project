using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using System.IO;

namespace CS4300_Final_Project
{
    class XWing : ModelObject
    {
        float MOVEMENT_SPEED = 0.1f;

        string PERLIN_NOISE_FILE = "Content/perlin_noise_text.txt";

        float[,] mNoiseArray;

        // These keep track of the noise position
        int noiseX = 0;
        int noiseY = 0;

        Vector3 mPosition = new Vector3(-15, 40, -70);

        public override void load(ContentManager content, Effect effect)
        {
            mModel = content.Load<Model>("xwing");

            mNoiseArray = loadPerlinNoiseFromFile();

            foreach (ModelMesh mesh in mModel.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = effect.Clone();
                }
            }
        }

        private float[,] loadPerlinNoiseFromFile()
        {
            // The array to return
            float[,] perlinArray = new float[100,100];

            System.IO.Stream stream = TitleContainer.OpenStream(PERLIN_NOISE_FILE);
            System.IO.StreamReader file = new System.IO.StreamReader(stream);
            
            String line = "";
            int lineCount = 0;
            while ((line = file.ReadLine()) != null)
            {
                string[] rowValues = line.Split(' ');
                for (int i = 0; i < 100; i++)
                {
                    perlinArray[lineCount, i] = -70 + ((float.Parse(rowValues[i]) - 0.45f) * 8.0f);
                }
                lineCount++;
            }

            file.Close();

            return perlinArray;
        }

        public void move()
        {
            // Update the position
            mPosition.Z = mNoiseArray[noiseX, noiseY];
            mPosition.X += MOVEMENT_SPEED;

            // Increment the noise values
            noiseX++;
            noiseY++;

            // Reset the values if they are outside the noise range
            if (noiseX > 99)
            {
                noiseX = 0;
            }
            if (noiseY > 99)
            {
                noiseY = 0;
            }
        }

        private Vector3 getMovementVector()
        {
            //int decision = (int)Math.Round(noiseValue) % 2;

            //Vector3 movementVector = new Vector3(0, 0, 0);

            //if (decision == 0)
            //{
            //    movementVector.X += MOVEMENT_SPEED;
            //}
            //else
            //{
            //    movementVector.Z += MOVEMENT_SPEED;
            //}

            //return movementVector;
            Vector3 movementVector = new Vector3(0, 0, 0);

            int lastX = noiseX - 1;
            int lastY = noiseY - 1;

            if(lastX < 0) { lastX = noiseX; }
            if(lastY < 0) { lastY = noiseY; }

            float delta = mNoiseArray[noiseX, noiseY] - mNoiseArray[lastX, lastY];

            movementVector.X += MOVEMENT_SPEED;
            movementVector.Z += mNoiseArray[noiseX, noiseY];

            return movementVector;

            
        }

        public override void draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            Matrix worldMatrix = Matrix.CreateScale(0.02f, 0.02f, 0.02f) * Matrix.CreateRotationY(MathHelper.Pi / 2.0f) * Matrix.CreateTranslation(mPosition);

            Matrix[] xwingTransforms = new Matrix[mModel.Bones.Count];
            mModel.CopyAbsoluteBoneTransformsTo(xwingTransforms);
            foreach (ModelMesh mesh in mModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Colored"];
                    currentEffect.Parameters["xWorld"].SetValue(xwingTransforms[mesh.ParentBone.Index] * worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(viewMatrix);
                    currentEffect.Parameters["xProjection"].SetValue(projectionMatrix);
                }
                mesh.Draw();
            }
        }

        /// <summary>
        /// Move this XWing around
        /// </summary>
        /// <param name="dx">Change in X</param>
        /// <param name="dy">Change in Y</param>
        /// <param name="dz">Change in Z</param>
        public void move(float dx, float dy, float dz)
        {
            mPosition.X += dx;
            mPosition.Y += dy;
            mPosition.Z += dz;
        }
    }
}
