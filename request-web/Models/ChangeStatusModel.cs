using System.Collections.Generic;
using System.Web.Mvc;
using request_web.Dto;

namespace request_web.Models
{
    public class ChangeStatusModel
    {
        public int RequestId { get; set; }
        public int CurrentStatus { get; set; }
        public string Status { get; set; }
        public StatusShortDto[] Statuses { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
    }
}