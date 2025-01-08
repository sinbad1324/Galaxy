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

    public enum Alagniement
    {
        Start,
        End,
        Center

    }

    public class Flex : IGlobal
    {
        public string name;
        public GlobalUI parent;
        public FlexAlagniement alagniement;
        public Alagniement horizontalAlagniement
        {
            get { return _horizontalAlagniement; }
            set
            {
                _horizontalAlagniement = value;
                SetHorizontalAlagnement(value);
            }
        }
        public Alagniement verticalAlagniement
        {
            get { return _verticalAlagniement; }
            set
            {
                _verticalAlagniement = value;
                SetVerticalAlagnement(value);
            }
        }

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

        private Alagniement _horizontalAlagniement;
        private Alagniement _verticalAlagniement;
        private int parentChildrenCount;
        private Vector2 parentSize;
        private bool _wraping;
        private float horizontalCount;
        private float verticalCount;
        private Dictionary<string , float> childrensInfos;
        private float maxHorizontalCount;
        private float maxVerticalCount;

        private float minHorizontalCount;
        private float minVerticalCount;

        private float maxYSize;
        private float maxXSize;

        private void SetVerticalAlagnement(Alagniement value)
        {
            if (value == Alagniement.Start)
                minVerticalCount = 0;
            else if (value == Alagniement.Center)
              {  minVerticalCount = parent.bgSize.Y / 4;
                maxVerticalCount -= parent.bgSize.Y / 4;
            }
            else
                minVerticalCount = parent.bgSize.Y / 2;
            
        }
        private void SetHorizontalAlagnement(Alagniement value)
        {

            if (value == Alagniement.Start)
                minHorizontalCount = 0;
            else if (value == Alagniement.Center)
            {
                minHorizontalCount = parent.bgSize.X / 4;
                maxHorizontalCount -= parent.bgSize.X / 4;
            }
            else
                minHorizontalCount = parent.bgSize.X / 2;
        }
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
            SetVerticalAlagnement(_verticalAlagniement);
            SetHorizontalAlagnement(_horizontalAlagniement);
        }

        public Flex(GlobalUI parent, string name = "FlexBox", FlexAlagniement alagniement = FlexAlagniement.Auto, float padding = 0)
        {
            this.name = name;
            this.parent = parent;
            this.alagniement = alagniement;
            this.padding = padding;

        }
        public void Initialize()
        {
            horizontalAlagniement = Alagniement.Center;
            verticalAlagniement = Alagniement.Center;
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
            return new Tuple<float, float>(bigX, bigY);
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
                        horizontalCount = minHorizontalCount;
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
                        verticalCount = minVerticalCount;
                        horizontalCount += (maxXSize + padding);
                        i--;
                    }

                }
            }
        }
        private Dictionary<string, float> GetAllChildrenSize()
        {
           Dictionary<string, float> result = new Dictionary<string, float>();
           Vector2 Size = new Vector2(0,0);
            float maxY = 0;
            for (int i = 0; i < parent.childrens.container.Count; i++)
            {
                GlobalUI child = parent.childrens.container[i];
                if (child != null) {
                    maxY = maxY < child.bgSize.Y ? child.bgSize.Y : maxY;
                    Size += child.bgSize; 
                }
            }
            float Xsi = (maxHorizontalCount - minHorizontalCount);
            result["totalSizeX"] = Size.X;
            result["totalSizeY"] = Size.Y;
            result["coucheY"] = (Size.X/ Xsi) *maxY;
            result["couheX"] = Size.X > Xsi ? Xsi : Size.X;

            return result;
        }
        public void Update()
        {
            if (parentChildrenCount != parent.childrens.container.Count || parentSize != parent.bgSize)
            {
                childrensInfos = GetAllChildrenSize();
                Console.WriteLine(childrensInfos);
                Wrap(flexWrap);
                horizontalCount = minHorizontalCount;
                verticalCount = minVerticalCount;
                (maxXSize, maxYSize) = getSizeYXMax();
                Console.WriteLine(maxXSize + " , " + maxYSize);
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
