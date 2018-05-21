using System.ComponentModel.DataAnnotations;

namespace SharedLib
{
    public class Account
    {
        [Required]
        public int AccountID { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public int Balance { get; set; }
    }
}