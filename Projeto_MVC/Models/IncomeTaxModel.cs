namespace Projeto_MVC.Models
{
    public class IncomeTaxModel
    {
        public decimal RendaAnual { get; set; }
        public int Dependentes { get; set; }
        public decimal Deducoes { get; set; }

        public decimal CalcularImposto()
        {
            decimal aliquota = 0;

            if (RendaAnual <= 22847.76m)
                aliquota = 0; // Isento
            else if (RendaAnual <= 33919.80m)
                aliquota = 0.075m; // 7,5%
            else if (RendaAnual <= 45012.60m)
                aliquota = 0.15m; // 15%
            else if (RendaAnual <= 55976.16m)
                aliquota = 0.225m; // 22,5%
            else
                aliquota = 0.275m; // 27,5%

            decimal impostoBruto = RendaAnual * aliquota;
            decimal deducoesCalculadas = Dependentes * 2275.08m + Deducoes;

            return Math.Max(0, impostoBruto - deducoesCalculadas); // Imposto final
        }
    }

}
