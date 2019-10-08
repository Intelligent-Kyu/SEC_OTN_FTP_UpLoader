using OneSpirit.Enterprise.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SET_OTN
{
    public static class ExtensionMethod
    {
        public static ExportFile ToFileWrite(this List<SEC_OTN_DATA> otn, string path)
        {
            ExportFile file = ExportFile.GetFile("PCBINFOR_A0ND" + DateTime.Now.ToString("yyyyMMddHH"), "ff", path);

            List<string> fileheader = new List<string>()
            {
                    "Vendor_Code","Material_Code","Diagram_No","Lot_ID",
                    "AOI_Start_Time","AOI_End_Time",
                    "AOI_In_Qty","AOI_Out_Qty","AOI_YLD",
                    "AOI_Open_Qty","AOI_Open_YLD",
                    "AOI_Short_Qty","AOI_Short_YLD",
                    "BBT_Start_Time","BBT_End_Time",
                    "BBT_In_Qty","BBT_Out_Qty","BBT_YLD",
                    "BBT_Open_Qty","BBT_Open_YLD",
                    "BBT_Short_Qty","BBT_Short_YLD"
            };

            try
            {
                using (StreamWriter writer = new StreamWriter(file.FullName, false, new UTF8Encoding(false)))
                {
                    writer.WriteLine(String.Join(",", fileheader));

                    foreach (var lotdata in otn)
                    {
                        writer.WriteLine(lotdata.ToString());
                    }
                }

                //StreamWriter writer = File.CreateText(file.FullName);
                //writer.WriteLine(String.Join(",", fileheader));

                //foreach (var v in otn)
                //{
                //    writer.WriteLine(v.ToString());
                //}
                //writer.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
           
            return file;
        }
    }
}
