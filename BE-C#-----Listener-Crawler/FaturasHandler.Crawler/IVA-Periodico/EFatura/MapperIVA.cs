using FaturasHandler.IoC.Dto;
using Newtonsoft.Json;

namespace FaturasHandler.Crawler.IVA_Periodico
{
    public static class MapperIVA
    {
        public static void Map(string json)
        {
            var response = JsonConvert.DeserializeObject<FaturaResponseDto>(json);

            Console.WriteLine($"Total Documents: {response.NumElementos}");
            foreach (var doc in response.Linhas)
            {
                Console.WriteLine($"Documento: {doc.NumeroDocumento}, Emitente: {doc.NomeEmitente}, Valor: {doc.ValorTotal}");
            }
        }
    }
}
