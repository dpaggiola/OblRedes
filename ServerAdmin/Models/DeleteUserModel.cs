using System.ComponentModel.DataAnnotations;

namespace ServerAdmin.Models
{
    public class DeleteUserModel
    {
        [Required]
        public string Username { get; set; }
     
    }
}