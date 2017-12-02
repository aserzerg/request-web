using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Highsoft.Web.Mvc.Charts;

namespace request_web.Models
{
    public class ServiceCompanyGeneralInfoModel
    {
        [Required(ErrorMessage = "Необходимо заполнить поле {0}.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "с:")]
        public DateTime FromDate { get; set; }
        [Required(ErrorMessage = "Необходимо заполнить поле {0}.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "по:")]
        public DateTime ToDate { get; set; }
        public List<string> RequestsByUsersXAxis;
        public List<Series> RequestsByUsersSeries;
        public List<string> RequestsByWorkersXAxis;
        public List<Series> RequestsByWorkersSeries;
    }
}