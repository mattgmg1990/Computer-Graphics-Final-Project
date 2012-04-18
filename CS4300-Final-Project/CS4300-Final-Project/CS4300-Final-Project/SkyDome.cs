using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace CS4300_Final_Project
{
    class SkyDome : ModelObject
    {
        // Texture for the clouds
        private Texture2D cloudMap;

        public override void load(ContentManager content, Effect effect)
        {
            mModel = content.Load<Model>("dome");
            cloudMap = content.Load<Texture2D>("cloud_texture");

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
            Matrix[] modelTransforms = new Matrix[mModel.Bones.Count];
            mModel.CopyAbsoluteBoneTransformsTo(modelTransforms);

            Matrix worldMatrix = Matrix.CreateTranslation(0.3f, -0.3f, -0.4f) * Matrix.CreateScale(425);
            foreach (ModelMesh mesh in mModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xWorld"].SetValue(modelTransforms[mesh.ParentBone.Index] * worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(viewMatrix);
                    currentEffect.Parameters["xProjection"].SetValue(projectionMatrix);

                    currentEffect.Parameters["xTexture"].SetValue(cloudMap);
                    currentEffect.Parameters["xEnableLighting"].SetValue(false);
                }
                mesh.Draw();
            }
        }
    }
}