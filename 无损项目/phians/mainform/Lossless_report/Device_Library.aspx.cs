using phians.custom_class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace phians.mainform.Lossless_report
{
    public partial class Device_Library : System.Web.UI.Page
    {
        private readonly DBHelper db = new DBHelper();

        protected void Page_Load(object sender, EventArgs e)
        {
            string command = Request.QueryString["cmd"];
            switch (command)
            {
                case "load_maintenance": load_maintenance(); break; //加载数据表格
                case "Device_add": Device_add(); break; //添加设备
                case "Device_edit": Device_edit(); break; //修改设备
                case "Device_delete": Device_delete(); break; //修改设备
            }
        }

        //获取加载datagrid数据
        private void load_maintenance()
        {
            int E_state = (int)DeviceStateEnum.SC;

            string search = Request.Params.Get("search");
            string key = Request.Params.Get("key");
            string sqlwhere = "where E_state != '" + E_state + "' ";

            //查询判断
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(key))
            {
                sqlwhere += " and " + search + " like '%" + key + "%'";
            }
            string getpage1 = Request.Params.Get("page");
            string getrows1 = Request.Params.Get("rows");
            string h_order = Request.Params.Get("order");
            string h_sortname =Request.Params.Get("sort");
            if (h_order == null)
            {
                h_order = "asc";
                h_sortname = "id";
            }
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by " + h_sortname + " " + h_order + ")RowId,* from dbo.TB_NDT_equipment_library " + sqlwhere + ")a where RowId  >= '" + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from dbo.TB_NDT_equipment_library " + sqlwhere;
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            Response.Write(strJson);//返回给前台页面   
            Response.End();
        }
        //添加设备
        private void Device_add()
        {
            string E_state = Request.Params.Get("E_state");
            string report_id = Request.Params.Get("report_id");
            string equipment_nem = Request.Params.Get("equipment_nem");
            string equipment_Type = Request.Params.Get("equipment_Type");
            string equipment_num = Request.Params.Get("equipment_num");
            string Manufacture = Request.Params.Get("Manufacture");
            string range_ = Request.Params.Get("range_");
            string effective = Request.Params.Get("effective");
            string Remarks = Request.Params.Get("Remarks");
            string personnel_time = DateTime.Now.ToLocalTime().ToString();                    //时间
            string personnel_ = Convert.ToString(Session["loginAccount"]);   //用户

            string insert_sql = "INSERT INTO TB_NDT_equipment_library (personnel_time,personnel_,report_id,equipment_nem,equipment_Type,equipment_num,Manufacture,range_,effective,E_state,Remarks) values('"
                + personnel_time + "','" + personnel_ + "','" + report_id + "','" + equipment_nem + "','" + equipment_Type + "','" + equipment_num + "','" + Manufacture + "','" 
                + range_ + "','" + effective + "','" + E_state + "','" + Remarks + "')";


            try
            {
                db.BeginTransaction();
                db.ExecuteNonQueryByTrans(insert_sql);
                db.CommitTransacton();
                string loginAccount = Convert.ToString(Session["loginAccount"]);
                string login_username = Convert.ToString(Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "添加无损设备", "设备编号为：" + equipment_num + "");

                Response.Write("T");
            }
            catch (System.Exception ex)
            {
                db.RollbackTransaction();
                Response.Write(ex);
            }
            finally
            {
                Response.End();
            }

        }
        //修改设备
        private void Device_edit()
        {

            string id = Request.Params.Get("id");
            string equipment_nem = Request.Params.Get("equipment_nem");
            string equipment_Type = Request.Params.Get("equipment_Type");
            string equipment_num = Request.Params.Get("equipment_num");
            string Manufacture = Request.Params.Get("Manufacture");
            string range_ = Request.Params.Get("range_");
            string effective = Request.Params.Get("effective");
            string E_state = Request.Params.Get("E_state");
            string Remarks = Request.Params.Get("Remarks");

            string update_sql = "UPDATE TB_NDT_equipment_library SET equipment_nem='" + equipment_nem + "',equipment_Type='" + equipment_Type + "',equipment_num='"
                    + equipment_num + "',Manufacture='" + Manufacture + "',range_='" + range_ + "',effective='" + effective + "',E_state='" + E_state + "',Remarks='" + Remarks + "' where id='" + id + "'";
            try
            {
                db.BeginTransaction();
                db.ExecuteNonQueryByTrans(update_sql);
                db.CommitTransacton();
                string loginAccount = Convert.ToString(Session["loginAccount"]);
                string login_username = Convert.ToString(Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "修改无损设备", "设备编号为：" + equipment_num + "");

                Response.Write("T");
            }
            catch (System.Exception ex)
            {
                db.RollbackTransaction();
                Response.Write(ex);
            }
            finally
            {
                Response.End();
            }

        }
        //删除设备
        private void Device_delete()
        {
            int E_state = (int)DeviceStateEnum.SC;

            string id = Request.Params.Get("id");
            string equipment_num = Request.Params.Get("equipment_num");

            string delete_sql = "update dbo.TB_NDT_equipment_library set E_state='" + E_state + "' where id='" + id + "'";

            try
            {
                db.BeginTransaction();
                db.ExecuteNonQueryByTrans(delete_sql);
                db.CommitTransacton();
                string loginAccount = Convert.ToString(Session["loginAccount"]);
                string login_username = Convert.ToString(Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "删除无损设备", "设备编号为：" + equipment_num + "");

                Response.Write("T");
            }
            catch (System.Exception ex)
            {
                db.RollbackTransaction();
                Response.Write(ex);
            }
            finally
            {
                Response.End();
            }


        }



    }
}