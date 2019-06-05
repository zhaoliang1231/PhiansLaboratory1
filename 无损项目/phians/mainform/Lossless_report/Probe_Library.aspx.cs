using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;
using System.Configuration;
using phians.custom_class;
using phians.Model;
using System.Collections;

namespace phians.mainform.Lossless_report
{
    public partial class Probe_Library : System.Web.UI.Page
    {
        private readonly DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            
                    
            string command =Request.QueryString["cmd"];
            switch (command)
            {
                case "load_maintenance": load_maintenance(); break; //加载数据表格
                case "Probe_add": Probe_add(); break; //添加探头
                case "Probe_edit": Probe_edit(); break; //修改探头
                case "Probe_delete": Probe_delete(); break; //修改探头
            }


        }
        //获取加载datagrid数据
        private void load_maintenance()
        {
            int Probe_state = (int)ProbeStateEnum.SC;

            string search = Request.Params.Get("search");
            string key = Request.Params.Get("key");
            string sqlwhere = "where Probe_state != '" + Probe_state + "'" ;

            //查询判断
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(key))
            {
                sqlwhere += " and " + search + " like '%" + key + "%'";
            }
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
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by " + h_sortname + " " + h_order + ")RowId,* from TB_NDT_probe_library " + sqlwhere + ")a where RowId  >= '" + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from TB_NDT_probe_library " + sqlwhere;
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            Response.Write(strJson);//返回给前台页面   
            Response.End();
        }
        #region  删除探头
        private void Probe_delete()
        {
             int Probe_state = (int)ProbeStateEnum.SC;
             string id = Request.Params.Get("id");
             string Probe_num = Request.Params.Get("Probe_num");

             string delete_sql = "UPDATE TB_NDT_probe_library SET Probe_state='" + Probe_state + "' where id='" + id + "'";

            try
            {
                db.BeginTransaction();
                db.ExecuteNonQueryByTrans(delete_sql);
                db.CommitTransacton();
                string loginAccount = Convert.ToString(Session["loginAccount"]);
                string login_username = Convert.ToString(Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "删除无损探头", "探头编号为：" + Probe_num + "");

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

        #endregion

        #region 修改探头

        private void Probe_edit() {

            string id = Request.Params.Get("id");
            string Probe_name = Request.Params.Get("Probe_name");
            string Probe_Manufacture = Request.Params.Get("Probe_Manufacture");
            string Probe_num = Request.Params.Get("Probe_num");
            string Probe_type = Request.Params.Get("Probe_type");
            string Probe_size = Request.Params.Get("Probe_size");
            string Probe_frequency = Request.Params.Get("Probe_frequency");
            string Coil_Size = Request.Params.Get("Coil_Size");
            string Probe_Length = Request.Params.Get("Probe_Length");
            string Cable_Length = Request.Params.Get("Cable_Length");
            string Mode_L = Request.Params.Get("Mode_L");
            string Mode_T = Request.Params.Get("Mode_T");
            string Chip_size = Request.Params.Get("Chip_size");
            string Angle = Request.Params.Get("Angle");
            string Nom_Angle = Request.Params.Get("Nom_Angle");
            string Shoe = Request.Params.Get("Shoe");
            string Probe_state = Request.Params.Get("Probe_state");
            string remarks = Request.Params.Get("remarks");

            SqlParameter[] para = 
                {
                    new SqlParameter("@id",id),
                    new SqlParameter("@Probe_name",Probe_name),
                    new SqlParameter("@Probe_Manufacture",Probe_Manufacture),
                    new SqlParameter("@Probe_num",Probe_num),
                    new SqlParameter("@Probe_type",Probe_type),
                    new SqlParameter("@Probe_size",Probe_size),
                    new SqlParameter("@Probe_frequency",Probe_frequency),                   
                    new SqlParameter("@Coil_Size",Coil_Size),
                    new SqlParameter("@Probe_Length",Probe_Length),
                    new SqlParameter("@Cable_Length",Cable_Length),
                    new SqlParameter("@Mode_L",Mode_L),
                    new SqlParameter("@Mode_T",Mode_T),
                    new SqlParameter("@Chip_size",Chip_size),                       
                    new SqlParameter("@Angle",Angle),
                    new SqlParameter("@Nom_Angle",Nom_Angle),
                    new SqlParameter("@Shoe",Shoe),
                    new SqlParameter("@Probe_state",Probe_state),                  
                    new SqlParameter("@remarks",remarks)
                };

            string update_sql = "UPDATE TB_NDT_probe_library SET Probe_Manufacture=@Probe_Manufacture, Probe_name=@Probe_name, Probe_num=@Probe_num, Probe_type=@Probe_type, Probe_size=@Probe_size, Probe_frequency=@Probe_frequency, "
                + "Coil_Size=@Coil_Size,  Probe_Length=@Probe_Length,  Cable_Length=@Cable_Length,  Mode_L=@Mode_L,  Mode_T=@Mode_T,  Chip_size=@Chip_size,  Angle=@Angle,  Nom_Angle=@Nom_Angle,  Shoe=@Shoe, " 
                + "Probe_state=@Probe_state, remarks=@remarks  where id=@id ";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                List<SqlParameter[]> SQLStringList2 = new List<SqlParameter[]>();
                SQLStringList2.Add(para);
                SQLStringList.Add(update_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList, SQLStringList2);
                string loginAccount = Convert.ToString(Session["loginAccount"]);
                string login_username = Convert.ToString(Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "修改无损探头", "探头编号为：" + Probe_num + "");

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
        #endregion

        #region 添加探头
        private void Probe_add() {

            string Probe_name = Request.Params.Get("Probe_name");
            string Probe_Manufacture = Request.Params.Get("Probe_Manufacture");
            string Probe_num = Request.Params.Get("Probe_num");
            string Probe_type = Request.Params.Get("Probe_type");
            string Probe_size = Request.Params.Get("Probe_size");
            string Probe_frequency = Request.Params.Get("Probe_frequency");
            string Coil_Size = Request.Params.Get("Coil_Size");
            string Probe_Length = Request.Params.Get("Probe_Length");
            string Cable_Length = Request.Params.Get("Cable_Length");
            string Mode_L = Request.Params.Get("Mode_L");
            string Mode_T = Request.Params.Get("Mode_T");
            string Chip_size = Request.Params.Get("Chip_size");
            string Angle = Request.Params.Get("Angle");
            string Nom_Angle = Request.Params.Get("Nom_Angle");
            string Shoe = Request.Params.Get("Shoe");
            string Probe_state = Request.Params.Get("Probe_state");
            string remarks = Request.Params.Get("remarks");

            SqlParameter[] para = 
                {
                    new SqlParameter("@Probe_name",Probe_name),
                    new SqlParameter("@Probe_Manufacture",Probe_Manufacture),
                    new SqlParameter("@Probe_num",Probe_num),
                    new SqlParameter("@Probe_type",Probe_type),
                    new SqlParameter("@Probe_size",Probe_size),
                    new SqlParameter("@Probe_frequency",Probe_frequency),                   
                    new SqlParameter("@Coil_Size",Coil_Size),
                    new SqlParameter("@Probe_Length",Probe_Length),
                    new SqlParameter("@Cable_Length",Cable_Length),
                    new SqlParameter("@Mode_L",Mode_L),
                    new SqlParameter("@Mode_T",Mode_T),
                    new SqlParameter("@Chip_size",Chip_size),                       
                    new SqlParameter("@Angle",Angle),
                    new SqlParameter("@Nom_Angle",Nom_Angle),
                    new SqlParameter("@Shoe",Shoe),
                    new SqlParameter("@Probe_state",Probe_state),                  
                    new SqlParameter("@remarks",remarks)
                };

            string insert_sql = "INSERT INTO TB_NDT_probe_library (Probe_Manufacture,Probe_name,Probe_num,Probe_type,Probe_size,Probe_frequency,Coil_Size,Probe_Length,Cable_Length,Mode_L,Mode_T,"
                + "Chip_size,Angle,Nom_Angle,Shoe,Probe_state,remarks) values(@Probe_Manufacture, @Probe_name,@Probe_num,@Probe_type,@Probe_size,@Probe_frequency,@Coil_Size,@Probe_Length,"
                + "@Cable_Length,@Mode_L,@Mode_T,@Chip_size,@Angle,@Nom_Angle,@Shoe,@Probe_state,@remarks)";


            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                List<SqlParameter[]> SQLStringList2 = new List<SqlParameter[]>();
                SQLStringList2.Add(para);
                SQLStringList.Add(insert_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList, SQLStringList2);
                string loginAccount = Convert.ToString(Session["loginAccount"]);
                string login_username = Convert.ToString(Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "添加无损探头", "探头编号为：" + Probe_num + "");
               
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

        #endregion

    }
}