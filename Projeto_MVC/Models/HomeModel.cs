namespace Projeto_MVC.Models
{
    public class HomeModel
    {

        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserCPF { get; set; } //alterado de int pra string, por mais que sejam apenas números, geralmente usa-se int apenas quando é feito calculos. 
    }
}
