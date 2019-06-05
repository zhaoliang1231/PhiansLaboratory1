using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface IDictionaryManagementDAL
    {
        List<TB_DictionaryManagement> LoadDicitionaryData(int pageIndex, int pageSize, out int totalRecord, Guid nodeid, string search, string key);

      List<TB_DictionaryManagement> LoadPageTree(Guid Parent_id);

      bool AddPageTree(TB_DictionaryManagement model,Guid UserId,out Guid NodeId);

      bool DelDicitionaryData(TB_DictionaryManagement model, Guid UserId);
      bool EditDictionary(TB_DictionaryManagement model, Guid UserId, out int errorType);

      bool EditDictionaryState(TB_DictionaryManagement model, Guid UserId);
    }
}
