
/*
Copyright (c) 2008-2012, Laboratorio de Investigación y Desarrollo en Visualización y Computación Gráfica - 
                         Departamento de Ciencias e Ingeniería de la Computación - Universidad Nacional del Sur.
All rights reserved.
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above copyright, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer
    in the documentation and/or other materials provided with the distribution.

•	Neither the name of the Universidad Nacional del Sur nor the names of its contributors may be used to endorse or promote products derived
    from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS ''AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

-----------------------------------------------------------------------------------------------------------------------------------------------
Author: Schneider, José Ignacio (jis@cs.uns.edu.ar)
-----------------------------------------------------------------------------------------------------------------------------------------------

*/


using Microsoft.Xna.Framework.Input;


//using XNAFinalEngine.EngineCore;

namespace XNAFinalEngine.Input
{

    public static class Mouse
    {

        public enum MouseButtons
        {
            LeftButton,
            MiddleButton,
            RightButton,
            XButton1,
            XButton2,
        } // MouseButtons



        // Mouse state, set every frame in the Update method.
        private static MouseState currentState, previousState;

        // X and Y movements of the mouse in this frame.
        private static int deltaX, deltaY;

        // Current mouse position.
        private static int positionX, positionY;

        // Mouse wheel delta. XNA does report only the total scroll value, but we usually need the current delta!
        private static int wheelDelta, wheelValue;

        // Start dragging pos, will be set when we just pressed the left mouse button. Used for the MouseDraggingAmount property.
        private static Point startDraggingPosition;

        // This mode allows to track the mouse movement when the mouse reach and pass the system window border.
        private static bool trackDeltaOutsideScreen;



        public static MouseState State => currentState;

        public static MouseState PreviousState => previousState;


        /*public static bool TrackDeltaOutsideScreen
	    {
	        get { return trackDeltaOutsideScreen; }
	        set
	        {
	            trackDeltaOutsideScreen = value;
                if (value)
                {
                    Microsoft.Xna.Framework.Input.Mouse.SetPosition(Screen.Width / 2 - 1, Screen.Height / 2 - 1);
                    positionX = Screen.Width / 2 - 1;
                    positionY = Screen.Height / 2 - 1;
                }
	        }
	    } // TrackDeltaOutsideScreen

		public static Point Position
		{
			get
			{
                Point aux = new Point(positionX, positionY);
                if (aux.X >= Screen.Width)
                    aux.X = Screen.Width - 1;
                if (aux.X < 0)
                    aux.X = 0;
                if (aux.Y >= Screen.Height)
                    aux.Y = Screen.Height - 1;
                if (aux.Y < 0)
                    aux.Y = 0;
                return aux;
			}
			set
			{
                if (!TrackDeltaOutsideScreen)
                {
                    Microsoft.Xna.Framework.Input.Mouse.SetPosition(value.X, value.Y);
                }
                positionX = value.X;
                positionY = value.Y;
			}
        } // Position
        */
        public static float DeltaX => deltaX;

        public static float DeltaY => deltaY;


        public static bool LeftButtonPressed => currentState.LeftButton == ButtonState.Pressed;

        public static bool RightButtonPressed => currentState.RightButton == ButtonState.Pressed;

        public static bool MiddleButtonPressed => currentState.MiddleButton == ButtonState.Pressed;

        public static bool XButton1Pressed => currentState.XButton1 == ButtonState.Pressed;

        public static bool XButton2Pressed => currentState.XButton2 == ButtonState.Pressed;

        public static bool LeftButtonJustPressed => currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released;

        public static bool RightButtonJustPressed => currentState.RightButton == ButtonState.Pressed && previousState.RightButton == ButtonState.Released;

        public static bool MiddleButtonJustPressed => currentState.MiddleButton == ButtonState.Pressed && previousState.MiddleButton == ButtonState.Released;

        public static bool XButton1JustPressed => currentState.XButton1 == ButtonState.Pressed && previousState.XButton1 == ButtonState.Released;

        public static bool XButton2JustPressed => currentState.XButton2 == ButtonState.Pressed && previousState.XButton2 == ButtonState.Released;

        public static bool LeftButtonJustReleased => currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed;

        public static bool RightButtonJustReleased => currentState.RightButton == ButtonState.Released && previousState.RightButton == ButtonState.Pressed;

