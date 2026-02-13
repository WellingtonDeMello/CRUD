using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    public class Venda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string ClienteDocumento { get; set; }

        [Column(TypeName = "nvarchar(40)")]
        public string ClienteNome { get; set; }

        [MaxLength(150)]
        public string Obs { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Total { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        [Required]
        public DateTime DataHora { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoVenda { get; set; }

        public List<VendaProduto> VendaProdutos { get; set; }

        public Venda()
        {

        }

        public Venda(string clienteDocumento, string clienteNome, decimal total, string obs, DateTime dataHora, decimal precovenda = 0)
        {
            ClienteDocumento = clienteDocumento;
            ClienteNome = clienteNome;
            Total = total;
            Obs = obs;
            DataHora = dataHora;
            PrecoVenda = precovenda;
        }
    }
}
