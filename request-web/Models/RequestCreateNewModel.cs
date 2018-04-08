using System;
using System.ComponentModel.DataAnnotations;
using request_web.Dto;
using request_web.WebService;

namespace request_web.Models
{
    public class RequestCreateNewModel
    {
        public WebUserDto UserInfo { get; set; }
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
        public string PhoneNumber { get; set; }

        [Display(Name = "Доп. фильтры")]
        public bool AdditionFiltering { get; set; }
        [Display(Name = "Для обсуждения!")]
        public bool IsBadWork { get; set; }

        [Display(Name = "Гарантийные")]
        public bool Garanty { get; set; }
        public DateTime CurrentDate { get; set; }
        public WorkerShortDto[] Workers { get; set; }
        public WorkerShortDto[] Executers { get; set; }
        public StreetShortDto[] Streets { get; set; }
        public StatusShortDto[] Statuses { get; set; }
        public HouseShortDto[] Houses { get; set; }
        public AddressShortDto[] Addresses { get; set; }
        public ServiceShortDto[] ParentServices { get; set; }
        public ServiceShortDto[] Services { get; set; }

    }
}