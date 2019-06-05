using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface IAbnormalApplyReviewDAL
    {

        #region 加载异常申请信息
        /// <summary>
        /// 加载异常申请信息
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="PageModel">页面传参</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>返回项目信息实体集</returns>
        List<TB_NDT_error_Certificate> LoadUnusualCertificateList(TPageModel PageModel, out int totalRecord);

        #endregion

        #region 申请通过审核
        /// <summary>
        /// 申请通过审核
        /// </summary>
        /// <param name="Model">异常流程记录表</param>
        /// <param name="MTRNO">MTR单号</param>
        /// <param name="AcceptState">异常申请状态</param>
        /// <param name="currentState">当前任务状态</param>
        /// <returns></returns>
        ReturnDALResult PassUnusualApply(TB_NDT_error_Certificate Model, TB_ProcessRecord TB_ProcessRecord, int type, int flag, Guid Operator, int state_);

        #endregion

    }
}
