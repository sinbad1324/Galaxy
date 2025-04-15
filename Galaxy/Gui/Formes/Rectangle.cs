using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Galaxy.modules;
using Galaxy.modules.Maths;


namespace LearnMatrix
{
    public struct BorderData
    {
        public int segments;
        public float Y;
        public float X;
        public float x;
        public float y;
        public Color color;
        public Vector2 arc;
    }
    public class Borders
    {

        public event EventHandler<EventArgs> changed;
        private void LuncheEvent()
        {
            if (changed != null) changed(this, EventArgs.Empty);
        }
        private float _raduis = 0;
        private int raduisController = 2;
        private Color _color = Color.White;
        private int _segements ;
        private Vector2 _size;
        private Vector2 _center;
        //Colors
        private Color _borderLColor = Color.White;
        private Color _borderRColor = Color.White;
        private Color _borderTColor = Color.White;
        private Color _borderBColor = Color.White;
        //Conotrolers
        private int _leftTopController = 2;
        private int _leftBottomController = 2;
        private int _rightTopController = 2;
        private int _rightBottomController = 2;
        //Raduis
        private float _leftTop;
        private float _leftBottom;
        private float _rightTop;
        private float _rightBottom;
        //Constructor
        private Dictionary<string, float> SommetData;
        //Methods
        public Borders(Vector2 center, Vector2 size)
        {
            this.center = center;
            this.size = size;
            SommetData = Utils.GetSegements(center, size);
            segements = 5;
        }
        public BorderData DefineLTData()
        {
            int _segements = segements;
            if (leftBottom <= 0) _segements = 0;
            Vector2 arc = new Vector2(SommetData["left"], SommetData["top"]);
            return new BorderData
            {
                segments = _segements,
                Y = Math.Clamp(arc.Y + rightTop, arc.Y, center.Y),
                X = Math.Clamp(arc.X + leftTop, arc.X, center.X),
                x = Math.Clamp(arc.X + leftTop / _leftTopController, arc.X, center.X),
                y = Math.Clamp(arc.Y + rightTop / _rightTopController, arc.Y, center.Y),
                arc = arc,
                color = _borderTColor,
            };
        }
        public BorderData DefineRTData()
        {
            int _segements = segements;
            if (leftBottom == 0) _segements = 0;
            Vector2 arc = new Vector2(SommetData["right"], SommetData["top"]);
            return new BorderData
            {
                segments = _segements,
                Y = Math.Clamp(arc.Y + rightTop ,arc.Y, center.Y),
                X = Math.Clamp(arc.X - rightTop, center.X, arc.X),
                x = Math.Clamp(arc.X - rightTop / _rightTopController, center.X, arc.X),
                y = Math.Clamp(arc.Y + rightTop / _rightTopController, arc.Y, center.Y),
                arc = arc,
                color = _borderRColor,
            };
        }

        public BorderData DefineRBData()
        {
            int _segements = segements;
            if (leftBottom == 0) _segements = 0;
            Vector2 arc = new Vector2(SommetData["right"], SommetData["bottom"]);
            return new BorderData
            {
                segments = _segements,
                Y = Math.Clamp(arc.Y - rightBottom, center.Y, arc.Y),
                X = Math.Clamp(arc.X - rightBottom, center.X, arc.X),
                x = Math.Clamp(arc.X - rightBottom / _rightBottomController, center.X, arc.X),
                y = Math.Clamp(arc.Y - rightBottom / _leftBottomController, center.Y, arc.Y),
                arc = arc,
                color = _borderRColor,
            };
        }

