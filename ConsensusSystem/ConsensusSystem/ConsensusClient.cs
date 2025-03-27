using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsensusSystem
{
    public class ConsensusClient
    {
        private static readonly HttpClient _client = new HttpClient();

        public static async Task RequestConsensus(int nodeId, string[] peers)
        {
            var message = new ConsensusMessage { NodeId = nodeId, Value = $"Propuesta de nodo {nodeId}" };
            var json = JsonSerializer.Serialize(message);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            foreach (var peer in peers)
            {
                try
                {
                    var url = $"http://localhost:{5000 + int.Parse(peer)}/";
                    var response = await _client.PostAsync(url, content);
                    var responseText = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Nodo {nodeId} recibió respuesta de nodo {peer}: {responseText}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al conectar con nodo {peer}: {ex.Message}");
                }
            }
        }
    }
}
