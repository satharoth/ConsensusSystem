using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;


namespace ConsensusSystem
{
    public class ConsensusNode
    {
        public int Id { get; private set; }
        private List<ConsensusNode> Neighbors;
        private int ProposedState;
        private bool IsIsolated;
        private List<string> Logs;

        public ConsensusNode(int id)
        {
            Id = id;
            Neighbors = new List<ConsensusNode>();
            Logs = new List<string>();
            IsIsolated = false;
        }

        // Agregar un nodo vecino para la comunicación
        public void AddNeighbor(ConsensusNode node)
        {
            Neighbors.Add(node);
        }

        // Proponer un estado inicial para este nodo
        public void ProposeState(int state)
        {
            ProposedState = state;
            Logs.Add($"Nodo {Id} propone estado: {state}");
            BroadcastState();
        }

        // Enviar el estado a los vecinos (simulación de consenso)
        private void BroadcastState()
        {
            if (IsIsolated)
            {
                Logs.Add($"Nodo {Id} está aislado y no puede comunicarse.");
                return;
            }

            foreach (var neighbor in Neighbors)
            {
                neighbor.ReceiveState(ProposedState, this);
            }
        }

        // Recibir un estado de otro nodo
        public void ReceiveState(int state, ConsensusNode sender)
        {
            if (IsIsolated)
                return;

            Logs.Add($"Nodo {Id} recibió estado {state} de nodo {sender.Id}");

            // Aquí podrías agregar la lógica para decidir el consenso
            ProposedState = (ProposedState + state) / 2;
        }

        // Simular una partición de red
        public void SimulatePartition(List<ConsensusNode> isolatedNodes)
        {
            if (isolatedNodes.Contains(this))
            {
                IsIsolated = true;
                Logs.Add($"Nodo {Id} está aislado.");
            }
        }

        // Obtener los logs del nodo
        public string GetLogs()
        {
            return string.Join("\n", Logs);
        }
    }
}
