using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace CS4300_Final_Project
{
    abstract class ModelObject
    {
        public Model mModel;
        public abstract void load(ContentManager content, Effect effect);
        public abstract void draw(Matrix viewMatrix, Matrix projectionMatrix);
    }
}
