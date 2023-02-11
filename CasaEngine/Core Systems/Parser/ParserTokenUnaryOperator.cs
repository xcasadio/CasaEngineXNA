﻿namespace CasaEngine.Design.Parser
{
    class ParserTokenUnaryOperator
        : ParserToken
    {





        public ParserTokenUnaryOperator(Parser parser, string token)
            : base(parser, token)
        {

        }



        public override bool Check(string sentence)
        {
            if (sentence.StartsWith(Token) == true)
            {
                Parser.Check(Token.Substring(1));
            }

            return false;
        }

    }
}
