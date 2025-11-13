// Librerias
using System.Text.RegularExpressions;
using static System.Text.Encoding;
using static System.Console;

// Constantes
const int NumeroFilas = 6;
const int NumeroColumnas = 8;
const string Libre = "🟢";
const string Ocupada = "🔴";
const string Averiada = "🚫";
const double PrecioEntrada = 6.50;
const int OpcionVerSala = 1;
const int OpcionComprar = 2;
const int OpcionDevolver = 3;
const int OpcionInforme = 4;
const int OpcionSalir = 5;
var random = Random.Shared;


// Inicio del Main
OutputEncoding = UTF8;

var bienvenida = "---- Bienvenido a Cines KOI ----";
Estado[,] sala = new Estado[NumeroFilas, NumeroColumnas];
CrearSala(sala);
int entradasCompradas = 0;
Menu(bienvenida, sala, ref entradasCompradas);
WriteLine("Gracias por su visita!!!!");

return;
// Fin del Main

// Funciones y parámetros
// Creamos la sala con los huecos libres, ocupados y averiados
void CrearSala(Estado[,] sala) {
    for (int i = 0; i < NumeroFilas; i++) {
        for (int j = 0; j < NumeroColumnas; j++) {
            sala[i, j] = Estado.Libre;
        }
    }
    
    int numeroAverias = new Random().Next(0, 4);
    do {
        int averiaFila = random.Next(0, NumeroFilas);
        int averiaColumna = random.Next(0, NumeroColumnas);
        sala[averiaFila, averiaColumna] = Estado.Averiada;
        numeroAverias--;
    } while (numeroAverias != 0);
}

// Creamos el menú de inicio
void Menu(string msg, Estado[,] sala, ref int entradasCompradas) {
    WriteLine(msg);
    int opcionElegida = 0;
    var regex = new Regex(@"^\d{1}$");
    do {
        WriteLine("Seleccione una opción:");
        WriteLine($"{OpcionVerSala} .- Ver estado de la sala");
        WriteLine($"{OpcionComprar} .- Comprar entrada");
        WriteLine($"{OpcionDevolver} .- Devolver entrada");
        WriteLine($"{OpcionInforme} .- Informe de venta");
        WriteLine($"{OpcionSalir} .- Salir");
        string input = (ReadLine() ?? "").Trim();
        if (regex.IsMatch(input)) {
            opcionElegida = int.Parse(input);
        }
        Opciones(opcionElegida, sala, ref entradasCompradas);
    } while (opcionElegida != OpcionSalir);
}

// Creamos las opciones
void Opciones(int opcionElegida, Estado[,] sala, ref int entradasCompradas) {
    switch (opcionElegida) {
        case OpcionVerSala:
            Clear();
            DibujarSala(sala);
            break;
        case OpcionComprar:
            Clear();
            ComprarEntrada(sala, ref entradasCompradas);
            break;
        case OpcionDevolver:
            Clear();
            DevolverEntrada(sala, ref entradasCompradas);
            break;
        case OpcionInforme:
            Clear();
            SacarInforme(sala, entradasCompradas);
            break;
        case OpcionSalir:
            Clear();
            WriteLine("Hasta Luego!!!!");
            break;
        default:
            Clear();
            WriteLine("Opción elegida no váida...");
            break;
    }
}

// Compramos una entrada
void ComprarEntrada(Estado[,] sala, ref int entradasCompradas) {
    var disponibles = EntradasDisponibles();
    WriteLine($"Quedan {disponibles} entradas disponibles");

    bool cantidadOk = false;
    do {
        WriteLine("Cuantas entradas quieres comprar???");
        var regexCant = new Regex($@"^[1-{disponibles}]$");
        string inputCant = (ReadLine() ?? "").Trim();
        if (regexCant.IsMatch(inputCant)) {
            int cantidad = int.Parse(inputCant);
            do {
                WriteLine();
                DibujarSala(sala);
                WriteLine("Que butaca desea comprar??? nºFila:nºColumna");
                var regexButaca = new Regex($"(^[1-{NumeroFilas}]):([1-{NumeroColumnas}])$");
                var input = (ReadLine() ?? "").Trim();
                Match match = regexButaca.Match(input);
                if (match.Success) {
                    string fila = match.Groups[1].Value;
                    string columna = match.Groups[2].Value;
                    int numFila = int.Parse(fila) - 1;
                    int numColumna = int.Parse(columna) - 1;
                    if (sala[numFila, numColumna] == Estado.Libre) {
                        cantidad--;
                        entradasCompradas++;
                        sala[numFila, numColumna] = Estado.Ocupada;
                        Clear();
                    }
                    else {
                        Clear();
                        WriteLine(
                            "La butaca introducida no se puede comprar, ya está vendida o está fuera de servicio, " +
                            "introduzca de nuevo...");
                    }
                }
                else {
                    Clear();
                    WriteLine("La butaca introducida no existe o no se puede comprar, introduzca de nuevo...");
                }

                WriteLine($"Llevas compradas {entradasCompradas} entradas");
            } while (cantidad != 0);
            cantidadOk = false;
        }
        else {
            WriteLine("Llevas compradas no se puede comprar");
            cantidadOk = true;
        }
    } while (!cantidadOk);

    DibujarSala(sala);
    double total = entradasCompradas * PrecioEntrada;
    WriteLine($"Has comprado {entradasCompradas} entradas");
    WriteLine($"Cantidad a pagar: {total}€ -- {PrecioEntrada}/entrada");
    WriteLine();
}

