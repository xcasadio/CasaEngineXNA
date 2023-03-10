/*

 Based in the project Neoforce Controls (http://neoforce.codeplex.com/)
 GNU Library General Public License (LGPL)

-----------------------------------------------------------------------------------------------------------------------------------------------
Modified by: Schneider, Jos? Ignacio (jis@cs.uns.edu.ar)
-----------------------------------------------------------------------------------------------------------------------------------------------

*/

namespace CasaEngine.Framework.UserInterface
{
    internal static class Utilities
    {

        public static string ControlTypeName(Control control)
        {
            var str = control.ToString();
            var i = str.LastIndexOf(".");
            return str.Remove(0, i + 1);
        } // ControlTypeName

        public static Color ParseColor(string str)
        {
            var val = str.Split(';');
            byte r = 255, g = 255, b = 255, a = 255;

            if (val.Length >= 1)
            {
                r = byte.Parse(val[0]);
            }

            if (val.Length >= 2)
            {
                g = byte.Parse(val[1]);
            }

            if (val.Length >= 3)
            {
                b = byte.Parse(val[2]);
            }

            if (val.Length >= 4)
            {
                a = byte.Parse(val[3]);
            }

            return Color.FromNonPremultiplied(r, g, b, a);
        } // ParseColor

    } // Utilities
} //  XNAFinalEngine.UserInterface
