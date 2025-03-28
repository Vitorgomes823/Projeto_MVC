using System.ComponentModel.DataAnnotations;

namespace Projeto_MVC.Models
{
    public class LoginModel
    {
        private bool rememberMe1 = false;
        public bool rememberMe { get => rememberMe1; set => rememberMe1 = value; }

        [Required(ErrorMessage = "O campo Usuário é obrigatório.")]
        public string username { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        public string password { get; set; }
    }
}
