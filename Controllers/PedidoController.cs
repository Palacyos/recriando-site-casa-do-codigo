using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {
        private IProdutoRepository ProdutoRepository { get; }
        private IPedidoRepository PedidoRepository { get; }
        private IItemPedidoRepository ItemPedidoRepository { get; }

        public PedidoController(IProdutoRepository produtoRepository, IPedidoRepository pedidoRepository,
                                IItemPedidoRepository itemPedidoRepository)
        {
            ProdutoRepository = produtoRepository;
            PedidoRepository = pedidoRepository;
            ItemPedidoRepository = itemPedidoRepository;

        }

        public IActionResult Carrossel()
        {

            return View(ProdutoRepository.GetProdutos());
        }

        public IActionResult Carrinho(string codigo)
        {

            if (!string.IsNullOrEmpty(codigo))
            {
                PedidoRepository.AddItem(codigo);
            }
            List<ItemPedido> itens = PedidoRepository.GetPedido().Itens;

            CarrinhoViewModel carrinhoViewModel = new CarrinhoViewModel(itens); 

            return View(carrinhoViewModel);
        }

        public IActionResult Cadastro()
        {

            var pedido = PedidoRepository.GetPedido();

            if (pedido == null)
            {
                return RedirectToAction("Carrossel");
            }

            return View(pedido.Cadastro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Resumo(Cadastro cadastro)
        {
            if (ModelState.IsValid)
            {
                Pedido pedido = PedidoRepository.UpdateCadastro(cadastro);
                return View(pedido);

            }

            return RedirectToAction("Cadastro");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public UpdateQuantidadeResponse UpdateQuantidade([FromBody]ItemPedido itemPedido) 
        {
            return PedidoRepository.UpdateQuantidade(itemPedido); 


        }
    }
}
