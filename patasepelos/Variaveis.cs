using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace patasepelos
{
    internal class Variaveis
    {
        //Estrutua de Fotos FTP
        public static string enderecoServidorFtp = "ftp://127.0.0.1/admin/";
        public static string usuarioFtp = "patasepelos";
        public static string senhaFtp = "123456789";
        //Fim Estruturra FTP

        //Geral
        public static string funcao;
        public static int linhaSelecionada;

        //Menu
        public static int tentativa;
        public static string usuario, especialidade, senha;

        //Cliente
        public static int idCliente;
        public static string nomeCliente, enderecoCliente, telefoneCliente, emailCliente, senhaCliente, fotoCliente, statusCliente, altCliente, altFotoCliente;
        public static DateTime datCadCliente;

        //Produto

        public static int idProduto;
        public static string nomeProduto, valorProduto, qtdeProduto, barrasProduto, statusatFotoProduto, caminhoFotoProduto, fotoProduto, statusProduto, altFotoProduto;
        public static DateTime dataValProduto;

        //Marca
        public static int codMarca;


    }
}
