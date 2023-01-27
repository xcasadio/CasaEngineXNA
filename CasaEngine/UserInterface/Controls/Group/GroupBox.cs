
#region License
/*

 Based in the project Neoforce Controls (http://neoforce.codeplex.com/)
 GNU Library General Public License (LGPL)

-----------------------------------------------------------------------------------------------------------------------------------------------
Modified by: Schneider, Jos� Ignacio (jis@cs.uns.edu.ar)
-----------------------------------------------------------------------------------------------------------------------------------------------

*/
#endregion

#region Using directives
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CasaEngine.Asset.Fonts;
#endregion

namespace XNAFinalEngine.UserInterface
{

    public enum GroupBoxType
    {
        Normal,
        Flat
    } // GroupBoxType

    /// <summary>
    /// Group Box. Group controls that will be enclosed by a bevel and it will be a title in the top bevel line.
    /// </summary>
    public class GroupBox : Container
    {

        #region Variables

        /// <summary>
        /// Group Box Type (Normal, Flat).
        /// </summary>
        private GroupBoxType type = GroupBoxType.Normal;

        #endregion

        #region Properties

        /// <summary>
        /// Group Box Type (Normal, Flat).
        /// </summary>
        public virtual GroupBoxType Type
        {
            get { return type; }
            set { type = value; Invalidate(); }
        } // Type

        #endregion

        #region Constructor

        /// <summary>
        /// Group Box. Group controls with and title in a line.
        /// </summary>
        public GroupBox(UserInterfaceManager userInterfaceManager_)
            : base(userInterfaceManager_)
        {
            CheckLayer(SkinInformation, "Control");
            CheckLayer(SkinInformation, "Flat");

            CanFocus = false;
            Passive = true;
            Width = 64;
            Height = 64;
            BackgroundColor = Color.Transparent;
        } // GroupBox

        #endregion

        #region Draw

        /// <summary>
        /// Prerender the control into the control's render target.
        /// </summary>
        protected override void DrawControl(Rectangle rect)
        {
            SkinLayer layer = type == GroupBoxType.Normal ? SkinInformation.Layers["Control"] : SkinInformation.Layers["Flat"];
            Font font = layer.Text.Font.Font;
            Point offset = new Point(layer.Text.OffsetX, layer.Text.OffsetY);
            Vector2 size = font.MeasureString(Text);
            size.Y = font.LineSpacing;
            Rectangle r = new Rectangle(rect.Left, rect.Top + (int)(size.Y / 2), rect.Width, rect.Height - (int)(size.Y / 2));

            UserInterfaceManager.Renderer.DrawLayer(this, layer, r);

            if (!string.IsNullOrEmpty(Text))
            {
                Rectangle bg = new Rectangle(r.Left + offset.X, (r.Top - (int)(size.Y / 2)) + offset.Y, (int)size.X + layer.ContentMargins.Horizontal, (int)size.Y);
                UserInterfaceManager.Renderer.DrawLayer(UserInterfaceManager.Skin.Controls["Control"].Layers[0], bg, new Color(64, 64, 64), 0);
                UserInterfaceManager.Renderer.DrawString(this, layer, Text, new Rectangle(r.Left, r.Top - (int)(size.Y / 2), (int)(size.X), (int)size.Y), true, 0, 0, false);
            }
        } // DrawControl

        #endregion

    } // GroupBox
} // XNAFinalEngine.UserInterface
