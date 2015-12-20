using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 




public class Driver_Behavior : IHttpHandler,IRequiresSessionState
{
   

    public Driver_Behavior()
	{
		//
		// TODO: Add constructor logic here
		//
	}




    
        private static string GetJson(object obj)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            jsonSerializer.MaxJsonLength = 2147483644;
            return jsonSerializer.Serialize(obj);
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string from = context.Request["from"].ToString();
            string to = context.Request["to"].ToString();
            string[] froms = from.Split('T');
            string[] tos = to.Split('T');
            MySqlCommand cmd;
            VehicleDBMgr vdm = new VehicleDBMgr();
            DataRow[] result;
            DataSet locationinfo;
           string str = "SELECT * FROM experiment_lab.driver_behavior_logs where time_stamp between " + "'" + froms[0] + " " + froms[1] + ":00" + "' " + "and '" + tos[0] + " " + tos[1] + ":00" + "'"; //This works fine
           cmd = new MySqlCommand(str);
            locationinfo = vdm.SelectQuery(cmd);

            result = locationinfo.Tables[0].Select("");

            List<Information_pos> list = new List<Information_pos>();

         

          
     
         

         


          float[] mov_var = new float[result.Length];
          float[] acc_x = new float[result.Length];
          float[] mean = new float[result.Length];


          for (int i = 0; i < result.Length; i++)
          {
              acc_x[i] = float.Parse(result[i]["x_axis"].ToString());



          }

       /*     
            float thr=4;
        
            

          for(int i=0;i<result.Length-1;i++)
          {
              if(acc_x[i+1]-acc_x[i]>=thr || acc_x[i]-acc_x[i+1] >= thr)
              {

                  Information_pos info = new Information_pos(float.Parse(result[i]["latitude"].ToString()), float.Parse(result[i]["longitude"].ToString()));
                  info.axis = 'x';
                  list.Add(info);

              }




          }
             
             

            */

          float max_var = -9999;
          for (int i = 2; i <= result.Length - 3; i++)
          {
              if (i == 2)
                  mean[i] = (acc_x[0] + acc_x[1] + acc_x[2] + acc_x[3] + acc_x[4]) / 5;
              else
                  mean[i] = (mean[i - 1] * 5 - acc_x[i - 3] + acc_x[i + 2]) / 5;

              for (int j = i - 2; j <= i + 2; j++)
              {
                  mov_var[i] = mov_var[i] + (mean[i] - acc_x[j]) * (mean[i] - acc_x[j]);

              }
              mov_var[i] = mov_var[i] / 5;
              mov_var[i] = (float)Math.Sqrt(mov_var[i]);

              if (mov_var[i] > max_var)
                  max_var = mov_var[i];
          }

          float fin_mean = 0;
          for (int i = 2; i <= result.Length - 3; i++)
          {
              fin_mean = fin_mean + mov_var[i];
          }

          fin_mean = fin_mean / (result.Length - 4);
          float fin_var = 0;

          for (int i = 2; i <= result.Length - 3; i++)
          {

              fin_var = fin_var + ((mov_var[i] - fin_mean) * (mov_var[i] - fin_mean));
              if (fin_var < 0)
                  Console.Write("Hell");
          }
          fin_var = fin_var / (result.Length - 4);
          fin_var = (float)Math.Sqrt(fin_var);
            
          for (int i = 2; i <= result.Length - 3; i++)
          {
             
              if (mov_var[i] > (fin_mean + fin_var * 2))
              {
               
                   
                     Information_pos info = new Information_pos(float.Parse( result[i]["latitude"].ToString()), float.Parse(result[i]["longitude"].ToString()) );
                     info.axis = 'x';
                  list.Add(info);
              }

          }

             
            string pass = GetJson(list);
           context.Response.Write(pass);

        }
}



public class Information_pos
{
   public  float lat;
   public float lon;
   public char axis;
    public Information_pos(float a, float b)
    {
        this.lat = a;
        this.lon = b;
    }
   
}