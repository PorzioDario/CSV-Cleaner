# CSV Cleaner

Questa DLL c# permette di leggere un file CSV e rimuovere eventuali righe vuote alla fine del file ed eventuali celle vuote in fondo alla riga in base alle colonne specificate nell'intestazione.

Il formato predefinito per il file csv prevede il punto e virgola ';' come carattere separatore e accetta sia il terminatore di riga linux (LF) sia quello di windows (CRLF).

Per utilizzare la DLL è sufficiente istanziare un oggetto di tipo CSVParser.Cleaner e chiamare il metodo Clean passando come parametro il path del file.

Il valore di ritorno del metodo è di tipo stringa e contiene i seguenti codici (nei primi 3 caratteri) seguiti eventualmente da messaggi di errore/warning:

|Codice |Messaggi possibili     |
|-------|-----------------------|
|+OK    |Nessun Messaggio       |
|-ER    |Nel file csv sono presenti righe con più colonne rispetto all'intestazione |
|-ER    |Nel file csv sono presenti righe con meno colonne rispetto all'intestazione|
|-WG    |Il file contiene righe vuote in fondo e/o separatori in eccesso a fine riga|
