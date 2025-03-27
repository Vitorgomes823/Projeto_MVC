using System.ComponentModel.DataAnnotations;

namespace Projeto_MVC.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O campo Usuário é obrigatório.")]
        public string username { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        public string password { get; set; }
        public bool rememberMe { get; set; }
    }
}
