using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace phians.mainform
{
    public partial class message_management : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string cmd = Request["cmd"];
            switch (cmd)
            {
                case "load_message": load_message(); break;//加载信息数据表格
                case "click_ok": click_ok(); break;//确认收到选择消息
                case "click_all_ok": click_all_ok(); break;//确认收到全部消息
                case "load_message_old": load_message_old(); break;//加载已确认信息数据表格
            }
        }

        //加载信息数据表格
        public void load_message()
        {
            string getpage1 = Request.Params.Get("page");
            string getrows1 = Request.Params.Get("rows");
            string h_order = Request.Params.Get("order");
            string h_sortname = Request.Params.Get("sort");
            if (h_order == null)
            {
                h_order = "asc";
                h_sortname = "id";
            }
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            string login_user = Session["loginAccount"].ToString().Trim();
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by send_time desc)RowId,* from dbo.TB_show_message where User_count  = '" + login_user + "')a where RowId  >= '" + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from  dbo.TB_show_message where User_count  = '" + login_user + "' ORDER BY send_time desc";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            Response.Write(strJson);//返回给前台页面   
            Response.End();
        }
        //确认收到选择消息
        public void click_ok() {
            string ids = Request.Params.Get("ids");
            string[] id = new string[100]; 
            id = ids.Split(';');
            string conditions = "";
            for (int i = 0; i < id.Length; i++)
            {
                if (i == 0)
                {
                    conditions = "where id = " + id[i];
                }
                if (i > 0)
                {
                    conditions = conditions + " or id = " + id[i];
                }
            }

            //登录用户
            string login_user = Convert.ToString(Session["loginAccount"]);
            string confirm_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string delete_read = "delete from dbo.TB_show_message " + conditions + "";
            string insert_read = "insert into  dbo.TB_show_message_old(User_count,message,message_type,create_time,send_time,confirm_time,confirm_message_flag,confirm_push_flag,message_push_personnel ) select User_count,message,message_type,create_time,send_time,'" + confirm_time + "','1','1',message_push_personnel from dbo.TB_show_message " + conditions + "";      
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
          
                SQLStringList.Add(insert_read);
                SQLStringList.Add(delete_read);
                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                Response.Write("T");


            }
            catch (Exception)
            {

               Response.Write("F");

            }
            finally
            {

               Response.End();
            }         
        }
        //确认收到全部消息
        public void click_all_ok()
        {
            //登录用户
            string login_user = Convert.ToString(Session["loginAccount"]);
            string confirm_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string delete_read = "delete from dbo.TB_show_message where User_count='" + login_user + "'";
            string insert_read = "insert into  dbo.TB_show_message_old(User_count,message,message_type,create_time,send_time,confirm_time,confirm_message_flag,confirm_push_flag,message_push_personnel ) select User_count,message,message_type,create_time,send_time,'" + confirm_time + "','1','1',message_push_personnel from dbo.TB_show_message where User_count='" + login_user + "'";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();

                SQLStringList.Add(insert_read);
                SQLStringList.Add(delete_read);
                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                Response.Write("T");


            }
            catch (Exception)
            {

                Response.Write("F");

            }
            finally
            {

                Response.End();
            }
        }
        //加载已确认信息数据表格
        public void load_message_old()
        {

            string getpage1 = Request.Params.Get("page");
            string getrows1 = Request.Params.Get("rows");
            string h_order = Request.Params.Get("order");
            string h_sortname = Request.Params.Get("sort");
            if (h_order == null)
            {
                h_order = "asc";
                h_sortname = "id";
            }
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            string login_user = Session["loginAccount"].ToString().Trim();
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over( ORDER BY confirm_time desc)RowId,* from dbo.TB_show_message_old where User_count  = '" + login_user + "' )a where RowId  >= '" + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from  dbo.TB_show_message_old where User_count  = '" + login_user + "' ORDER BY confirm_time desc";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            Response.Write(strJson);//返回给前台页面   
            Response.End();

        }
    }
}