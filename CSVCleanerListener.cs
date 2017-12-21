using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace CSVParser
{
    public class CSVCleanerListener : CSVBaseListener
    {
        public TokenStreamRewriter r;
        public List<KeyValuePair<int, string>> LogRows;
        public bool shortRows;

        private int index;

        public CSVCleanerListener(ITokenStream tokens)
        {
            r = new TokenStreamRewriter(tokens);
            index = 0;
            LogRows = new List<KeyValuePair<int, string>>();
            shortRows = false;
        }

        public override void ExitRow([NotNull] CSVParser.RowContext context)
        {
            //skip header
            if (context.col == 0)
                return;

            index++;

            var separator = context.children.Count(x => x.GetText().Equals(";"));

            //riga vuota
            if (context.fields == 0)
            {
                LogRows.Add(new KeyValuePair<int, string>(index, "Riga vuota"));
                r.Delete(context.start, context.stop);
            }
            //separatori in eccesso a fine riga
            else if (separator >= context.fields)
            {
                LogRows.Add(new KeyValuePair<int, string>(index, "Separatori in eccesso alla fine della riga"));

                //cancello i separatori in eccesso alla fine della riga
                for (int i = context.fields; i <= separator; i++)
                {
                    r.Delete(context.SEP(i-1).Symbol);
                }
            }
            else if (separator < context.col - 1)
            {
                shortRows = true;
                LogRows.Add(new KeyValuePair<int, string>(index, "Riga troppo corta"));
            }
        }
    }
}
