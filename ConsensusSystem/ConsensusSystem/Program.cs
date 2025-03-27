using ConsensusSystem;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Iniciando el sistema de consenso distribuido...");

        // Crear nodos simulando un sistema distribuido
        var node1 = new ConsensusNode(1);
        var node2 = new ConsensusNode(2);
        var node3 = new ConsensusNode(3);

        // Establecer conexiones entre nodos
        node1.AddNeighbor(node2);
        node1.AddNeighbor(node3);
        node2.AddNeighbor(node1);
        node2.AddNeighbor(node3);
        node3.AddNeighbor(node1);
        node3.AddNeighbor(node2);

        // Simular propuestas de estado
        node1.ProposeState(10);
        node2.ProposeState(20);
        node3.ProposeState(30);

        // Simular una partición de red donde node1 queda aislado
        node3.SimulatePartition(new List<ConsensusNode> { node1 });

        // Esperar un momento para que el consenso se establezca
        await Task.Delay(2000);

        // Recuperar logs de cada nodo
        Console.WriteLine("\n--- Logs de los nodos ---");
        Console.WriteLine($"Nodo 1: {node1.GetLogs()}");
        Console.WriteLine($"Nodo 2: {node2.GetLogs()}");
        Console.WriteLine($"Nodo 3: {node3.GetLogs()}");

        Console.WriteLine("\nSistema finalizado.");
    }
}
