using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TFTEC.Web.EcommerceAdmin.Models;

namespace TFTEC.Web.EcommerceAdmin.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Produto> Produto { get; set; }

        public DbSet<ListaPreco> ListaPreco { get; set; }

        public DbSet<ItensListaPreco> ItensListaPreco { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalhe> PedidoDetalhes { get; set; }
    }
}
