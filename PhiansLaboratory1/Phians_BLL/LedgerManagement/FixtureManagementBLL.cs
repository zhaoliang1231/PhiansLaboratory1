using Phans_DAL_DALFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phians_Entity;
using Phans_DAL_INTERFACE.LedgerManagement;
using System.IO;
using System.Data;
using PhiansCommon.ExcelOperate;
using PhiansCommon;
using Phians_Entity.Common;
using Phans_DAL_INTERFACE;

namespace Phians_BLL
{
    public class FixtureManagementBLL
    {
        IFixtureManagementDAL dal = DALFactory.GetFixtureManagement();

        #region  夹具设备列表
        public List<TB_NDT_probe_library> GetProbeList(TPageModel PageModel, out int totalRecord)
        {
            return dal.GetProbeList(PageModel, out  totalRecord);
        }
        #endregion

        #region  添加探头
        public ReturnDALResult Probe_add(TB_NDT_probe_library model,Guid LogPersonnel)
        {
            return dal.Probe_add(model,LogPersonnel);
        }
        #endregion

        #region 修改探头
        public ReturnDALResult Probe_edit(TB_NDT_probe_library model, Guid LogPersonnel)
        {

            return dal.Probe_edit(model, LogPersonnel);
        }
        #endregion

        #region 删除探头
        /// <summary>
        /// 删除夹具
        /// </summary>
        /// <param name="Id">夹具id</param>
        /// <param name="LogPersonnel">操作人</param>
        /// <param name="FixtureName">夹具名称</param>
        /// <param name="Address">仓库位置</param>
        /// <returns></returns>
        public ReturnDALResult Probe_delete(TB_NDT_probe_library model, Guid LogPersonnel)
        {
            return dal.Probe_delete(model, LogPersonnel);
        }
        #endregion

        #region 批量导入探头
        public ReturnDALResult importProbe(string FileUrl, Guid OperatePerson)
        {
            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            try
            {
                Windows.Excel.Workbook Workbook2 = new Windows.Excel.Workbook(FileUrl);
                DataTable newDataTable2 = ExcelOperate.WorksheetToDataTable(0, 0, Workbook2, 0, true);//获取

                DataTable newDataTable = newDataTable2.Clone();//仅复制表结构
                //newDataTable.Columns["DueDate"].DataType = Type.GetType("System.String");
                //newDataTable.Columns["CalibrationDate"].DataType = Type.GetType("System.String");
                //newDataTable.Columns["DateOfLaunch"].DataType = Type.GetType("System.String");

                for (int i = 0; i < newDataTable2.Rows.Count; i++)
                {
                    newDataTable.ImportRow(newDataTable2.Rows[i]);
                }
                for (int i = 0; i < newDataTable.Columns.Count; i++)
                {
                    string[] newColumns = newDataTable.Columns[i].ColumnName.Split('(');
                    newDataTable.Columns[i].ColumnName = newColumns[0];
                }
                //List<TB_UserInfo> UserInfoList = dal.GetAllUserInfoList();
                List<TB_NDT_probe_library> listmodel = DataTableToList.TableToList<TB_NDT_probe_library>(newDataTable);
                //List<TB_EquipmentInfo> TB_EquipmentInfo =new List<TB_EquipmentInfo>();
                //for (int i = 0; i < listmodel.Count; i++)
                //{
                //    int ManagerCount = UserInfoList.FindIndex(A => A.JobNum == listmodel[i].Manager);
                //    int OwnerEquipmentCount = UserInfoList.FindIndex(A => A.JobNum == listmodel[i].OwnerEquipment.ToString());

                //    if (ManagerCount != -1)
                //    {
                //        listmodel[i].Manager = UserInfoList[ManagerCount].UserId.ToString();
                //    }
                //    else
                //    {
                //        listmodel[i].Manager = null;
                //    }
                //    if (OwnerEquipmentCount != -1)
                //    {
                //        listmodel[i].OwnerEquipment = UserInfoList[OwnerEquipmentCount].UserId.ToString();
                //    }
                //    else
                //    {
                //        listmodel[i].OwnerEquipment = null;
                //    }
                //    设备状态
                //    string Status = listmodel[i].Status.Trim();
                //    switch (Status)
                //    {

                //        case "检定合格": listmodel[i].Status = "1"; break;
                //        case "搁置": listmodel[i].Status = "2"; break;
                //        case "禁用": listmodel[i].Status = "3"; break;
                //        case "限用": listmodel[i].Status = "4"; break;
                //        case "参考": listmodel[i].Status = "5"; break;
                //        default: listmodel[i].Status = "0";
                //            break;
                //    }
                //    设备警告时间(不是数字就设置成默认时间23天)
                //    if (!System.Text.RegularExpressions.Regex.IsMatch(listmodel[i].WarningDays, @"^\d+$"))
                //    {
                //        listmodel[i].WarningDays = "21";
                //    }
                //}
                ReturnDALResult = dal.importProbe(listmodel, OperatePerson);
            }
            catch (Exception e)
            {
                throw;
            }
            return ReturnDALResult;
        }
        #endregion
    }
}
