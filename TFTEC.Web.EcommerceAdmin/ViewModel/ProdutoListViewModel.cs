using TFTEC.Web.EcommerceAdmin.Models;

namespace TFTEC.Web.EcommerceAdmin.ViewModel
{
    public class ProdutoListViewModel
    {
        public IEnumerable<Produto> Produtos { get; set; }
        public string CategoriaAtual { get; set; }
    }
}
