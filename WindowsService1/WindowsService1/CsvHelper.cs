using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    public class CsvHelper
    {
        public void ExecuteProcess(SqlDataReader reader)
        {
            string path = "C:\\text.csv";
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            while (reader.Read())
            {
                string fullLine = string.Empty;


                string belgeTarih = reader.GetDateTime(0).ToString();
                string islemTipi = reader.GetInt32(1).ToString();
                string vadeliTarih = reader.GetDateTime(2).ToString();

                string meblag = reader.GetDouble(4).ToString();
                meblag = meblag.Replace(',', '.');

                string bakiye = reader.GetDouble(5).ToString();
                bakiye = bakiye.Replace(',', '.');

                fullLine += belgeTarih;
                fullLine += ',';
                fullLine += islemTipi;
                fullLine += ',';
                fullLine += vadeliTarih;
                fullLine += ',';
                fullLine += islemTipi;
                fullLine += ',';
                fullLine += meblag;
                fullLine += ',';
                fullLine += bakiye;

                sw.WriteLine(fullLine);
                sw.Flush();

                reader.NextResult();
            }

            sw.Close();
            fs.Close();
        }
    }
}
