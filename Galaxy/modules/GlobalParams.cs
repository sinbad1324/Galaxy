using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Debuger.modules.Timer;
using Galaxy;
using Galaxy.Gui;
using Galaxy.workspace;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LearnMatrix
{
    class GlobalParams
    {
        static public FillMode globalFillMode = FillMode.WireFrame;
        static public  int WINDOW_WIDHT = 1000;
        static public  int WINDOW_HEIGTH = 800;
        static public SpriteBatch spriteBatch;
        static public ContentManager Content;
        static public GraphicsDevice Device;
        static public ScreenGui Screen;
        static public Workspace Workspace;
        static public int raid=0;
        static public GameWindow GameWindow;
        static public Random random = new Random();
        static public List<Delay> delays = new();
         static public GameTime UpdateTime;
         static public GameTime DrawTime;

    }
}
