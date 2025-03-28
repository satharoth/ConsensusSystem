# Simulador de Consenso Distribuido

Este proyecto implementa un sistema distribuido de consenso basado en múltiples nodos que se comunican a través de HTTP para intercambiar mensajes y alcanzar un estado compartido.

## 📂 Estructura del Proyecto

- `ConsensusNode.cs` → Servidor HTTP que representa un nodo en el sistema distribuido.
- `ConsensusMessage.cs` → Modelo de datos para los mensajes intercambiados entre nodos.
- `ConsensusClient.cs` → Cliente HTTP que permite enviar mensajes a otros nodos.
- `appsettings.json` → Configuración del nodo (puerto, direcciones de otros nodos, etc.).
- `Program.cs` → Punto de entrada de la aplicación.

## 🚀 Instalación y Ejecución

### 1️⃣ Requisitos Previos

- .NET 6 o superior instalado en el sistema.
- Docker (opcional, para ejecutar los nodos en contenedores).

### 2️⃣ Clonar el Repositorio

```sh
git clone https://github.com/satharoth/ConsensusSystem.git
cd ConsensusSystem
```

### 3️⃣ Ejecutar el Proyecto en Modo Local

```sh
dotnet run --project ConsensusNode
```

Cada nodo debe ejecutarse en un puerto diferente. Para ello, puedes modificar el archivo `appsettings.json` o pasar el puerto como argumento:

```sh
dotnet run --project ConsensusNode --urls=http://localhost:5001
```

### 4️⃣ Simulación de Mensajes entre Nodos

Puedes enviar mensajes entre nodos con `ConsensusClient`. Aquí un ejemplo de cómo hacerlo:

```csharp
var client = new ConsensusClient("http://localhost:5001");
await client.SendMessageAsync(new ConsensusMessage { SenderId = 1, Content = "Propuesta de estado" });
```

### 5️⃣ Ejecución con Docker

Puedes ejecutar múltiples nodos usando Docker con el siguiente comando:

```sh
docker-compose up --scale node=3
```

Esto iniciará 3 nodos simulando un sistema distribuido.

## 🛠️ Pruebas

El código implementa pruebas unitarias en XUnit para verificar la mensajería en un sistema de consenso distribuido. Se usa HttpClient para enviar mensajes HTTP entre nodos simulados. A continuación, se explica cada sección del código:

### 1️⃣ Configuración General

private readonly HttpClient _httpClient = new HttpClient();
Se crea una instancia de HttpClient, que se usará para enviar solicitudes HTTP a los nodos del sistema.

 ### 2️⃣ Prueba: SendMessage_ShouldReceiveMessage

[Fact]
public async Task SendMessage_ShouldReceiveMessage()
Propósito: Verificar que un nodo puede recibir un mensaje de otro nodo correctamente.

### 🔹 Pasos de la prueba:
Definir URLs de los nodos

```csharp
var senderNodeUrl = "http://localhost:5001";
var receiverNodeUrl = "http://localhost:5002";
Se establece el nodo emisor (5001) y el nodo receptor (5002).
```
Crear el mensaje en formato JSON

```csharp
var message = new { SenderId = 1, ReceiverId = 2, Content = "Test Message" };
var jsonContent = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
```
Se crea un mensaje con SenderId, ReceiverId y Content.

Se serializa el mensaje en JSON para enviarlo en la solicitud HTTP.

Enviar el mensaje al nodo receptor

```csharp
var response = await _httpClient.PostAsync($"{receiverNodeUrl}/receive", jsonContent);
```
Se hace una solicitud HTTP POST al endpoint /receive del nodo receptor.

Verificar la respuesta

```csharp
response.EnsureSuccessStatusCode();
var responseContent = await response.Content.ReadAsStringAsync();
Assert.Contains("Message received", responseContent);
```
Se asegura que la respuesta tenga un código 200 OK.

Se verifica que la respuesta contenga "Message received", lo que confirma que el nodo receptor procesó el mensaje correctamente.

### 3️⃣ Prueba: BroadcastMessage_ShouldBeReceivedByAllNodes
```csharp
[Fact]
public async Task BroadcastMessage_ShouldBeReceivedByAllNodes()
```
Propósito: Comprobar que un mensaje enviado a múltiples nodos es recibido por todos.

🔹 Pasos de la prueba:
Definir el nodo emisor y los nodos receptores

```csharp
var senderNodeUrl = "http://localhost:5001";
var nodes = new[] { "http://localhost:5002", "http://localhost:5003" };
```
Se define un nodo emisor (5001).

Se crean múltiples nodos receptores (5002 y 5003).

Crear el mensaje a enviar

```csharp
var message = new { SenderId = 1, Content = "Broadcast Test" };
var jsonContent = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
```
Se crea un mensaje de prueba en formato JSON.

Enviar el mensaje a todos los nodos y validar la respuesta
```csharp

foreach (var node in nodes)
{
    var response = await _httpClient.PostAsync($"{node}/receive", jsonContent);
    response.EnsureSuccessStatusCode();
    var responseContent = await response.Content.ReadAsStringAsync();
    Assert.Contains("Message received", responseContent);
}
```
Se itera sobre la lista de nodos.

Se envía la solicitud HTTP POST a cada nodo.

Se verifica que cada nodo responda con "Message received", asegurando que todos recibieron el mensaje.

## 📌 Notas

- El sistema es capaz de manejar fallos y particiones de red.
- Puedes extender la lógica de consenso para mejorar la toma de decisiones entre nodos.

---

📧 **Contacto**: Si tienes preguntas o sugerencias, abre un issue en el repositorio de GitHub.

🚀 ¡Feliz codificación!

