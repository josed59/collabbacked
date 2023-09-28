using System;
using System.ComponentModel.DataAnnotations;
using collabBackend.Filters;

namespace collabBackend.Models
{
    public class InsertTaskDTO
    {
        private DateTime _from;
        private DateTime _to;

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(50, ErrorMessage = "Title must not exceed 50 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "From date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime From { get; set; }

        [Required(ErrorMessage = "To date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [DateGreaterThan("From", ErrorMessage = "To date must be greater than From date.")]
        public DateTime To { get; set; }

        [Required(ErrorMessage = "UserTest is required.")]
        public bool UserTest { get; set; }

        [Required(ErrorMessage = "TaskSizeId is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "TaskSizeId must be an integer.")]
        public int TaskSizeId { get; set; }
    }
}
