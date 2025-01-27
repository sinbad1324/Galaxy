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
        public float inSetPadding;
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
            {
                minVerticalCount = (parent.bgSize.Y / 2) - (childrensInfos["coucheY"] / 2);
                maxVerticalCount = float.IsInfinity(maxVerticalCount) ? float.PositiveInfinity : (parent.bgSize.Y / 2)  + childrensInfos["coucheY"] / 2;
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
                minHorizontalCount = (parent.bgSize.X / 2) - (childrensInfos["couheX"] / 2);

                maxHorizontalCount = float.IsInfinity(maxHorizontalCount) ? float.PositiveInfinity : (parent.bgSize.X / 2) + childrensInfos["couheX"] / 2;

            }
            else
                minHorizontalCount = parent.bgSize.X / 2;
        }
       private void setDefaulMAXSIZE(bool value)
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
        private void Wrap(bool value)
        {
            setDefaulMAXSIZE(value);
            SetVerticalAlagnement(_verticalAlagniement);
            SetHorizontalAlagnement(_horizontalAlagniement);
        }
        public Flex(GlobalUI parent, string name = "FlexBox", FlexAlagniement alagniement = FlexAlagniement.Auto, float padding = 0)
        {
            this.name = name;
            this.parent = parent;
            this.alagniement = alagniement;
            this.padding = padding;
            childrensInfos = GetAllChildrenSize();
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
            inSetPadding = 50;
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
            short Xcount =0;
            for (int i = 0; i < parent.childrens.container.Count; i++)
            {
                GlobalUI child = parent.childrens.container[i];
                if (child != null)
                {
                    if (((child.bgSize.X + horizontalCount) + padding) <= maxHorizontalCount)
                    {
                        if (Xcount == 0)
                        {
                            child.position = new Vector2(horizontalCount + inSetPadding, verticalCount );
                            horizontalCount += child.bgSize.X + inSetPadding;
                        }
                        else
                        {
                            child.position = new Vector2(horizontalCount, verticalCount);
                            horizontalCount += (child.bgSize.X + padding);
                        }
                        Xcount++;
                    }
                    else
                    {
                        horizontalCount = minHorizontalCount;
                        verticalCount += (maxYSize + padding);
                        Xcount = 0;
                        i--;
                    }

                }
            }
        }
        private void Vertical()
        {
            short Xcount = 0;
            short Ycount = 0;
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
                            child.position = new Vector2(horizontalCount, inSetPadding);
                            verticalCount += child.bgSize.Y+ inSetPadding;
                        }
                        else
                        {
                            child.position = new Vector2(horizontalCount, verticalCount);
                            verticalCount += (child.bgSize.Y + padding);
                        }
                        Ycount++;
                    }
                    else
                    {
                        verticalCount = minVerticalCount;
                        horizontalCount += (maxXSize + padding);
                        Xcount++;
                        Ycount = 0;
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
                setDefaulMAXSIZE(flexWrap);
                childrensInfos = GetAllChildrenSize();
                Wrap(flexWrap);
                horizontalCount = minHorizontalCount;
                verticalCount = minVerticalCount;
                (maxXSize, maxYSize) = getSizeYXMax();

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
