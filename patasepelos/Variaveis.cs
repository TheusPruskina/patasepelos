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

        //Produto
        public static int idProduto;
        public static double valorProduto, qtdeProduto;
        public static string nomeProduto, statusatFotoProduto, barrasProduto,  caminhoFotoProduto, fotoProduto, statusProduto, altFotoProduto;
        public static DateTime dataValProduto;
      

        //Funcionario
        public static int idFuncionario;
        public static string nomeFuncionario, enderecoFuncionario, caminhoFotoFuncionario, emailFuncionario, senhaFuncionario, fotoFuncionario, altFuncionario, statusFuncionario, dataaltFuncionario, especialidadeFuncionario, altFotoFuncionario, descFuncionario;
        public static DateTime dataFuncionario;
        public static double telefoneFuncionario;

        //Servico
        public static int idServico;
        public static string nomeServico, fotoServico, altServico, descricaoServico, statusServico, caminhoFotoServico, altFotoServico;

        //Marca
        public static int codMarca;


    }
}
