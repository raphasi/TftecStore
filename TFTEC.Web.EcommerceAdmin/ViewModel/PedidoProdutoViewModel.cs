using TFTEC.Web.EcommerceAdmin.Models;

namespace TFTEC.Web.EcommerceAdmin.ViewModel
{
    public class PedidoProdutoViewModel
    {
        public Pedido Pedido { get; set; }
        public IEnumerable<PedidoDetalhe> PedidoDetalhes { get; set; }
    }
}
