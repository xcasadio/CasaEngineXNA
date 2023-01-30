namespace CasaEngine.AI.Messaging
{
    public class MessageComparer : IComparer<Message>
    {

        protected internal double precision;



        public MessageComparer(double precision)
        {
            String message = String.Empty;

            if (ValidatePrecision(precision, ref message) == false)
                throw new AIException("precision", this.GetType().ToString(), message);

            this.precision = precision;
        }



        public static bool ValidatePrecision(double precision, ref string message)
        {
            if (precision < 0)
            {
                message = "The precision value must be equal or greater than 0";
                return false;
            }

            return true;
        }



        public int Compare(Message x, Message y)
        {
            //Note to myself: it�s really a good idea to compare the extra info field? Maybe will let pass nearly similar messages...
            if ((x.SenderID == y.SenderID) && (x.RecieverID == y.RecieverID) && (x.Type == y.Type) && (x.ExtraInfo == y.ExtraInfo) && (System.Math.Abs(x.DispatchTime - y.DispatchTime) < precision))
                return 0;

            if (x.DispatchTime >= y.DispatchTime)
                return 1;

            else
                return -1;
        }

    }
}
