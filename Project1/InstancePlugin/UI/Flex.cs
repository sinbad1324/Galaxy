using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Galaxy.Gui.GuiInterface;
using Galaxy.modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Project1.InstancePlugin.UI
{
    public enum FlexAlagniement
    {
        Auto,
        Horizontal,
        Vertical
    }

    public enum Alagniement {
        Start,
        End,
        Center
    
    }

    public class Flex : IGlobal
    {
        public string name;
        public GlobalUI parent;
        public FlexAlagniement alagniement;
        public float padding;
        public bool flexWrap
        {
            get { return _wraping; }
            set
            {
                parentChildrenCount = 0;
                Wrap(value);
                _wraping = value;
            }

        }


        private int parentChildrenCount;
        private Vector2 parentSize;
        private bool _wraping;
        private float horizontalCount;
        private float verticalCount;

        private float maxHorizontalCount;
        private float maxVerticalCount;

        private float maxYSize;
        private float maxXSize;

        private void Wrap(bool value)
        {
            if (value == false)
            {
                maxHorizontalCount = float.PositiveInfinity;
                maxVerticalCount = float.PositiveInfinity;
            }
            else
            {
                maxHorizontalCount = parent.bgSize.X;
                maxVerticalCount = parent.bgSize.Y;
            }
        }

        public Flex(GlobalUI parent, string name = "FlexBox", FlexAlagniement alagniement = FlexAlagniement.Auto, float padding = 0)
        {
            this.name = name;
            this.parent = parent;
            this.alagniement = alagniement;
            this.padding = padding;

            Console.WriteLine("FlexAdded");
        }
        public void Initialize()
        {
            parentChildrenCount = 0;
            parentSize = new Vector2(0, 0);
            horizontalCount = 0;
            verticalCount = 0;
            maxYSize = 0;
            maxXSize = 0;
            flexWrap = false;
        }
        public void LoadContent(ContentManager content, GraphicsDevice device) { }

        private Tuple<float, float> getSizeYXMax()
        {
            float bigY = 0;
            float bigX = 0;

            for (int i = 0; i < parent.childrens.container.Count; i++)
            {
                GlobalUI child = parent.childrens.container[i];
                if (child.bgSize.Y >= bigY)
                    bigY = child.bgSize.Y;

                if (child.bgSize.X >= bigX)
                    bigX = child.bgSize.X;
            }
            return new Tuple<float , float >(bigX ,  bigY);
        }


        private void Horizontal()
        {
            for (int i = 0; i < parent.childrens.container.Count; i++)
            {
                GlobalUI child = parent.childrens.container[i];
                if (child != null)
                {
                    if (((child.bgSize.X + horizontalCount) + padding) <= maxHorizontalCount)
                    {
                        if (horizontalCount == 0)
                        {
                            child.position = new Vector2(0, verticalCount);
                            horizontalCount += child.bgSize.X;
                        }
                        else
                        {
                            child.position = new Vector2(horizontalCount, verticalCount);
                            horizontalCount += (child.bgSize.X + padding);
                        }
                    }
                    else
                    {
                        horizontalCount = 0;
                        verticalCount += (maxYSize + padding);
                        Console.WriteLine(verticalCount);
                        i--;
                    }

                }
            }
        }
        private void Vertical()
        {
            for (int i = 0; i < parent.childrens.container.Count; i++)
            {
                GlobalUI child = parent.childrens.container[i];
                if (child != null)
                {
                    if (((child.bgSize.Y + verticalCount) + padding) <= maxVerticalCount)
                    {
                        if (child.bgSize.X >= maxXSize)
                            maxXSize = child.bgSize.X;

                        if (verticalCount == 0)
                        {
                            child.position = new Vector2(horizontalCount, 0);
                            verticalCount += child.bgSize.Y;
                        }
                        else
                        {
                            child.position = new Vector2(horizontalCount, verticalCount);
                            verticalCount += (child.bgSize.Y + padding);
                        }
                    }
                    else
                    {
                        verticalCount = 0;
                        horizontalCount += (maxXSize + padding);
                        i--;
                    }

                }
            }
        }
        public void Update()
        {
            if (parentChildrenCount != parent.childrens.container.Count || parentSize != parent.bgSize)
            {
                Wrap(flexWrap);
                horizontalCount = 0;
                verticalCount = 0;
               ( maxXSize, maxYSize) = getSizeYXMax();
                Console.WriteLine(maxXSize +" , "+ maxYSize);
                if (alagniement == FlexAlagniement.Horizontal)
                    Horizontal();
                else if (alagniement == FlexAlagniement.Vertical)
                    Vertical();
                else
                {
                    if (maxHorizontalCount > maxVerticalCount)
                        Horizontal();
                    else
                        Vertical();
                }

                parentChildrenCount = parent.childrens.container.Count;
                parentSize = parent.bgSize;
            }

        }
        public void Draw(SpriteBatch target) { }
    }
}
