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
        private string moedaVisualizacao = "dólar";

        private bool stackPaisesVisivel = false;

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

        private async Task carregarCotacaoDiaria(string dias = "1")
        {
            CotacaoService cotacaoService = new CotacaoService();

            var jsonResponse = await cotacaoService.GetCotacaoEmDias(moedaSelecionada, dias);

            var entradas = new List<Microcharts.ChartEntry>();

            int diasReduzir = 0;

            foreach (Cotacao item in jsonResponse)
            {
                Cotacao cotacao = item;

                DateTime hoje = DateTime.Now;

                string dataFormatada = hoje.AddDays(-diasReduzir).ToString("dd/MM");

                entradas.Add(new Microcharts.ChartEntry((float)cotacao.Bid)
                {
                    Label = dataFormatada,
                    ValueLabelColor = SKColor.Parse("#2ECC71"),
                    ValueLabel = cotacao.Bid.ToString("N2"),
                    Color = SKColor.Parse("#2ECC71")
                });

                diasReduzir++;
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
                MinValue = 0,
                MaxValue = 10
            };

            OnPropertyChanged(nameof(GraficoCotacao));
        }

        private async Task selecionaMoeda(string moeda)
        {
            moedaSelecionada = moeda;

            await carregarCotacaoDiaria();

            ValorMoedaSelecionada = formataMoeda(moedaSelecionada);
        }

        private string formataMoeda(string codigoMoeda)
        {
            string moedaFormatada = string.Empty;

            switch (codigoMoeda)
            {
                case ("USD-BRL"):
                    moedaFormatada = "dólar";
                    break;
                case ("GBP-BRL"):
                    moedaFormatada = "libra";
                    break;
                case ("JPY-BRL"):
                    moedaFormatada = "iene";
                    break;
                case ("CHF-BRL"):
                    moedaFormatada = "franco suíço";
                    break;
                case ("BTC-BRL"):
                    moedaFormatada = "bitcoin";
                    break;
            }

            return moedaFormatada;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
