using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVParser
{
    public class Cleaner
    {
        public string Clean (string Parametri)
        {
            var result = string.Empty;
            var parameters = Parametri.Split('§');

            if (parameters.Length == 0) return "-ER - È necessario specificare il path del file csv";
            
            //la funzione si aspetta in input il path del file seguito dal carattere § e il separatore del file CSV (di default è ;)
            string filename = Parametri.Split('§')[0];

            if (parameters.Length == 2)
            {
                //not implemented - per il momento utilizza solo il punto e virgola
                string separator = Parametri.Split('§')[1];
                result = @"-WG - La funzionalità di scelta del carattere separatore non è implementata. 
    Il separatore di Default è il punto e virgola";
            }

            try
            {
                var file = File.ReadAllText(filename);
                
                AntlrInputStream inputStream = new AntlrInputStream(file);
                CSVLexer lex = new CSVLexer(inputStream);
                CommonTokenStream TokenStream = new CommonTokenStream(lex);
                CSVParser par = new CSVParser(TokenStream);

                CSVErrorListener el = new CSVErrorListener();
                par.RemoveErrorListeners();
                par.AddErrorListener(el);

                var csvContext = par.csvFile();

                if (el.SyntaxErrorsList.Count > 0)
                {
                    result += @"-ER - Nel file csv sono presenti righe con più colonne rispetto all'intestazione:
Righe Errate  ";
                    foreach(var se in el.SyntaxErrorsList)
                    {
                        result += se.Line + ", ";
                    }
                    result = result.Remove(result.Length - 2);
                    result += "\n";
                    return result;
                }

                CSVCleanerListener listener = new CSVCleanerListener(TokenStream);
                ParseTreeWalker walker = new ParseTreeWalker();
                walker.Walk(listener, csvContext);

                if (listener.shortRows)
                    result += @"-ER - Nel file csv sono presenti righe con meno colonne rispetto all'intestazione\n";

                if (listener.LogRows.Count != 0)
                {
                    result += @"-WG - Il file contiene righe vuote in fondo e/o separatori in eccesso a fine riga";
                    foreach (var row in listener.LogRows)
                    {
                        result += "\nRiga " + row.Key + " - " + row.Value;
                    }
                }

                //se non ci sono errori bloccanti sovrascrivo il file "pulito"
                if (!listener.shortRows && el.SyntaxErrorsList.Count == 0)
                    File.WriteAllText(filename, listener.r.GetText());
            }
            catch (IOException exIO)
            {
                return " - ER - Errore in lettura o scrittura del file csv\n    " + exIO.Message;
            }
            catch (Exception e)
            {
                return "-ER - Errore non gestito: " + e.Message;
            }

            if (result.Equals(string.Empty)) result = "+OK";

            return result;
        }
    }
}
