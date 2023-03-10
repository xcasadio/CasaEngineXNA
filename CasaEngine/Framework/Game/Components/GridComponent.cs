using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CasaEngine.Framework.Game.Components;

public class GridComponent : DrawableGameComponent
{
    private VertexPositionColor[] LinesVertices;
    private int m_Size = 50;
    private BasicEffect GridEffect;

    public GridComponent(Microsoft.Xna.Framework.Game game) : base(game)
    {
        game.Components.Add(this);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            lock (this)
            {
                Game.RemoveGameComponent<GridComponent>();
            }
        }

        base.Dispose(disposing);
    }

    protected override void LoadContent()
    {
        int nbVertices = m_Size * 8 + 4;
        GridEffect = new BasicEffect(GraphicsDevice);
        GridEffect.VertexColorEnabled = true;
        GridEffect.LightingEnabled = false;
        LinesVertices = new VertexPositionColor[nbVertices];
        Color color;
        int i = 0;

        for (int x = m_Size; x > 0; x--)
        {
            if (x % 10 == 0)
            {
                color = Color.DarkBlue;
            }
            else if (x % 5 == 0)
            {
                color = Color.DarkGray;
            }
            else
            {
                color = Color.DimGray;
            }

            LinesVertices[i++] = new VertexPositionColor(new Vector3(x, 0.0f, m_Size), color);
            LinesVertices[i++] = new VertexPositionColor(new Vector3(x, 0.0f, -m_Size), color);

            LinesVertices[i++] = new VertexPositionColor(new Vector3(-x, 0.0f, m_Size), color);
            LinesVertices[i++] = new VertexPositionColor(new Vector3(-x, 0.0f, -m_Size), color);

            LinesVertices[i++] = new VertexPositionColor(new Vector3(m_Size, 0.0f, x), color);
            LinesVertices[i++] = new VertexPositionColor(new Vector3(-m_Size, 0.0f, x), color);

            LinesVertices[i++] = new VertexPositionColor(new Vector3(m_Size, 0.0f, -x), color);
            LinesVertices[i++] = new VertexPositionColor(new Vector3(-m_Size, 0.0f, -x), color);
        }

        LinesVertices[i++] = new VertexPositionColor(new Vector3(-m_Size, 0.0f, 0), Color.DarkBlue);
        LinesVertices[i++] = new VertexPositionColor(new Vector3(m_Size, 0.0f, 0), Color.DarkBlue);
        LinesVertices[i++] = new VertexPositionColor(new Vector3(0, 0.0f, m_Size), Color.DarkBlue);
        LinesVertices[i++] = new VertexPositionColor(new Vector3(0, 0.0f, -m_Size), Color.DarkBlue);

        base.LoadContent();
    }

    public override void Draw(GameTime gameTime)
    {
        if (GameInfo.Instance.ActiveCamera == null)
        {
            return;
        }

        GridEffect.World = Matrix.Identity;
        GridEffect.View = GameInfo.Instance.ActiveCamera.ViewMatrix;
        GridEffect.Projection = GameInfo.Instance.ActiveCamera.ProjectionMatrix;
        GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        foreach (EffectPass pass in GridEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, LinesVertices, 0, LinesVertices.Length / 2);
        }

        base.Draw(gameTime);
    }
}