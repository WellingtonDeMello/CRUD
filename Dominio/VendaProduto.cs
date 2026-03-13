using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    public class VendaProduto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int VendaId { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoVenda { get; set; }

        [Required]
        public int Quantidade { get; set; }

        // 🔹 Total calculado automaticamente
        [NotMapped]
        public decimal Total
        {
            get { return PrecoVenda * Quantidade; }
        }

        // Relações
        public virtual Produto Produto { get; set; }
        public virtual Venda Venda { get; set; }
    }
}