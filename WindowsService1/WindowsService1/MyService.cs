using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Configuration;

namespace WindowsService1
{
    [RunInstaller(true)]
    public partial class MyService : ServiceBase
    {
        public System.Timers.Timer timerRunTask = new System.Timers.Timer();
        DateTime scheduledTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ScheduledTime"]);
        public bool first = false;
        public MyService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                timerRunTask.Interval = 30 * 1000;
                timerRunTask.Enabled = true;
                timerRunTask.Elapsed += new System.Timers.ElapsedEventHandler(timerRunTask_Elapsed);
                timerRunTask.Start();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                timerRunTask.Stop();
                timerRunTask.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void timerRunTask_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour == scheduledTime.Hour)
            {
                if (DateTime.Now.Minute == scheduledTime.Minute)
                {
                    if (!first)
                    {
                        GetRecord();
                        first = true;
                    }
                    else
                    {
                        first = false;
                    }

                }
            }
        }

        private static void GetRecord()
        {
            using (SqlConnection connection = new SqlConnection("Server=.;Database=TL_Cursor; Trusted_Connection=True;"))
            {
                using (SqlCommand command = new SqlCommand())
                {

                    command.Connection = connection;
                    command.CommandText = "exec GetCurrentExtract";

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    CsvHelper helper = new CsvHelper();
                    helper.ExecuteProcess(reader);

                    connection.Close();
                }
            }

        }

    }
}
