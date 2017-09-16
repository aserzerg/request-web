using System.Collections.Generic;
using Highsoft.Web.Mvc.Charts;

namespace request_web.Models
{
    public class GeneralInfoModel
    {
        public List<string> RequestsByUsersXAxis;
        public List<Series> RequestsByUsersSeries;
        public List<string> RequestsByWorkersXAxis;
        public List<Series> RequestsByWorkersSeries;
    }
}