using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using MySql.Data.MySqlClient;

public class VehicleDBMgr
{
      string ConnectionString;
      public MySqlConnection conn = new MySqlConnection();
      List<string> data = new List<string>();
      public MySqlCommand cmd;
    //  public string UserName = "";
      object obj_lock = new object();
      public VehicleDBMgr()
      {
         
              conn.ConnectionString = @"SERVER=122.175.32.16;DATABASE=experiment_lab;UID=root;PASSWORD=bsr@123radhA;";
      }

    public static DateTime GetTime(MySqlConnection conn)
    {

        DataSet ds = new DataSet();
        DateTime dt = DateTime.Now;
        MySqlCommand cmd = new MySqlCommand("SELECT NOW()");
        cmd.Connection = conn;
        if (cmd.Connection.State == ConnectionState.Open)
        {
            cmd.Connection.Close();
        }
        conn.Open();
        //cmd.ExecuteNonQuery();
        MySqlDataAdapter sda = new MySqlDataAdapter();
        sda.SelectCommand = cmd;
        sda.Fill(ds, "Table");
        conn.Close();
        if (ds.Tables[0].Rows.Count > 0)
        {
            dt = (DateTime)ds.Tables[0].Rows[0][0];
        }
        return dt;
    }

    public  bool insert(MySqlCommand _cmd)
    {

        try
        {
            cmd = _cmd;
            lock (cmd)
            {
                cmd.Connection = conn;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            return true;
        }
        catch (Exception ex)
        {
            cmd.Connection.Close();
            throw new ApplicationException(ex.Message);
        }
        //}
    }


    public long insertScalar(MySqlCommand _cmd)
    {
        long sno = 0;
        try
        {
            cmd = _cmd;
            lock (cmd)
            {
                cmd.Connection = conn;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                sno = cmd.LastInsertedId;
                cmd.Connection.Close();
            }
            return sno;
        }
        catch (Exception ex)
        {
            cmd.Connection.Close();
            throw new ApplicationException(ex.Message);
        }
        //}
    }
    public  DataSet SelectQuery(MySqlCommand _cmd)
    {
        lock (obj_lock)
        {
            cmd = _cmd;

            lock (cmd)
            {
                try
                {
                    DataSet ds = new DataSet();
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MySqlDataAdapter sda = new MySqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds, "table");
                    conn.Close();
                    return ds;
                }
                catch (Exception ex)
                {
                    conn.Close();
                        throw new ApplicationException(ex.Message);
                }
            }
        }
    }

    public void Update(MySqlCommand _cmd)
    {
        lock (obj_lock)
        {
            try
            {

                cmd = _cmd;
                lock (cmd)
                {
                    cmd.Connection = conn;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                cmd.Connection.Close();
                throw new ApplicationException(ex.Message);
            }
        }
    }

    public void Delete(MySqlCommand _cmd)
    {
        lock (obj_lock)
        {
            try
            {
                cmd = _cmd;
                cmd.Connection = conn;
                cmd.Connection.Open();
             int ii=   cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                cmd.Connection.Close();
                throw new ApplicationException(ex.Message);
            }
        }
    }

    public long insertgetrefno(MySqlCommand _cmd)
    {
        long refno = 0;
        try
        {
            cmd = _cmd;
            lock (cmd)
            {
                cmd.Connection = conn;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                refno = cmd.LastInsertedId;
                cmd.Connection.Close();
            }
            return refno;
        }
        catch (Exception ex)
        {
            cmd.Connection.Close();
            throw new ApplicationException(ex.Message);
        }
        //}
    }
}