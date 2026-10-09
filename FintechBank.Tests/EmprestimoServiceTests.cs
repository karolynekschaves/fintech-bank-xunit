using Xunit;
using FintechBank.App;

namespace FintechBank.Tests
{
    public class EmprestimoServiceTests
    {
        private readonly EmprestimoService _emprestimoService;

        public EmprestimoServiceTests()
        {
            _emprestimoService = new EmprestimoService();
        }

        [Theory]
        [InlineData(10000.00, 10, 800, 1050.00)]
        [InlineData(10000.00, 10, 600, 1120.00)]
        [InlineData(10000.00, 10, 400, 1200.00)]
        public void CalcularValorParcela_ComJurosPorScore_DeveRetornarValorCorreto(decimal valorEmprestimo, int parcelas, int score, decimal valorParcelaEsperado)
        {
            // Act
            decimal valorCalculado = _emprestimoService.CalcularValorParcela(valorEmprestimo, parcelas, score);

            // Assert
            Assert.Equal(valorParcelaEsperado, valorCalculado);
        }

        [Theory]
        [InlineData(5000.00, 1500.00, true)]
        [InlineData(5000.00, 1600.00, false)]
        public void AprovarEmprestimo_PorMargemConsignavel_DeveRetornarResultadoEsperado(decimal renda, decimal valorParcela, bool resultadoEsperado)
        {
            // Act
            bool resultado = _emprestimoService.AprovarEmprestimo(renda, valorParcela);

            // Assert
            Assert.Equal(resultadoEsperado, resultado);
        }
    }
}