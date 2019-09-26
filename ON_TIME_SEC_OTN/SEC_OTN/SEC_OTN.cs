using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace SET_OTN
{
    public class SEC_OTN
    {
        OracleConnection oraConn;
        public bool DBConnect()
        {
            try
            {
                oraConn = new OracleConnection(ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString);
                oraConn.Open();

                //NLS Setting
                var info = oraConn.GetSessionInfo();
                info.Language = "AMERICAN";
                info.Territory = "AMERICA";
                oraConn.SetSessionInfo(info);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool DBdisconnet()
        {
            try
            {
                if (oraConn.State == ConnectionState.Open)
                {
                    oraConn.Close();
                    oraConn.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        
        public List<SEC_OTN_DATA> getOnTimeData()
        {
            List<SEC_OTN_DATA> otnList = new List<SEC_OTN_DATA>();

            try
            {

                OracleCommand oraCmd = oraConn.CreateCommand();
                oraCmd.CommandText = "customer_data_pkg.GetSecOTNDataCurrent";
                oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oraCmd.Parameters.Add(new OracleParameter("vCursor", OracleDbType.RefCursor, System.Data.ParameterDirection.Output));
                OracleDataReader dr = oraCmd.ExecuteReader();

                while (dr.Read())
                {
                    SEC_OTN_DATA otn = new SEC_OTN_DATA();
                    otn.DicDepts = new Dictionary<string, DeptData>();
                    otn.MaterialCode = dr["material_code"].ToString();
                    otn.DiagramNo = dr["diagram_no"].ToString();
                    otn.JobName = dr["job_name"].ToString();

                    //AOI
                    DeptData dept = new DeptData();
                    dept.DicDefects = new Dictionary<string, DefectsQty>();
                    dept.StartDateTime = dr["aoi_start_time"] == DBNull.Value ? dept.StartDateTime : Convert.ToDateTime(dr["aoi_start_time"]);
                    dept.EndDateTime   = dr["aoi_end_time"]   == DBNull.Value ? dept.EndDateTime   : Convert.ToDateTime(dr["aoi_end_time"]);
                    dept.InputQty      = dr["aoi_input_qty"]  == DBNull.Value ? dept.InputQty      : Convert.ToInt32(dr["aoi_input_qty"]);
                    dept.OutputQty     = dr["aoi_output_qty"] == DBNull.Value ? dept.OutputQty     : Convert.ToInt32(dr["aoi_output_qty"]);

                    //AOI OPEN DEFECT
                    DefectsQty aoiDefectQty = new DefectsQty();
                    aoiDefectQty.InputQty = dept.InputQty;
                    aoiDefectQty.OutputQty = dr["aoi_open_qty"] == DBNull.Value ? aoiDefectQty.OutputQty : Convert.ToInt32(dr["aoi_open_qty"]);
                    dept.DicDefects.Add("OPEN", aoiDefectQty);

                    //AOI SHORT DEFECT
                    aoiDefectQty = new DefectsQty();
                    aoiDefectQty.InputQty = dept.InputQty;
                    aoiDefectQty.OutputQty = dr["aoi_short_qty"] == DBNull.Value ? aoiDefectQty.OutputQty : Convert.ToInt32(dr["aoi_short_qty"]);
                    dept.DicDefects.Add("SHORT", aoiDefectQty);

                    if(!dept.InputQty.Equals(0))
                        otn.DicDepts.Add("AOI", dept);

                    //BBT
                    dept = new DeptData();
                    dept.DicDefects = new Dictionary<string, DefectsQty>();
                    dept.StartDateTime = dr["bbt_start_time"] == DBNull.Value ? dept.StartDateTime : Convert.ToDateTime(dr["bbt_start_time"]);
                    dept.EndDateTime   = dr["bbt_end_time"]   == DBNull.Value ? dept.EndDateTime   : Convert.ToDateTime(dr["bbt_end_time"]);
                    dept.InputQty      = dr["bbt_input_qty"]  == DBNull.Value ? dept.InputQty      : Convert.ToInt32(dr["bbt_input_qty"]);
                    dept.OutputQty     = dr["bbt_output_qty"] == DBNull.Value ? dept.OutputQty     : Convert.ToInt32(dr["bbt_output_qty"]);

                    //BBT OPEN DEFECT
                    DefectsQty bbtDefectQty = new DefectsQty();
                    bbtDefectQty.InputQty = dept.InputQty;
                    bbtDefectQty.OutputQty = dr["bbt_open_qty"] == DBNull.Value ? bbtDefectQty.OutputQty : Convert.ToInt32(dr["bbt_open_qty"]);
                    dept.DicDefects.Add("OPEN", bbtDefectQty);

                    //BBT SHORT DEFECT
                    bbtDefectQty = new DefectsQty();
                    bbtDefectQty.InputQty = dept.InputQty;
                    bbtDefectQty.OutputQty = dr["bbt_short_qty"] == DBNull.Value ? bbtDefectQty.OutputQty  : Convert.ToInt32(dr["bbt_short_qty"]);
                    dept.DicDefects.Add("SHORT", bbtDefectQty);

                    if(!dept.InputQty.Equals(0))
                        otn.DicDepts.Add("BBT", dept);

                    otnList.Add(otn);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return otnList;
        }
    }
}
