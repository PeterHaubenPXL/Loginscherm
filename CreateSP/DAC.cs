using CreateSp;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CreateSP
{
    public static class DAC
    {
        private static string _MyConnectionString;

        public static string MyConnectionString
        {
            get
            {
                return _MyConnectionString;
            }

            // Set bijgezet zodat ik de constring programmatisch kan aanpassen
           
            set
            {
                _MyConnectionString = value;
            }
        }


        public static void OpslaanConString()
        {
            int x = 0;

            //Settings.Default.CNGast = MyConnectionString;
            //Settings.Default.Save();
        }


        public static DataTable ExecuteDataTable(string storedProcedureName)
        {
            DataTable dt;

            using (SqlConnection cnn = new SqlConnection(MyConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storedProcedureName;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                try
                {
                    dt = new DataTable();
                    da.Fill(dt);
                }
                catch (SqlException ex)
                {
                    DebugLogger.log(ex.Message);
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    DebugLogger.log(ex.Message);
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cnn.Close();
                }
            }
            return dt;
        }


        //Onderstaande bij  Insert Rek, Cat & SubCat bij NewStatus == true
        public static (DataTable dt, bool ok, string boodschap) ExecuteDataTable(string storedProcedureName,
            ref int nr, ref int id,
            params SqlParameter[] arrParam)
        {
            DataTable DT;
            bool IHaveAReturnValue = false;

            using (SqlConnection CN = new SqlConnection(MyConnectionString))
            {
                using (SqlCommand CMD = new SqlCommand(storedProcedureName, CN))
                {
                    CMD.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter DA = new SqlDataAdapter(CMD))
                    {
                        if (arrParam != null)
                        {
                            foreach (SqlParameter param in arrParam)
                            {
                                CMD.Parameters.Add(param);
                                if (param.ParameterName == "ReturnValue")
                                {
                                    IHaveAReturnValue = true;
                                    CMD.Parameters["ReturnValue"].Direction = ParameterDirection.Output;
                                }
                                if (param.ParameterName == "ID")
                                {
                                    CMD.Parameters["ID"].Direction = ParameterDirection.Output;
                                }
                            }
                        }
                        try
                        {
                            DT = new DataTable();
                            DA.Fill(DT);

                            if (IHaveAReturnValue)
                            {
                                nr = (int)CMD.Parameters["ReturnValue"].Value;
                                switch (nr)
                                {
                                    case 999:
                                        id = (int)CMD.Parameters["ID"].Value;
                                        return (DT, true, "De bewerking is gelukt.");
                                    case 0:
                                        DebugLogger.log("concurrency probleem");
                                        return (null, false, "Record is ondertussen door een andere bewerking aangepast, probeer het opnieuw.");
                                    case 547:
                                        DebugLogger.log("SQl error 547 : key constraint violation");
                                        if (storedProcedureName.Substring(0, 1) == "D")
                                        {
                                            return (null, false, "De bewerking is Niet gelukt, U probeert een record te verwijderen die aan een andere record gekoppeld is.  Verwijder eerst de gekoppelde record");
                                        }
                                        else
                                        {
                                            return (null, false, "De bewerking is niet gelukt, gelieve jouw systeembeheerder te contacteren.");
                                        }
                                    case 2627:
                                        DebugLogger.log("SQl error 2627 : unique constraint violation - record bestaat al");
                                        if (storedProcedureName.Substring(0, 1) == "I")
                                        {
                                            return (null, false, "De bewerking is Niet gelukt, U probeert een record toe te voegen die reeds bestaat");
                                        }
                                        else
                                        {
                                            return (null, false, "De bewerking is niet gelukt, gelieve jouw systeembeheerder te contacteren.");
                                        }
                                    default:
                                        DebugLogger.log("Stored Procedure : " + storedProcedureName + " uitgevoerd met error - Return Code : " + nr.ToString());
                                        return (null, false, "De bewerking is Niet gelukt, gelieve jouw systeembeheerder te contacteren. (SQL Foutcode : " + nr.ToString() + ")");
                                }
                            }
                            return (DT, true, "De bewerking is gelukt.");
                        }
                        catch (SqlException ex)
                        {
                            DebugLogger.log(ex.Message);
                            nr = (int)CMD.Parameters["ReturnValue"].Value;
                            switch (nr)
                            {
                                case 997:
                                    DebugLogger.log("SQl error 997 : unique constraint violation - record bestaat al");
                                    if (storedProcedureName.Substring(0, 2) == "U_")
                                    {
                                        return (null, false, "De bewerking is Niet gelukt, U probeert een record aan te passen naar een reeds bestaand item");
                                    }
                                    else if (storedProcedureName.Substring(0, 2) == "I_")
                                    {
                                        return (null, false, "De bewerking is Niet gelukt, U probeert een record toe te voegen dat reeds bestaat");
                                    }
                                    else
                                    {
                                        return (null, false, "De bewerking is niet gelukt, gelieve jouw systeembeheerder te contacteren.");
                                    }
                                default:
                                    DebugLogger.log("Stored Procedure : " + storedProcedureName + " uitgevoerd met error - Return Code : " + nr.ToString());
                                    return (null, false, "De bewerking is Niet gelukt, gelieve jouw systeembeheerder te contacteren. (SQL Foutcode : " + nr.ToString() + ")");
                            }

                            throw new Exception(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            DebugLogger.log(ex.Message);
                            return (null, false, "De bewerking is Niet gelukt, gelieve jouw systeembeheerder te contacteren.");
                            throw new Exception(ex.Message);
                        }
                        finally
                        {
                            CN.Close();
                        }
                    }
                }

            }
        }

        public static DataTable ExecuteDataTable(string storedProcedureName,
             ref int nr, params SqlParameter[] arrParam)
        {
            DataTable dt = new DataTable();
            string ControlValue = string.Empty;

            using (SqlConnection cnn = new SqlConnection(MyConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storedProcedureName;

                //HIER GA IK KIJKEN OF IK EEN PARAMETER MET DE NAAM @ReturnValue HEB, INDIEN DIT HET GEVAL IS DAN 
                //GA IK EEN ANDERS WAARDE OP OK ZETTEN ZODAT IK DEZE PARAMETER ALS OUTPUT KAN DEFINIEREN
                if (arrParam != null)
                {
                    foreach (SqlParameter param in arrParam)
                    {
                        cmd.Parameters.Add(param);
                        if (param.ParameterName == "ReturnValue")
                        {
                            ControlValue = "ok";
                            cmd.Parameters["ReturnValue"].Direction = ParameterDirection.Output;
                        }
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                int ReturnValue = 0;
                try
                {
                    dt = new DataTable();
                    da.Fill(dt);

                    if (ControlValue == "ok")
                    {
                        ReturnValue = (int)cmd.Parameters["ReturnValue"].Value;
                        nr = ReturnValue;
                    }
                }
                catch (SqlException ex)
                {
                    DebugLogger.log(ex.Message);
                    ReturnValue = 996;
                    nr = ReturnValue;
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    DebugLogger.log(ex.Message);
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cnn.Close();
                }
            }
            return dt;
        }

        public static SqlDataReader GetData(string storedProcedureName, params SqlParameter[] arrParam)
        {
            SqlConnection con = new SqlConnection(MyConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandText = storedProcedureName;


            if (arrParam != null)
            {
                foreach (SqlParameter param in arrParam)
                {
                    cmd.Parameters.Add(param);
                }
            }

            con.Open();
            return cmd.ExecuteReader();
        }

        public static (DataTable dt, bool ok, string boodschap) ExecuteDataTable(string storedProcedureName,
            params SqlParameter[] arrParam)
        {
            DataTable DT;
            bool IHaveAReturnValue = false;
            int nr = -1;

            using (SqlConnection CN = new SqlConnection(MyConnectionString))
            {
                using (SqlCommand CMD = new SqlCommand(storedProcedureName, CN))
                {
                    CMD.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter DA = new SqlDataAdapter(CMD))
                    {
                        if (arrParam != null)
                        {
                            foreach (SqlParameter param in arrParam)
                            {
                                CMD.Parameters.Add(param);
                                if (param.ParameterName == "ReturnValue")
                                {
                                    IHaveAReturnValue = true;
                                    CMD.Parameters["ReturnValue"].Direction = ParameterDirection.Output;
                                }
                            }
                        }

                        try
                        {
                            DT = new DataTable();
                            DA.Fill(DT);

                            if (IHaveAReturnValue)
                            {
                                nr = (int)CMD.Parameters["ReturnValue"].Value;
                                switch (nr)
                                {
                                    case 999:
                                        return (DT, true, "De bewerking is gelukt.");
                                    case 1:
                                        return (DT, true, "De bewerking is gelukt.");
                                    case 0:
                                        DebugLogger.log("concurrency probleem");
                                        return (null, false, "Record is ondertussen door een andere bewerking aangepast, probeer het opnieuw.");
                                    case 547:
                                        DebugLogger.log("SQl error 547 : key constraint violation");
                                        if (storedProcedureName.Substring(0, 1) == "D")
                                        {
                                            return (null, false, "De bewerking is Niet gelukt, U probeert een record te verwijderen die aan een andere record gekoppeld is.  Verwijder eerst de gekoppelde record");
                                        }
                                        else
                                        {
                                            return (null, false, "De bewerking is niet gelukt, gelieve jouw systeembeheerder te contacteren.");
                                        }
                                    case 2627:
                                        DebugLogger.log("SQl error 2627 : unique constraint violation - record bestaat al");
                                        if (storedProcedureName.Substring(0, 1) == "I")
                                        {
                                            return (null, false, "De bewerking is Niet gelukt, U probeert een record toe te voegen die reeds bestaat");
                                        }
                                        else
                                        {
                                            return (null, false, "De bewerking is niet gelukt, gelieve jouw systeembeheerder te contacteren.");
                                        }
                                    default:
                                        DebugLogger.log("Stored Procedure " + storedProcedureName  + " uitgevoerd met error - Return Code : " + nr.ToString());
                                        return (null, false, "De bewerking is Niet gelukt, gelieve jouw systeembeheerder te contacteren. (SQL Foutcode : " + nr.ToString() + ")");
                                }
                            }
                            return (DT, true, "De bewerking is gelukt.");
                        }
                        catch (SqlException ex)
                        {
                            DebugLogger.log(ex.Message);
                            nr = (int)CMD.Parameters["ReturnValue"].Value;
                            switch (nr)
                            {
                                case 997:
                                    DebugLogger.log("SQl error 997 : unique constraint violation - record bestaat al");
                                    if (storedProcedureName.Substring(0, 2) == "U_")
                                    {
                                        return (null, false, "De bewerking is Niet gelukt, U probeert een record aan te passen naar een reeds bestaand item");
                                    }
                                    else if (storedProcedureName.Substring(0, 2) == "I_")
                                    {
                                        return (null, false, "De bewerking is Niet gelukt, U probeert een record toe te voegen dat reeds bestaat");
                                    }
                                    else
                                    {
                                        return (null, false, "De bewerking is niet gelukt, gelieve jouw systeembeheerder te contacteren.");
                                    }
                                default:
                                    DebugLogger.log("Stored Procedure " + storedProcedureName + " uitgevoerd met error - Return Code : " + nr.ToString());
                                    return (null, false, "De bewerking is Niet gelukt, gelieve jouw systeembeheerder te contacteren. (SQL Foutcode : " + nr.ToString() + ")");

                            }
                        }
                        catch (Exception ex)
                        {
                            DebugLogger.log(ex.Message);
                            return (null, false, "De bewerking is Niet gelukt, gelieve jouw systeembeheerder te contacteren.");
                            throw new Exception(ex.Message);
                        }
                        finally
                        {
                            CN.Close();
                        }
                    }
                }

            }
        }

        public static (DataTable dt, bool ok, string boodschap, int returnID, object returnControlField) ExecuteDataTableWithReturnIDAndControlField(string storedProcedureName, params SqlParameter[] arrParam)
        {
            DataTable DT;
            bool IHaveAReturnValue = false;
            int nr = -1;
            int nr2 = -1;
            object temp = null;

            using (SqlConnection CN = new SqlConnection(MyConnectionString))
            {
                using (SqlCommand CMD = new SqlCommand(storedProcedureName, CN))
                {
                    CMD.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter DA = new SqlDataAdapter(CMD))
                    {
                        if (arrParam != null)
                        {
                            foreach (SqlParameter param in arrParam)
                            {
                                CMD.Parameters.Add(param);
                                if (param.ParameterName == "ReturnValue")
                                {
                                    IHaveAReturnValue = true;
                                    CMD.Parameters["ReturnValue"].Direction = ParameterDirection.Output;
                                }
                                if (param.ParameterName == "ReturnID")
                                {
                                    IHaveAReturnValue = true;
                                    CMD.Parameters["ReturnID"].Direction = ParameterDirection.Output;
                                }
                                if (param.ParameterName == "ReturnControlField")
                                {
                                    IHaveAReturnValue = true;
                                    CMD.Parameters["ReturnControlField"].Direction = ParameterDirection.Output;
                                }
                            }
                        }

                        try
                        {
                            DT = new DataTable();
                            DA.Fill(DT);

                            if (IHaveAReturnValue)
                            {
                                nr = (int)CMD.Parameters["ReturnValue"].Value;
                                switch (nr)
                                {
                                    case 999:
                                        nr2 = (int)CMD.Parameters["ReturnID"].Value;
                                        temp = (object)CMD.Parameters["ReturnControlField"].Value;
                                        return (DT, true, "De bewerking is gelukt.", nr2, temp);
                                    case 0:
                                        DebugLogger.log("concurrency probleem");
                                        return (null, false, "Record is ondertussen door een andere bewerking aangepast, probeer het opnieuw.", 0, null);
                                    case 547:
                                        DebugLogger.log("SQl error 547 : key constraint violation");
                                        if (storedProcedureName.Substring(0, 1) == "D")
                                        {
                                            return (null, false, "De bewerking is Niet gelukt, U probeert een record te verwijderen die aan een andere record gekoppeld is.  Verwijder eerst de gekoppelde record", 0, null);
                                        }
                                        else
                                        {
                                            return (null, false, "De bewerking is niet gelukt, gelieve jouw systeembeheerder te contacteren.", 0, null);
                                        }
                                    case 2627:
                                        DebugLogger.log("SQl error 2627 : unique constraint violation - record bestaat al");
                                        if (storedProcedureName.Substring(0, 1) == "I")
                                        {
                                            return (null, false, "De bewerking is Niet gelukt, U probeert een record toe te voegen dat reeds bestaat", 0, null);
                                        }
                                        else
                                        {
                                            return (null, false, "De bewerking is niet gelukt, gelieve jouw systeembeheerder te contacteren.", 0, null);
                                        }
                                    default:
                                        DebugLogger.log("Stored Procedure " + storedProcedureName + " uitgevoerd met error - Return Code : " + nr.ToString());
                                        return (null, false, "De bewerking is Niet gelukt, gelieve jouw systeembeheerder te contacteren. (SQL Foutcode : " + nr.ToString() + ")", 0, null);
                                }
                            }
                            return (DT, true, "De bewerking is gelukt.", 0, null);
                        }
                        catch (SqlException ex)
                        {
                            DebugLogger.log(ex.Message);
                            return (null, false, "De bewerking is Niet gelukt, gelieve jouw systeembeheerder te contacteren.", 0, null);
                        }
                        catch (Exception ex)
                        {
                            DebugLogger.log(ex.Message);
                            return (null, false, "De bewerking is Niet gelukt, gelieve jouw systeembeheerder te contacteren.", 0, null);
                            throw new Exception(ex.Message);
                        }
                        finally
                        {
                            CN.Close();
                        }
                    }
                }
            }
        }

        public static SqlDataReader GetData(string storedProcedureName)
        {
            SqlConnection con = new SqlConnection(MyConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandText = storedProcedureName;
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                DebugLogger.log(ex.Message);
            }

            return cmd.ExecuteReader();
        }


        public static SqlParameter Parameter(string parameterName, object parameterValue)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = parameterName;
            param.Value = parameterValue;
            param.IsNullable = true;

            return param;
        }

        public static SqlParameter Parameter2(string parameterName, object parameterValue)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = parameterName;
            if (parameterValue == null)
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = parameterValue;
            }
            param.IsNullable = true;

            return param;
        }
    }
}
