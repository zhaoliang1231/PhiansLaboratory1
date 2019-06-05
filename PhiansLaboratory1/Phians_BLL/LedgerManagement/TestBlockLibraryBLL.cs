using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using PhiansCommon;
using PhiansCommon.ExcelOperate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_BLL
{
    public class TestBlockLibraryBLL
    {

        ITestBlockLibraryDAL dal = DALFactory.GeTTestBlockLibraryDAL();

        #region  试块库列表

        public List<TB_NDT_TestBlockLibrary> load_TestBlockLibrary(TPageModel PageModel, out int totalRecord)
        {
            return dal.load_TestBlockLibrary(PageModel, out  totalRecord);
        }
        #endregion

        #region  添加试块
        public ReturnDALResult Add_TestBlockLibrary(TB_NDT_TestBlockLibrary model, Guid LogPersonnel)
        {
            return dal.Add_TestBlockLibrary(model, LogPersonnel);
        }
        #endregion

        #region  修改试块
        public ReturnDALResult Edit_TestBlockLibrary(TB_NDT_TestBlockLibrary model, Guid LogPersonnel)
        {
            return dal.Edit_TestBlockLibrary(model, LogPersonnel);
        }
        #endregion

        #region  删除试块
        public ReturnDALResult Del_TestBlockLibrary(TB_NDT_TestBlockLibrary model, Guid LogPersonnel)
        {
            return dal.Del_TestBlockLibrary(model, LogPersonnel);
        }
        #endregion

        #region 批量导入试块
        public ReturnDALResult importTestBlockLibrary(string FileUrl, Guid OperatePerson)
        {
            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            try
            {
                Windows.Excel.Workbook Workbook2 = new Windows.Excel.Workbook(FileUrl);
                DataTable newDataTable2 = ExcelOperate.WorksheetToDataTable(0, 0, Workbook2, 0, true);//获取

                DataTable newDataTable = newDataTable2.Clone();//仅复制表结构


                for (int i = 0; i < newDataTable2.Rows.Count; i++)
                {
                    newDataTable.ImportRow(newDataTable2.Rows[i]);
                }
                for (int i = 0; i < newDataTable.Columns.Count; i++)
                {
                    string[] newColumns = newDataTable.Columns[i].ColumnName.Split('(');
                    newDataTable.Columns[i].ColumnName = newColumns[0];
                }
                List<TB_NDT_TestBlockLibrary> listmodel = DataTableToList.TableToList<TB_NDT_TestBlockLibrary>(newDataTable);

                ReturnDALResult = dal.importTestBlockLibrary(listmodel, OperatePerson);
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
