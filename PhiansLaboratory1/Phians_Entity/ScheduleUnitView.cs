using System.Collections.Generic;

namespace Phians_Entity
{
    public class ScheduleUnitView
    {
        public string name { get; set; }

        public string desc { get; set; }

        public List<ScheduleUnitDetail> values { get; set; }
    }

    public class ScheduleUnitDetail
    {
        public string to { get; set; }

        public string from { get; set; }

        public string desc { get; set; }

        public string label { get; set; }

        public string customClass { get; set; }

        public string dataObj { get; set; }

    }
}
