using CoteAqui.Api;
using SkiaSharp;
using System.ComponentModel;
using System.Windows.Input;
using Microcharts;
using Cota_aqui.Model;

namespace CoteAqui.ViewModel
{
    public class CotacaoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CarregarCotacoesCommand { get; private set; }
        public ICommand CarregarMoedaCommand { get; private set; }

        private Chart graficoCotacao;

        private string moedaSelecionada = "USD-BRL"; // dolar como padrao
        private string moedaVisualizacao = string.Empty;

        private bool stackPaisesVisivel = false;

        private double _graficoWidth;

        public double GraficoWidth
        {
            get => _graficoWidth;
            set
            {
                _graficoWidth = value;
                OnPropertyChanged(nameof(GraficoWidth));
            }
        }

        public Chart GraficoCotacao
        {
            get => graficoCotacao;
            private set
            {
                graficoCotacao = value;
                OnPropertyChanged(nameof(GraficoCotacao));
            }
        }

        public string ValorMoedaSelecionada
        {
            get => moedaVisualizacao;
            set
            {
                moedaVisualizacao = value;
                OnPropertyChanged(nameof(ValorMoedaSelecionada));
            }
        }

        public CotacaoViewModel()
        {
            CarregarCotacoesCommand = new Command<string>(async (dias) => await carregarCotacaoDiaria(dias));
            CarregarMoedaCommand = new Command<string>(async (moeda) => await selecionaMoeda(moeda));
        }

        private async Task carregarCotacaoDiaria(string dias = "5")
        {
            CotacaoService cotacaoService = new CotacaoService();

            var jsonResponse = await cotacaoService.GetCotacaoEmDias(moedaSelecionada, dias);

            carregarGrafico(jsonResponse);
            detalhaMoeda(jsonResponse);
        }

        private void carregarGrafico(List<Cotacao> lstCotacoesResponse)
        {
            defineTamanhoGrafico(lstCotacoesResponse.Count); // total de dias

            var entradas = new List<Microcharts.ChartEntry>();

            foreach (Cotacao item in lstCotacoesResponse.DistinctBy(o => DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(o.Timestamp))
                                                  .ToUniversalTime()
                                                  .ToString("yyyy-MM-dd"))
                                                  .OrderBy(o => o.Timestamp))
            {
                string timestamp = item.Timestamp;

                DateTimeOffset datetime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(timestamp));

                Cotacao cotacao = item;

                entradas.Add(new Microcharts.ChartEntry((float)cotacao.Ask)
                {
                    Label = datetime.ToString("dd/MM"),
                    ValueLabelColor = SKColor.Parse("#2ECC71"),
                    ValueLabel = "R$" + cotacao.Ask.ToString("N2"),
                    Color = SKColor.Parse("#2ECC71")
                });
            }

            GraficoCotacao = new LineChart
            {
                Entries = entradas,
                LabelTextSize = 30,
                ValueLabelTextSize = 25,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                BackgroundColor = SKColor.Parse("#1C1C1C"),
                IsAnimated = true,
                ShowYAxisLines = false,
                LineMode = LineMode.Spline,
                PointSize = 20,
                MinValue = entradas.Min(o => o.Value.Value) * 0.98f,
                MaxValue = entradas.Max(o => o.Value.Value) * 1.02f
            };

            OnPropertyChanged(nameof(GraficoCotacao));
        }

        private void detalhaMoeda(List<Cotacao> lstCotacoesResponse)
        {
            ValorMoedaSelecionada = lstCotacoesResponse[0].Code + " R$" + lstCotacoesResponse[0].Ask.ToString("N2");

            OnPropertyChanged(nameof(ValorMoedaSelecionada));
        }

        private async Task selecionaMoeda(string moeda)
        {
            moedaSelecionada = moeda;

            await carregarCotacaoDiaria();
        }

        private void defineTamanhoGrafico(int dias)
        {
            switch (dias)
            {
                case 5:
                    GraficoWidth = 500;
                    break;
                case 15:
                    GraficoWidth = 2000;
                    break;
                case 30:
                    GraficoWidth = 3500;
                    break;
                default:
                    GraficoWidth = 500;
                    break;
            }

            OnPropertyChanged(nameof(GraficoCotacao));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
