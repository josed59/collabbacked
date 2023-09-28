using collabBackend.Filters;
using System.ComponentModel.DataAnnotations;

namespace collabBackend.Models
{
    public class updateTaskDTO
    {
        [Required(ErrorMessage = "TaskId is required.")]
        public Guid TaskId { get; set; }
        public string? Description { get; set; }

        [RequiredIf("To", null, ErrorMessage = "From date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public  DateTime? From { get; set; }

        [RequiredIf("From", null, ErrorMessage = "To date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DateGreaterThan("From", ErrorMessage = "To date must be greater or equal From date.")]
        public DateTime? To { get; set; }

        [Required(ErrorMessage = "Comment is required.")]
        public string Comment { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "TaskSizeId must be an integer.")]
        public int? Size { get; set; }
        public bool? isCompleted { get; set; }
        public bool? isUatFinished { get; set; }
        public bool? isStandBy { get; set; }
        public bool? userTest { get; set; }

        [RequiredIfMultiple(new[] { "isUatFinished", "isCompleted" }, true, ErrorMessage = "updateDate is required.")]
        [DataType(DataType.Date)]
        public DateTime? updateDate { get; set; }

        public bool? isDeleted { get; set; }


    }
}
