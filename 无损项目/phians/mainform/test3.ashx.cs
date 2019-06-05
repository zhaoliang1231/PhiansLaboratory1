using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;
namespace phians.mainform
{
    /// <summary>
    /// test3 的摘要说明
    /// </summary>
    public class test3 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string command = context.Request.QueryString["cmd"];
            switch (command)
            {
                case "add": add(context); break;//添加
                case "edit": edit(context); break;//修改
                case "del": h_del(context); break;//删除
            }
        }

        private void add(HttpContext context)
        {
            String company = context.Request.Params.Get("company");
            String company_en = context.Request.Params.Get("company_en");
            String address = context.Request.Params.Get("address");
            String address_en = context.Request.Params.Get("address_en");
            String postcode = context.Request.Params.Get("postcode");
            String phone = context.Request.Params.Get("phone");
            String c_fax = context.Request.Params.Get("c_fax");
            String web_site = context.Request.Params.Get("web_site");
            String rg_sites = context.Request.Params.Get("rg_sites");
            String Supply_project = context.Request.Params.Get("Supply_project");
            String Investment_type = context.Request.Params.Get("Investment_type");
            String partnership = context.Request.Params.Get("partnership");
            String remarks = context.Request.Params.Get("remarks");
            String keyword = context.Request.Params.Get("keyword");
            String login_user = Convert.ToString(context.Session["loginAccount"]);

            //SQLHelper sql2 = new SQLHelper();
            //SqlConnection con = sql2.getConn();
            String insert_sql = "INSERT INTO dbo.TB_Supplier_info (company,company_en,address,address_en,postcode,phone,c_fax,web_site,rg_sites,Supply_project,"
                + "Investment_type,partnership,remarks,keyword,appraiser) values('" + company + "','" + company_en + "','" + address + "','" + address_en + "','"
                + postcode + "','" + phone + "','" + c_fax + "','" + web_site + "','" + rg_sites + "','" + Supply_project + "','" + Investment_type + "','"
                + partnership + "','" + remarks + "','" + keyword + "','" + login_user + "') ";
            //SqlCommand cmd1 = new SqlCommand(sql1, con);
            //int count = cmd1.ExecuteNonQuery();
            //con.Close();
            //if (count >= 1)
            //{
            //    context.Response.Write("T");
            //    context.Response.End();
            //}
            //else
            //{
            //    context.Response.Write("F");
            //    context.Response.End();
            //}
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(insert_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList, context);
            }
            catch (Exception)
            {
                context.Response.Write("F");
            }
            finally
            {
                context.Response.End();
            }

        }

        private void edit(HttpContext context)
        {
            String company = context.Request.Params.Get("company");
            String company_en = context.Request.Params.Get("company_en");
            String address = context.Request.Params.Get("address");
            String address_en = context.Request.Params.Get("address_en");
            String postcode = context.Request.Params.Get("postcode");
            String phone = context.Request.Params.Get("phone");
            String c_fax = context.Request.Params.Get("c_fax");
            String web_site = context.Request.Params.Get("web_site");
            String rg_sites = context.Request.Params.Get("rg_sites");
            String Supply_project = context.Request.Params.Get("Supply_project");
            String Investment_type = context.Request.Params.Get("Investment_type");
            String partnership = context.Request.Params.Get("partnership");
            String remarks = context.Request.Params.Get("remarks");
            String keyword = context.Request.Params.Get("keyword");
            String login_user = Convert.ToString(context.Session["loginAccount"]);
            String id = context.Request.Params.Get("id");

            //SQLHelper sql2 = new SQLHelper();
            //SqlConnection con = sql2.getConn();
            String update_sql = "UPDATE dbo.TB_Supplier_info SET company='" + company + "',company_en='" + company_en + "',address='" + address + "',address_en='"
                + address_en + "',postcode='" + postcode + "',phone='" + phone + "',c_fax='" + c_fax + "', web_site='" + web_site + "',rg_sites='"
                + rg_sites + "',Supply_project='" + Supply_project + "',Investment_type='" + Investment_type + "',partnership='" + partnership + "',remarks='"
                + remarks + "',keyword='" + keyword + "' where id='" + id + "'";
            //SqlCommand cmd1 = new SqlCommand(sql1, con);
            //int count = cmd1.ExecuteNonQuery();
            //con.Close();
            //if (count >= 1)
            //{
            //    context.Response.Write("T");
            //    context.Response.End();
            //}
            //else
            //{
            //    context.Response.Write("F");
            //    context.Response.End();
            //}
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(update_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList, context);
            }
            catch (Exception)
            {
                context.Response.Write("F");
            }
            finally
            {
                context.Response.End();
            }

        }

        private void h_del(HttpContext context)
        {

            String id = context.Request.Params.Get("id");
            //SQLHelper sql2 = new SQLHelper();
            //SqlConnection con = sql2.getConn();
            String delete_sql = "delete from dbo.TB_Supplier_info  where id='" + id + "'";
            //SqlCommand cmd1 = new SqlCommand(sql1, con);
            //int count = cmd1.ExecuteNonQuery();
            //con.Close();
            //if (count >= 1)
            //{
            //    context.Response.Write("T");
            //    context.Response.End();
            //}
            //else
            //{
            //    context.Response.Write("F");
            //    context.Response.End();
            //}
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(delete_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList, context);
            }
            catch (Exception)
            {
                context.Response.Write("F");
            }
            finally
            {
                context.Response.End();
            }

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}