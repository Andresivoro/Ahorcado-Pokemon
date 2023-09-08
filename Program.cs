// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

var client = new HttpClient();
var request = new HttpRequestMessage
{
    Method = HttpMethod.Get,
    //RequestUri = new Uri("https://pokedex2.p.rapidapi.com/pokedex/usa/pikachu"),
    /*Headers =
    {
        { "X-RapidAPI-Key", "8b612b5078msheef33a9277fb744p1dbfddjsne0206765bd88" },
        { "X-RapidAPI-Host", "pokedex2.p.rapidapi.com" },
    },*/
    RequestUri = new Uri("https://pokeapi.co/api/v2/pokemon?limit=100000&offset=0")
};
Random random = new Random();
string pokemonAleatorio, palabraIncognita = "";
string cuerpo = "";
string piernas = "";
int errores = 0;
using (var response = await client.SendAsync(request))
{
    response.EnsureSuccessStatusCode();
    string body = await response.Content.ReadAsStringAsync();
    JObject jsonObject = JsonConvert.DeserializeObject<JObject>(body);

    JArray resultsArray = (JArray)jsonObject["results"];

    OcultarPalabra(resultsArray);
    InstruccionesJuego(resultsArray);
    ImprimirAhorcado();
    VerificarInputJugador();
}
void VerificarInputJugador()
{
    Console.WriteLine("Ingresa una letra");
    string inputJugador = Console.ReadLine();
    if(inputJugador.Length > 1)
    {
        Console.WriteLine("Dato incorrecto, reintentalo...");
        VerificarInputJugador();
    }
    ComprobarLetra(inputJugador);
}
void ComprobarLetra(string input)
{
    bool letraCorrecta = false;
    string nuevaPalabraIncognita = "";
    for(int i = 0; i < pokemonAleatorio.Length; i++)
    {
        if (pokemonAleatorio[i].ToString() == input)
        {
            letraCorrecta = true;
            nuevaPalabraIncognita += input;
        }
        else
        {
            nuevaPalabraIncognita += palabraIncognita[i].ToString();
        }
    }
    palabraIncognita = nuevaPalabraIncognita;
    if (errores < 4)
    {
        bool victoria = CondicionGanar(nuevaPalabraIncognita);
        if (!victoria)
        {
            if (!letraCorrecta)
            {
                Console.WriteLine("Letra incorrecta, reintentalo...");
                errores++;
                ImprimirAhorcado();
                VerificarInputJugador();
            }
            else
            {
                ImprimirAhorcado();
                VerificarInputJugador();
            }
        }
        else
        {
            Console.WriteLine("Ganaste, felicidades...");
        }
    }
    else
    {
        Console.WriteLine("Perdiste...");
        Console.ReadKey();
    }
    
}
bool CondicionGanar(string palabra)
{
    int letrasAcertadas = 0;
    for(int i = 0; i<palabra.Length; i++)
    {
        if (palabra[i].ToString() != "_")
        {
            letrasAcertadas++;
        }
    }
    if (letrasAcertadas < palabra.Length)
    {
        return false;
    }
    else
    {
        return true;
    }
}
void ImprimirAhorcado()
{
    Console.Clear();
    Console.WriteLine("╔═══");
    Console.WriteLine("║   |");
    if(errores>0)  
    {
        Console.WriteLine("║   O");
        if (errores == 1) cuerpo += "/";
        if (errores == 2) cuerpo += "|";
        if (errores == 3) cuerpo += "/";
        if (errores == 4) piernas += "/";
        if (errores == 5) piernas += "/";
        Console.WriteLine("║  "+cuerpo);
        Console.WriteLine("║   "+piernas);
    }
    Console.WriteLine(pokemonAleatorio);
    Console.WriteLine(palabraIncognita);
}
void InstruccionesJuego(JArray resultsArray)
{
    Console.WriteLine("Bienvenido al pokedex...");
    Console.WriteLine("Se escogera un pokemon aleatorio entre los " + resultsArray.Count + " pokemones disponibles");
    Console.WriteLine("Tendras que adivinar el nombre del pokemon como el juego del ahorcado");
    Console.WriteLine("Solo te puedes equivocar hasta 5 veces, so whach out");
    Console.WriteLine("Presiona una tecla para emprezar");
    Console.ReadKey();
}
void OcultarPalabra(JArray resultsArray)
{
    pokemonAleatorio = SacarNombreAleatorio(resultsArray);
    for(int i = 0; i < pokemonAleatorio.Length; i++)
    {
        if(pokemonAleatorio[i].ToString() == " ")
        {
            palabraIncognita += " ";
        }
        else if(pokemonAleatorio[i].ToString() == "-")
        {
            palabraIncognita += "-";
        }
        else
        {
            palabraIncognita += "_";
        }
    }
}
string  SacarNombreAleatorio(JArray resultsArray)
{
    int index = random.Next(0, resultsArray.Count);
    string nombreR = (string)resultsArray[index]["name"];
    return nombreR;
}