// Devolvemos una entrada
void DevolverEntrada(Estado[,] sala, ref int entradasCompradas) {
    if (entradasCompradas == 0) {
        WriteLine("Aun no has comprado ninguna entrada...");
        WriteLine();
        return;
    }
    WriteLine("Cuantas entradas quieres devolver???");
    int cantidad = 0;
    var regexCant = new Regex($@"^[1-{entradasCompradas}]$");
    string inputCant = (ReadLine() ?? "").Trim();
    if (regexCant.IsMatch(inputCant)) {
        cantidad = int.Parse(inputCant);
    }
    int entradasDevueltas = cantidad;
    do {
        WriteLine();
        DibujarSala(sala);
        WriteLine("Que butaca desea devolver??? nºFila:nºColumna");
        var regexButaca = new Regex($"(^[1-{NumeroFilas}]):([1-{NumeroColumnas}])$");
        var input = (ReadLine() ?? "").Trim();
        Match match = regexButaca.Match(input);
        string fila = match.Groups[1].Value;
        string columna = match.Groups[2].Value;
        int numFila = int.Parse(fila) - 1;
        int numColumna = int.Parse(columna) - 1;
        if (match.Success) {
            if (sala[numFila, numColumna] == Estado.Ocupada) {
                cantidad--;
                entradasCompradas--;
                sala[numFila, numColumna] = Estado.Libre;
                Clear();
            } else {
                Clear();
                WriteLine("La butaca introducida no se puede devolver, introduzca de nuevo...");
            }
        } else {
            Clear();
            WriteLine("La butaca introducida no se puede devolver, introduzca de nuevo..."); 
        }
    } while (cantidad != 0);
    
    DibujarSala(sala);
    double total = entradasDevueltas * PrecioEntrada;
    WriteLine($"Se te devolverán {entradasDevueltas} entradas");
    WriteLine($"Cantidad a devolver: {total}€ -- {PrecioEntrada}/entrada");
    WriteLine();
}

// Sacamos informe de la venta
void SacarInforme(Estado[,] sala, int entradasCompradas) {
    Clear();
    WriteLine();
    WriteLine("---- Informe de Venta Cines KOI ----");
    DibujarSala(sala);
    int entradasDisponibles = EntradasDisponibles();
    int entradasVendidas = EntradasVendidas();
    int asientosAveriados = AsientosAveriados();
    int totalEntradas = entradasDisponibles + entradasVendidas + asientosAveriados;
    double ocupacion = (double)entradasVendidas/100 * (totalEntradas-asientosAveriados);
    double recaudación = entradasVendidas * PrecioEntrada;
    WriteLine($"Entradas vendidas: {entradasCompradas}");
    WriteLine($"Asientos disponibles: {entradasDisponibles}");
    WriteLine($"Asientos Fuera de Servicio: {asientosAveriados}");
    WriteLine($"Ocupación: {ocupacion}%");
    WriteLine($"Recaudación: {recaudación}");
    WriteLine();
}

// Dibujamos la sala en pantalla
void DibujarSala(Estado[,] sala) {
    var columna = "C";
    var fila = "F";
    int numColumna = 0;
    Write("   ");
    
    do {
        string nuevaColumna = columna + (numColumna + 1);
        Write($" {nuevaColumna} ");
        numColumna ++;
    } while (numColumna < NumeroColumnas);
    WriteLine();
    
    for (int i = 0; i < NumeroFilas; i++) {
        string nuevaFila = fila + (i + 1);
        Write($"{nuevaFila} ");
        for (int j = 0; j < NumeroColumnas; j++) {
            if (sala[i, j] == Estado.Libre) {
                Write($"|{Libre}|");
            }
            else if (sala[i, j] == Estado.Ocupada) {
                Write($"|{Ocupada}|");
            }
            else if (sala[i, j] == Estado.Averiada) {
                Write($"|{Averiada}|");
            }
        }
        WriteLine();
    }
}

// Entradas disponibles
int EntradasDisponibles() {
    int entradas = 0;
    for (int i = 0; i < NumeroFilas; i++) {
        for (int j = 0; j < NumeroColumnas; j++) {
            if (sala[i, j] == Estado.Libre) {
                entradas++;
            }
        }
    }
    return entradas;
}

// Entradas vendidas
int EntradasVendidas() {
    int entradas = 0;
    for (int i = 0; i < NumeroFilas; i++) {
        for (int j = 0; j < NumeroColumnas; j++) {
            if (sala[i, j] == Estado.Ocupada) {
                entradas++;
            }
        }
    }
    return entradas;
}

// Entradas no disponibles por averia
int AsientosAveriados() {
    int entradas = 0;
    for (int i = 0; i < NumeroFilas; i++) {
        for (int j = 0; j < NumeroColumnas; j++) {
            if (sala[i, j] == Estado.Averiada) {
                entradas++;
            }
        }
    }
    return entradas;
}

// Enums y structs
enum Estado {
    Libre,
    Ocupada,
    Averiada
}