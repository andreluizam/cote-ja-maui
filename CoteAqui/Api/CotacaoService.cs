using Cota_aqui.Model;
using Newtonsoft.Json;
using RestSharp;

namespace CoteAqui.Api
{
    public class CotacaoService
    {
        public async Task<List<Cotacao>> GetCotacaoEmDias(string moeda, string dias)
        {
            var options = new RestClientOptions("https://economia.awesomeapi.com.br");

            var client = new RestClient(options);

            var request = new RestRequest($"/json/daily/{moeda}/{dias}", Method.Get);

            try
            {

                RestResponse response = await client.ExecuteAsync(request);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("erro ao buscar cotação" + response.ErrorMessage);


                var jsonResponse = JsonConvert.DeserializeObject<List<Cotacao>>(response.Content);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Dictionary<string,Cotacao>> GetCotacaoAtual(string moeda)
        {
            var options = new RestClientOptions("https://economia.awesomeapi.com.br");

            var client = new RestClient(options);

            var request = new RestRequest($"/last/{moeda}", Method.Get);

            try
            {

                RestResponse response = await client.ExecuteAsync(request);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("erro ao buscar cotação" + response.ErrorMessage);


                var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string,Cotacao>>(response.Content);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
