using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.modules
{     
    public interface IGlobal
        {

        public void Initialize()
        {
        }
        public void LoadContent(ContentManager content, GraphicsDevice device);
        public void Update();
        public void Draw(SpriteBatch target);
    }
}
