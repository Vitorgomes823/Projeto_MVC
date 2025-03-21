using System.ComponentModel.DataAnnotations;

namespace Projeto_MVC.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        [Required(ErrorMessage = "O campo Password � obrigat�rio.")]
        

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
