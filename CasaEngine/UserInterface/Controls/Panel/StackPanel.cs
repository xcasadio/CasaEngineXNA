﻿
/*

 Based in the project Neoforce Controls (http://neoforce.codeplex.com/)
 GNU Library General Public License (LGPL)

-----------------------------------------------------------------------------------------------------------------------------------------------
Modified by: Schneider, José Ignacio (jis@cs.uns.edu.ar)
-----------------------------------------------------------------------------------------------------------------------------------------------

*/

namespace XNAFinalEngine.UserInterface
{
    public class StackPanel : Container
    {

        private Orientation _orientation;



        public XNAFinalEngine.UserInterface.Orientation Orientation
        {
            get => _orientation;
            set => _orientation = value;
        }



        public StackPanel(UserInterfaceManager userInterfaceManager)
            : base(userInterfaceManager)
        {
            this._orientation = Orientation.Horizontal;
            Color = Color.Transparent;
        } // StackPanel



        private void CalculateLayout()
        {
            int top = Top;
            int left = Left;

            foreach (Control c in ClientArea.ChildrenControls)
            {
                Margins m = c.DefaultDistanceFromAnotherControl;

                if (_orientation == Orientation.Vertical)
                {
                    top += m.Top;
                    c.Top = top;
                    top += c.Height;
                    top += m.Bottom;
                    c.Left = left;
                }

                if (_orientation == Orientation.Horizontal)
                {
                    left += m.Left;
                    c.Left = left;
                    left += c.Width;
                    left += m.Right;
                    c.Top = top;
                }
            }
        } // CalculateLayout



        protected override void OnResize(ResizeEventArgs e)
        {
            CalculateLayout();
            base.OnResize(e);
        } // OnResize


    } // StackPanel
} //  XNAFinalEngine.UserInterface