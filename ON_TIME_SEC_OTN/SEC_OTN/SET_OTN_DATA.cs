using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SET_OTN
{
    public class SEC_OTN_DATA
    {
        public string VendorCode { get { return "A0ND"; } }  //A0ND //DY4W
        public string MaterialCode { get; set; }
        public string DiagramNo { get; set; }
        public string JobName { get; set; }
        public Dictionary<string,DeptData> DicDepts { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string separator = ",";

            sb.Append(VendorCode + separator);
            sb.Append(MaterialCode + separator);
            sb.Append(DiagramNo + separator);
            sb.Append(JobName + separator);

            if (DicDepts.ContainsKey("AOI"))
            {
                sb.Append(DicDepts["AOI"].StartDateTime.ToString("yyyy-MM-dd-HH:mm") + separator);
                sb.Append(DicDepts["AOI"].EndDateTime.ToString("yyyy-MM-dd-HH:mm") + separator);
                sb.Append(DicDepts["AOI"].InputQty + separator);
                sb.Append(DicDepts["AOI"].OutputQty + separator);
                sb.Append(DicDepts["AOI"].Yield + separator);

                sb.Append(DicDepts["AOI"].DicDefects["OPEN"].OutputQty + separator);
                sb.Append(DicDepts["AOI"].DicDefects["OPEN"].Yield + separator);
                sb.Append(DicDepts["AOI"].DicDefects["SHORT"].OutputQty + separator);
                sb.Append(DicDepts["AOI"].DicDefects["SHORT"].Yield + separator);
            }
            else
            {
                for (int i = 0; i < 9; i++)
                    sb.Append(separator);
            }

            if (DicDepts.ContainsKey("BBT"))
            {
                sb.Append(DicDepts["BBT"].StartDateTime.ToString("yyyy-MM-dd-HH:mm") + separator);
                sb.Append(DicDepts["BBT"].EndDateTime.ToString("yyyy-MM-dd-HH:mm") + separator);
                sb.Append(DicDepts["BBT"].InputQty + separator);
                sb.Append(DicDepts["BBT"].OutputQty + separator);
                sb.Append(DicDepts["BBT"].Yield + separator);

                sb.Append(DicDepts["BBT"].DicDefects["OPEN"].OutputQty + separator);
                sb.Append(DicDepts["BBT"].DicDefects["OPEN"].Yield + separator);
                sb.Append(DicDepts["BBT"].DicDefects["SHORT"].OutputQty + separator);
                sb.Append(DicDepts["BBT"].DicDefects["SHORT"].Yield + separator);
            }
            else
            {
                for (int i = 0; i < 9; i++)
                    sb.Append(separator);
            }

            return sb.ToString().Substring(0, sb.ToString().Length - 1);
        }
    }

    public class DeptData : DefectsQty
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime  { get; set; }

        public Dictionary<string, DefectsQty> DicDefects { get; set; }
    }

    public class DefectsQty
    {
        public int InputQty { get; set; }   
        public int OutputQty { get; set; }  //defect Qty, input Qty
        public decimal Yield
        {
            get { return 100 - (Math.Round(Convert.ToDecimal(OutputQty) /  Convert.ToDecimal(InputQty) * 100,3)); }

        }
    }
}
