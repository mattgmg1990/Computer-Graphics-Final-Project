using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace CS4300_Final_Project
{
    class XWing : ModelObject
    {
        public override void load(ContentManager content, Effect effect)
        {
            mModel = content.Load<Model>("xwing");

            foreach (ModelMesh mesh in mModel.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = effect.Clone();
                }
            }
        }

        public override void draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            Matrix worldMatrix = Matrix.CreateScale(0.0005f, 0.0005f, 0.0005f) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(new Vector3(19, 12, -5));

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
    }
}