        public BorderData DefineLBData()
        {
            int _segements = segements;
            if (leftBottom == 0) _segements = 0;
            Vector2 arc = new Vector2(SommetData["left"], SommetData["bottom"]);
            return new BorderData
            {
                segments = _segements,
                Y = Math.Clamp(arc.Y - leftBottom, center.Y, arc.Y),
                X = Math.Clamp(arc.X + leftBottom, arc.X, center.X),
                x = Math.Clamp(arc.X + leftBottom / _leftBottomController, arc.X, center.X),
                y = Math.Clamp(arc.Y - leftBottom / _leftBottomController, center.Y, arc.Y),
                arc = arc,
                color = _borderRColor,
            };
        }
        //Publics 
        public Vector2 size { get { return _size; }  set { _size = value; SommetData = Utils.GetSegements(_center,value); } }
        public Vector2 center { get { return _center; } set { _center = value; SommetData = Utils.GetSegements(value, _size); } }
        //Segements
        public int segements { get { return _segements; } set { _segements = Math.Clamp(value , 3 , 50); LuncheEvent(); } }
        //Raduis
        public float Radius { get { return _raduis; } set { _raduis = value;
                _leftTop = value;
                _leftBottom = value;
                _rightTop = value;
                _rightBottom = value;
                LuncheEvent(); 
            } 
        }
        public float leftTop { get { return _leftTop; } set { _leftTop = value; LuncheEvent(); } }
        public float leftBottom { get { return _leftBottom; } set { _leftBottom = value; LuncheEvent(); } }
        public float rightTop { get { return _rightTop; } set { _rightTop = value; LuncheEvent(); } }
        public float rightBottom { get { return _rightBottom; } set { _rightBottom = value; LuncheEvent(); } }
        //Controlers
        public int RadiusController { get { return raduisController; } set { raduisController = value;
                _leftTopController = value;
                _leftBottomController = value;
                _rightTopController = value;
                _rightBottomController = value;
                LuncheEvent(); 
            } 
        }
        public int leftTopController { get { return _leftTopController; } set { _leftTopController = value; LuncheEvent(); } }
        public int leftBottomController { get { return _leftBottomController; } set { _leftBottomController = value; LuncheEvent(); } }
        public int rightTopController { get { return _rightTopController; } set { _rightTopController = value; LuncheEvent(); } }
        public int rightBottomController { get { return _rightBottomController; } set { _rightBottomController = value; LuncheEvent(); } }
        //Colors
        public Color borderColor { get { return _color; } set {
                _color = value;
                _borderLColor = value;
                _borderRColor = value;
                _borderTColor = value;
                _borderBColor = value;
                LuncheEvent(); 
            } 
        }
        public Color borderLColor { get { return _borderLColor; } set { _borderLColor = value; LuncheEvent(); } }
        public Color borderRColor { get { return _borderRColor; } set { _borderRColor = value; LuncheEvent(); } }
        public Color borderTColor { get { return _borderTColor; } set { _borderTColor = value; LuncheEvent(); } }
        public Color borderBColor { get { return _borderBColor; } set { _borderBColor = value; LuncheEvent(); } }
    }
  public  class RoundedRectangle
    {
        public bool overflow;
        public BasicEffect basicEffect;
        private Vector2 _origine;
        private Vector2 _size;
        private Color _color = Color.White;
        private Texture2D _texture;
        private float _rotation = 0;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private Vector2 _position = Vector2.Zero;
        private List<short> _indices;
        private List<VertexPositionColorTexture> _vertice;
        private Vector2 _scale = Vector2.One;
        private bool OrigineIsModified = false;
        private bool ClassLoaded = false;
        private bool CanBeModified = false;
        //Publics
        public RasterizerState rasterizerState;
        public const int MaxVertice = 50;
        public const int MaxIndices = (MaxVertice*3)-3;
        public Rectangle scissorRectangle;
        public Texture2D texture { get { return _texture; } set { if (value == null) return;  _texture = value;  basicEffect.Texture = value; } }
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public int Left => X;
        public int Right => X + Width;
        public int Top => Y;
        public int Bottom => Y + Height;
        public float rotation { get { return _rotation; } set { _rotation = value;  } }
        public Vector2 position { get { return _position; } set { 
                _position = value;
                X = (int)value.X;
                Y = (int)value.Y;
                SetBasicMatrix();
                UpdateScissorRectangle();
                UpdateRectangle();
            }
        }
        public Vector2 origine { get { return _origine; } set { 
                _origine = value;
                OrigineIsModified = true;
                UpdateRectangle();
            } }
        public Vector2 scale { get { return _scale; } set { 
                _scale = value;
                SetBasicMatrix();
                UpdateScissorRectangle();
         } }
        public Color color { get { return _color; } set { _color = value;  }  }
        public Vector2 size { get { return _size; } set {
                _size = value;
                if (OrigineIsModified == false)
                {
                    _origine = value / 2;
                    OrigineIsModified = false;
                }
                if (borderRaduis != null)
                {
                    borderRaduis.size = value;
                    borderRaduis.center = origine;
                }
                Width = (int)value.X;
                Height = (int)value.Y;
                UpdateScissorRectangle();
                UpdateRectangle();
            }
        }
        public Borders borderRaduis;
        public Matrix world = Matrix.Identity;
        public Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, .01f), Vector3.Zero, Vector3.Up);
        public Matrix projection = Matrix.CreateOrthographicOffCenter(0, GlobalParams.WINDOW_WIDHT, GlobalParams.WINDOW_HEIGTH, 0, 0.001f, 1f);
        // Constructor
        public RoundedRectangle(Vector2 position, Vector2 size )
        {
            CanBeModified = true;
            this._size = size;
            this._origine = size / 2;
          //borderRaduis.leftBottom = 20;
            this.position = position;
            this.color = Color.White;
            borderRaduis = new Borders(origine, size) { Radius = 0, borderColor = color , RadiusController = 2 };
        }
        //Destructor
        public void SetNewTexture(Texture2D texture)
        {
            this.texture = texture;
            if (basicEffect != null ) 
                basicEffect.Texture = texture;
        }
        public void SetNewTextureColor(Color clr)
        {
            if (texture == null)
                texture = new Texture2D(GlobalParams.Device, 1, 1);
            texture.SetData(new Color[] { clr });
            if (basicEffect != null)
                basicEffect.Texture = texture;
        }
        public void Destroy()
        {
            if (_vertice != null)
                _vertice.Clear();
            if (_indices != null)
                _indices.Clear();
            vertexBuffer?.Dispose();
            indexBuffer?.Dispose();
            basicEffect?.Dispose();
            basicEffect =null;
            borderRaduis = null;
            texture = null;
            

            ClassLoaded = false;
        }
        //Privates methodes
        public void Create2DRectangleRounded()
        {
            if (!ClassLoaded || GlobalParams.Device == null || !CanBeModified) return;
            CanBeModified = false;
            _indices?.Clear();
            _vertice?.Clear();
            vertexBuffer?.Dispose();
            indexBuffer?.Dispose();
            _indices = new List<short>();
            _vertice = new List<VertexPositionColorTexture>();
            var sommetData = Utils.GetSegements(origine, size);
            _vertice.Add(new VertexPositionColorTexture(new Vector3(origine, 0), color, new Vector2(0.5f, 0.5f)));
            _vertice.Add(new VertexPositionColorTexture(new Vector3(sommetData["left"], sommetData["top"], 0), color, new Vector2(0, 0)));
            _vertice.Add(new VertexPositionColorTexture(new Vector3(sommetData["right"], sommetData["top"], 0), color, new Vector2(1, 0)));
            _vertice.Add(new VertexPositionColorTexture(new Vector3(sommetData["left"], sommetData["bottom"], 0), color, new Vector2(0, 1)));
            _vertice.Add(new VertexPositionColorTexture(new Vector3(sommetData["right"], sommetData["bottom"], 0), color, new Vector2(1, 1)));
            _indices.AddRange(new short[] { 0, 2, 1, 0, 1, 3, 0, 2, 4, 0, 3, 4 });
            CreateBorderRound(borderRaduis.DefineLBData(), 3, 10); // left bottom
            CreateBorderRound(borderRaduis.DefineLTData(), 1, 2);  // left top
            CreateBorderRound(borderRaduis.DefineRTData(), 2, 1);  // right top
            CreateBorderRound(borderRaduis.DefineRBData(), 4, 11); // right bottom
            vertexBuffer = new VertexBuffer(GlobalParams.Device, typeof(VertexPositionColorTexture), _vertice.Count, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(GlobalParams.Device, typeof(short), _indices.Count, BufferUsage.WriteOnly);

            vertexBuffer.SetData(_vertice.ToArray(), 0, _vertice.Count);
            indexBuffer.SetData(_indices.ToArray(), 0, _indices.Count);
        }
        private void CreateBorderRound(BorderData data, short theSegement, short indicePrinc)
        {
            if (data.segments < 3) return;
            for (int i = 0; i <= data.segments; i++)
            {
                float t = (float)i / data.segments;
                Vector2 vector = Formules.CubicBezier(
                    t,
                    new Vector2(data.arc.X, data.Y),
                    new Vector2(data.arc.X, data.y),
                    new Vector2(data.x, data.arc.Y),
                    new Vector2(data.X, data.arc.Y));

                //ChatGPT
                 float texWidth = texture.Width;
                float texHeight = texture.Height;
                float imgRatio = texWidth / texHeight;
                float rectRatio = size.X / size.Y;
                Vector2 centerUV = new Vector2(0.5f, 0.5f);
                Vector2 uvOffset = new Vector2((vector.X - origine.X) / size.X, (vector.Y - origine.Y) / size.Y);
                if (imgRatio > rectRatio) uvOffset.X *= rectRatio / imgRatio;
                else uvOffset.Y *= imgRatio / rectRatio;
                Vector2 uv = centerUV + uvOffset;
                //ChatGPT

                if (i > 0)
                {
                    _vertice.Add(new VertexPositionColorTexture(
                        new Vector3(vector.X, vector.Y, 0),
                        data.color, uv)
                    );
                    short c = (short)(_vertice.Count - 1);
                    _indices.Add(0);
                    _indices.Add(c);
                    if (i == data.segments)
                    {
                        _indices[indicePrinc] = c;
                        _indices.Add((short)(_vertice.Count - 2));
                    }
                    else
                    {
                        if (i == 1)
                            _indices.Add(theSegement);
                        else if (i < data.segments)
                            _indices.Add((short)(_vertice.Count - 2));
                    }
                }
                else
                    _vertice[theSegement] = new VertexPositionColorTexture(new Vector3(vector.X, vector.Y, 0), data.color, uv);
            }
        }
        private void updateEffect()
        {
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = true;
        }
        private void UpdateVertex()
        {
            GlobalParams.Device.SetVertexBuffer(vertexBuffer);
            GlobalParams.Device.Indices = indexBuffer;
        }
        private void setRasterizerState()
        {

            rasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None,
                FillMode = GlobalParams.globalFillMode,
                
                MultiSampleAntiAlias = true,
                ScissorTestEnable = overflow
            };
        }
        private void SetBasicMatrix()
        {
            world = Matrix.CreateTranslation(new Vector3(_position, 0)) * Matrix.CreateRotationZ(MathHelper.ToRadians(_rotation)) * Matrix.CreateScale(new Vector3(_scale, 1));
        }
        private void UpdateScissorRectangle()
        {
            scissorRectangle = new Rectangle((int)(position.X * scale.X), (int)(position.Y * scale.Y) , (int)(size.X * scale.X), (int)(size.Y * scale.Y));
        }
        private void UpdateRectangle()
        {
            CanBeModified = true;
        }
       
        //Public methodes
        public void Initialize()
        {
            Create2DRectangleRounded();
            overflow = true;
            borderRaduis.changed += (sender, e) => UpdateRectangle();
            setRasterizerState();
            scissorRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }
        public void LoadContent()
        {
            ClassLoaded = true;
            basicEffect = new BasicEffect(GlobalParams.Device);
            basicEffect.TextureEnabled = true;
            basicEffect.VertexColorEnabled = true;
            basicEffect.FogEnabled = true;
        }
        public void Update()
        {
        }
        public void Draw()
        {
            if (basicEffect == null) return;
            Create2DRectangleRounded();
            setRasterizerState();
            GlobalParams.Device.RasterizerState = rasterizerState;
            GlobalParams.Device.ScissorRectangle = scissorRectangle;
            UpdateVertex();
            updateEffect();
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GlobalParams.Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertice.Count, 0, _indices.Count / 3);
            }
            GlobalParams.Device.SetVertexBuffer(null);
            GlobalParams.Device.Indices = null;
            rasterizerState?.Dispose();

        }
        public bool Contains(int x, int y)
        {
            if (X <= x && x < X + Width && Y <= y)
            {
                return y < Y + Height;
            }

            return false;
        }
        public bool Contains(float x, float y)
        {
            if ((float)X <= x && x < (float)(X + Width) && (float)Y <= y)
            {
                return y < (float)(Y + Height);
            }

            return false;
        }
        public bool Contains(Point value)
        {
            if (X <= value.X && value.X < X + Width && Y <= value.Y)
            {
                return value.Y < Y + Height;
            }

            return false;
        }
        public void Contains(ref Point value, out bool result)
        {
            result = X <= value.X && value.X < X + Width && Y <= value.Y && value.Y < Y + Height;
        }
        public bool Contains(Vector2 value)
        {
            if ((float)X <= value.X && value.X < (float)(X + Width) && (float)Y <= value.Y)
            {
                return value.Y < (float)(Y + Height);
            }

            return false;
        }
        public void Contains(ref Vector2 value, out bool result)
        {
            result = (float)X <= value.X && value.X < (float)(X + Width) && (float)Y <= value.Y && value.Y < (float)(Y + Height);
        }
        public bool Contains(RoundedRectangle value)
        {
            if (X <= value.X && value.X + value.Width <= X + Width && Y <= value.Y)
            {
                return value.Y + value.Height <= Y + Height;
            }

            return false;
        }
        public void Contains(ref RoundedRectangle value, out bool result)
        {
            result = X <= value.X && value.X + value.Width <= X + Width && Y <= value.Y && value.Y + value.Height <= Y + Height;
        }
        public override int GetHashCode()
        {
            return (((17 * 23 + X.GetHashCode()) * 23 + Y.GetHashCode()) * 23 + Width.GetHashCode()) * 23 + Height.GetHashCode();
        }
        public void Inflate(int horizontalAmount, int verticalAmount)
        {
            X -= horizontalAmount;
            Y -= verticalAmount;
            Width += horizontalAmount * 2;
            Height += verticalAmount * 2;
        }
        public void Inflate(float horizontalAmount, float verticalAmount)
        {
            X -= (int)horizontalAmount;
            Y -= (int)verticalAmount;
            Width += (int)horizontalAmount * 2;
            Height += (int)verticalAmount * 2;
        }
        public bool Intersects(RoundedRectangle value)
        {
            if (value.Left < Right && Left < value.Right && value.Top < Bottom)
            {
                return Top < value.Bottom;
            }

            return false;
        }
        public void Intersects(ref RoundedRectangle value, out bool result)
        {
            result = value.Left < Right && Left < value.Right && value.Top < Bottom && Top < value.Bottom;
        }
    }
}
