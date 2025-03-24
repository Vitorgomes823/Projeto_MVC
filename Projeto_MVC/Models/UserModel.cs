namespace Projeto_MVC.Models
{
    public class UserModel
    {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string FullName { get; set; }
            public string CPF { get; set; }
            public DateTime BirthDate { get; set; }
    }
}
