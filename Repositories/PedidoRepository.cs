using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {

        private IHttpContextAccessor ContextAccessor { get; }
        private IItemPedidoRepository ItemPedidoRepository { get; }
        private ICadastroRepository CadastroRepository { get; }



        public PedidoRepository(ApplicationContext context, IHttpContextAccessor contextAccessor,
                                IItemPedidoRepository itemPedidoRepository, ICadastroRepository cadastroRepository) : base(context)
        {
            ContextAccessor = contextAccessor;
            ItemPedidoRepository = itemPedidoRepository;
            CadastroRepository = cadastroRepository;
        }

        private int? GetPedidoId()
        {
          return ContextAccessor.HttpContext.Session.GetInt32("pedidoId");
        }

        private void SetPedidoId(int pedidoId)
        {
            ContextAccessor.HttpContext.Session.SetInt32("pedidoId", pedidoId);
        }

        public Pedido GetPedido()
        {
            var pedidoId = GetPedidoId();
            var pedido = Db
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)

                .Include(p => p.Cadastro)
                .Where(p => p.Id == pedidoId)
                .SingleOrDefault();

            if (pedido == null)
            {
                pedido = new Pedido();
                Db.Add(pedido);
                Context.SaveChanges();
                SetPedidoId(pedido.Id);
            }

            return pedido;
        }

        public void AddItem(string codigo)
        {
            var produto = Context.Set<Produto>().Where(p => p.Codigo == codigo).SingleOrDefault();

            if (produto == null)
            {
                throw new ArgumentException("Produto não encontrado.");
            }

            var pedido = GetPedido();

            var itemPedido = Context.Set<ItemPedido>().Where(i => i.Produto.Codigo == codigo
                                                              && i.Pedido.Id == pedido.Id).SingleOrDefault();

            if (itemPedido == null)
            {
                itemPedido = new ItemPedido(pedido, produto, 1, produto.Preco);
                Context.Set<ItemPedido>().Add(itemPedido);
                Context.SaveChanges();

            }
        }

        public UpdateQuantidadeResponse UpdateQuantidade(ItemPedido itemPedido)
        {
            var itemPedidoDb = ItemPedidoRepository.GetItemPedido(itemPedido.Id);

            if (itemPedidoDb != null)
            {
                itemPedidoDb.AtualizaQuantidade(itemPedido.Quantidade);

                if (itemPedido.Quantidade == 0)
                {
                    ItemPedidoRepository.RemoveItemPedido(itemPedido.Id);
                }

                Context.SaveChanges();

                var carrinhoVielModel = new CarrinhoViewModel(GetPedido().Itens);

                return new UpdateQuantidadeResponse(itemPedidoDb, carrinhoVielModel);
            }

            throw new ArgumentException("Item não encontrado.");
        }

        public Pedido UpdateCadastro(Cadastro cadastro)
        {
            var pedido = GetPedido();
            CadastroRepository.Update(pedido.Cadastro.Id, cadastro);
            return pedido;
        }
    }
}
