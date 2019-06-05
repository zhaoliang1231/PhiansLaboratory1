using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.test
{
    public class WordDocumentTable
    {
        public WordDocumentTable(int PiTableID)
        {
            MiTableID = PiTableID;
        }

        public WordDocumentTable(int PiTableID, int PiColumnID)
        {
            MiTableID = PiTableID;
            MiColumnID = PiColumnID;
        }

        public WordDocumentTable(int PiTableID, int PiColumnID, int PiRowID)
        {
            MiTableID = PiTableID;
            MiColumnID = PiColumnID;
            MiRowID = PiRowID;
        }

        private int MiTableID = 0;

        public int TableID
        {
            get { return MiTableID; }
            set { MiTableID = value; }
        }

        private int MiRowID = 0;
        public int RowID
        {
            get { return MiRowID; }
            set { MiRowID = value; }
        }

        private int MiColumnID = 0;
        public int ColumnID
        {
            get { return MiColumnID; }
            set { MiColumnID = value; }
        }

    }


}