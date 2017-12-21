using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVParser
{
    public class CSVSyntaxError
    {
        public IToken Symbol;
        public int Line, Position;
        public string Message;

        public CSVSyntaxError(IToken offendingSymbol, int lineNumber, int charPositionInLine, string msg)
        {
            Symbol = offendingSymbol;
            Line = lineNumber;
            Position = charPositionInLine;
            Message = msg;
        }
    }
}