        public static bool MiddleButtonJustReleased => currentState.MiddleButton == ButtonState.Released && previousState.MiddleButton == ButtonState.Pressed;

        public static bool XButton1JustReleased => currentState.XButton1 == ButtonState.Released && previousState.XButton1 == ButtonState.Pressed;

        public static bool XButton2JustReleased => currentState.XButton2 == ButtonState.Released && previousState.XButton2 == ButtonState.Pressed;


        //public static Point DraggingAmount { get { return new Point(-startDraggingPosition.X + Position.X, -startDraggingPosition.Y + Position.Y); } }

        /*public static Rectangle DraggingRectangle
	    {
	        get
	        {
	            int x, y, width, height;
                if (startDraggingPosition.X <= Position.X)
                {
                    x = startDraggingPosition.X;
                    width = Position.X - startDraggingPosition.X;
                }
                else
                {
                    x = Position.X;
                    width = startDraggingPosition.X - Position.X;
                }
                if (startDraggingPosition.Y <= Position.Y)
                {
                    y = startDraggingPosition.Y;
                    height = Position.Y - startDraggingPosition.Y;
                }
                else
                {
                    y = Position.Y;
                    height = startDraggingPosition.Y - Position.Y;
                }
                return new Rectangle(x, y, width, height);
	        }
	    } // DraggingRectangle
        */
        //public static bool IsDragging { get { return Math.Abs(Position.X - startDraggingPosition.X) + Math.Abs(Position.Y - startDraggingPosition.Y) == 0; } }



        public static int WheelDelta => wheelDelta;

        public static int WheelValue => wheelValue;


        /*public static void ResetDragging()
		{
			startDraggingPosition = Position;
		} // ResetDragging
        */


        public static bool MouseInsideRectangle(Rectangle rectangle)
        {
            return positionX >= rectangle.X &&
                   positionY >= rectangle.Y &&
                   positionX < rectangle.Right &&
                   positionY < rectangle.Bottom;
        } // MouseInsideRectangle



        public static bool ButtonJustPressed(MouseButtons button)
        {
            if (button == MouseButtons.LeftButton)
                return LeftButtonJustPressed;
            if (button == MouseButtons.MiddleButton)
                return MiddleButtonJustPressed;
            if (button == MouseButtons.RightButton)
                return RightButtonJustPressed;
            if (button == MouseButtons.XButton1)
                return XButton1JustPressed;
            return XButton2JustPressed;
        } // ButtonJustPressed

        public static bool ButtonPressed(MouseButtons button)
        {
            if (button == MouseButtons.LeftButton)
                return LeftButtonPressed;
            if (button == MouseButtons.MiddleButton)
                return MiddleButtonPressed;
            if (button == MouseButtons.RightButton)
                return RightButtonPressed;
            if (button == MouseButtons.XButton1)
                return XButton1Pressed;
            return XButton2Pressed;
        } // ButtonPressed



        internal static void Update()
        {
            // Update mouse state.
            previousState = currentState;
            currentState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            //if (!TrackDeltaOutsideScreen)
            {
                // Calculate mouse movement.
                deltaX = currentState.X - positionX; // positionX is the old position.
                deltaY = currentState.Y - positionY;
                // Update position.
                positionX = currentState.X; // Now is the new one.
                positionY = currentState.Y;
            }
            /*else
            {
                deltaX = currentState.X - Screen.Width / 2;
                deltaY = currentState.Y - Screen.Height / 2;
                positionX += deltaX;
                positionY += deltaY;
                if (positionX >= Screen.Width)
                    positionX = Screen.Width - 1;
                if (positionX < 0)
                    positionX = 0;
                if (positionY >= Screen.Height)
                    positionY = Screen.Height - 1;
                if (positionY < 0)
                    positionY = 0;
                Microsoft.Xna.Framework.Input.Mouse.SetPosition(Screen.Width / 2 - 1, Screen.Height / 2 - 1);
            }*/

            // Dragging
            /*if (LeftButtonJustPressed || (!LeftButtonPressed && !LeftButtonJustReleased))
            {
                startDraggingPosition = Position;
            }*/

            // Wheel
            wheelDelta = currentState.ScrollWheelValue - wheelValue;
            wheelValue = currentState.ScrollWheelValue;
        } // Update

    } // Mouse
} // XNAFinalEngine.Input
