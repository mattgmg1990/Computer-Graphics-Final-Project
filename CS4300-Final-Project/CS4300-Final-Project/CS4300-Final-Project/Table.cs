using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace CS4300_Final_Project
{
    class Table : ModelObject
    {
        public override void load(ContentManager content, Effect effect)
        {
            mModel = content.Load<Model>("table");
        }
        
        /// <summary>
        /// Draw the table!
        /// </summary>
        /// <param name="viewMatrix">The current view matrix</param>
        /// <param name="projectionMatrix">The current projection matrix</param>
        public override void draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            Matrix worldMatrix = Matrix.Identity * Matrix.CreateTranslation(new Vector3(50, 33, -70));

            Matrix[] tableTransforms = new Matrix[mModel.Bones.Count];
            mModel.CopyAbsoluteBoneTransformsTo(tableTransforms);
            foreach (ModelMesh mesh in mModel.Meshes)
            {
                foreach (BasicEffect currentEffect in mesh.Effects)
                {
                    currentEffect.EnableDefaultLighting();
                    currentEffect.World = tableTransforms[mesh.ParentBone.Index] * worldMatrix;
                    currentEffect.View = viewMatrix;
                    currentEffect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }
        }
    }
}
