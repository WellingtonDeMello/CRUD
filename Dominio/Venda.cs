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

        [Required]
        public DateTime DataHora { get; set; }

        public List<VendaProduto> VendaProdutos { get; set; }

        public Venda()
        {
            VendaProdutos = new List<VendaProduto>();
        }

        public Venda(string clienteDocumento, string clienteNome, decimal total, string obs, DateTime dataHora)
        {
            ClienteDocumento = clienteDocumento;
            ClienteNome = clienteNome;
            Total = total;
            Obs = obs;
            DataHora = dataHora;
            VendaProdutos = new List<VendaProduto>();
        }
    }
}