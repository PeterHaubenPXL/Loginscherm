namespace CreateSP
{
    public static class Common
    {
        private static string _foundFilePath;
        public static string foundFilePath
        {
            get
            {
                return _foundFilePath;
            }
            set
            {
                _foundFilePath = value;
            }
        }


        private static bool _StatusDatabaseGewijzigd;
        public static bool StatusDatabaseGewijzigd
        {
            get
            {
                return _StatusDatabaseGewijzigd;
            }
            set
            {
                _StatusDatabaseGewijzigd = value;
            }
        }


        private static int _VerrichtingID;
        public static int VerrichtingID
        {
            get 
            { 
                return _VerrichtingID; 
            }
            set 
            { 
                _VerrichtingID = value; 
            }
        }


        private static int _AantalRecords;
        public static int AantalRecords
        {
            get
            {
                return _AantalRecords;
            }
            set
            {
                _AantalRecords = value;
            }
        }
        

        private static string _strQuery;
        public static string strQuery
        {
            get
            {
                return _strQuery;
            }
            set
            {
                _strQuery = value;
            }
        }


        private static bool _StatusBeveiliging;
        public static bool StatusBeveiliging 
        {
            get
            {
                return _StatusBeveiliging;
            }
            set
            {
                _StatusBeveiliging = value;
            }
        }


        private static bool _boolchkNieuwPaswoord;
        public static bool boolchkNieuwPaswoord
        {
            get 
            { 
                return _boolchkNieuwPaswoord; 
            }
            set 
            { 
                _boolchkNieuwPaswoord = value; 
            }
        }


        private static string _InputInputBox;
        public static string InputInputBox
        {
            get
            {
                return _InputInputBox;
            }
            set
            {
                _InputInputBox = value;
            }
        }


        private static string _strSP;
        public static string strSP
        {
            get
            {
                return _strSP;
            }
            set
            {
                _strSP = value;
            }
        }


    }

}
