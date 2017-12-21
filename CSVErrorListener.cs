using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVParser
{
    public class CSVErrorListener : BaseErrorListener
    {
        public List<CSVSyntaxError> SyntaxErrorsList;

        public CSVErrorListener()
        {
            SyntaxErrorsList = new List<CSVSyntaxError>();
        }

        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            SyntaxErrorsList.Add(new CSVSyntaxError(offendingSymbol, line, charPositionInLine, msg));
        }
    }
}
