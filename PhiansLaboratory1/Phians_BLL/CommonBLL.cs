using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Phians_Entity;
using Phians_Entity.LosslessReport;

namespace Phians_BLL
{
    public class CommonBLL
    {
        ICommonDAL dal = DALFactory.GetCommonDAL();

        #region 检查人员权限
        public bool GetAuthorization_BLL(Guid UserId, string U_url)
        {

            int Authorization_ount = dal.GetAuthorization(UserId, U_url);
            if (Authorization_ount > 0)
            {

                return true;
            }
            else { return false; }

        }

        #endregion

        #region 注册消息
        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="model">注册消息实体</param>
        /// <returns></returns>

        public bool RegisterSignalRBLL(TB_MessageRegister model)
        {

            return dal.RegisterSignalR(model);
        }
        #endregion

        #region 获取注册消息
        /// <summary>
        /// 获取注册消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TB_MessageRegister getRegisterSignalRInfoLL(Guid UserId)
        {

            return dal.getRegisterSignalRInfo(UserId);
        }
        #endregion

        #region 日志写入

        public static int operation_log_(Guid UserId, string OperationType, string OperationInfo)
        {

            string OperationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string OperationIp = GetIP4Address();
            string sql1 = "insert into TB_OperationLog (UserId,OperationDate,OperationType,OperationInfo,OperationIp) values('" + UserId + "','" + OperationDate + "','" + OperationType + "','" + OperationInfo + "','" + OperationIp + "')";
            try
            {
                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                //SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql1);
                //con.Close();

            }
            catch
            {

            }
            return 1;

        }
        public static string GetIP4Address()
        {
            string IP4Address = String.Empty;

            // foreach (IPAddress IPA in Dns.GetHostAddresses(System.Web.HttpContext.Current.Request.UserHostAddress))
            string hostName = Dns.GetHostName();
            foreach (IPAddress IPA in Dns.GetHostAddresses(hostName))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }
        #endregion

        #region 组、人员组合选择系列

        #region 室下拉框
        /// <summary>
        /// 室下拉框
        /// </summary>
        /// <param name="TempTable">TempTable.Group = 组id</param>
        /// <returns></returns>
        public List<ComboboxEntity> LoadRoomCombobox(dynamic TempTable)
        {

            return dal.LoadRoomCombobox(TempTable);
        }
        #endregion

        #region 组下拉框
        /// <summary>
        /// 组下拉框
        /// </summary>
        /// <param name="TempTable">TempTable.Group = 组id</param>
        /// <returns></returns>
        public List<LosslessComboboxEntity> LoadGroupCombobox(dynamic TempTable)
        {

            return dal.LoadGroupCombobox(TempTable);
        }
        #endregion

        #region 根据组选择人员
        /// <summary>
        /// 根据组选择人员
        /// </summary>
        /// <param name="TempTable">TempTable.Group = 组id</param>
        /// <returns></returns>
        public List<LosslessUserComboboxEntity> LoadPersonnelCombobox(int id)
        {

            return dal.LoadPersonnelCombobox(id);
        }
        #endregion

        #endregion

        #region 逾期时间下拉框
        /// <summary>
        /// 逾期时间下拉框
        /// </summary>
        /// <param name="Parent_id">逾期时间下拉框 字典父id</param>
        /// <returns></returns>
        public List<ComboboxEntityString> LoadDaySetting(Guid Parent_id)
        {
            return dal.LoadDaySetting(Parent_id);
        }
        #endregion

        #region 报告类型下拉框——报告模板
        /// <summary>
        /// 报告类型下拉框——报告模板
        /// </summary>
        /// <param name="Parent_id">报告类型下拉框 字典父id</param>
        /// <returns></returns>
        public List<ComboboxEntity> LoadReportType(Guid Parent_id)
        {
            return dal.LoadReportType(Parent_id);
        }
        #endregion

        #region 通用字典获取
        /// <summary>
        /// 通用字典获取  Project_value  Project_name
        /// </summary>
        /// <param name="Parent_id">字典父id</param>
        /// <returns></returns>
        public List<ComboboxEntityString> GetDictionaryListBLL(Guid Parent_id)
        {

            return dal.GetDictionaryList(Parent_id);
        }
        #endregion

        #region 通用字典获取  id  Project_name
        /// <summary>
        ///  通用字典获取  id  Project_name
        /// </summary>
        /// <param name="Parent_id">字典父id</param>
        /// <returns></returns>
        public List<ComboboxEntityString> GetDictionaryListIdBLL(Guid Parent_id)
        {

            return dal.GetDictionaryListId(Parent_id);

        }
        #endregion

        #region 通用字典获取  id　获取的字典内容
        /// <summary>
        /// 通用字典获取  id　获取的字典内容
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public TB_DictionaryManagement GetDicitionaryContentBLL(Guid ID)
        {
            return dal.GetDicitionaryContent(ID);

        }
        #endregion


        #region 加载提交CNAS人员下拉框
        /// <summary>
        /// 加载提交CNAS人员下拉框
        /// </summary>
        /// <param name="TempTable">传递信息</param>
        /// <returns></returns>
        public List<ComboboxEntity> LoadCNASPersonnelBLL(dynamic TempTable)
        {

            return dal.LoadCNASPersonnel(TempTable);
        }
        #endregion
    }
}
