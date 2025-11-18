//---------------------------------------------------------------
// LIBRERIAS
//---------------------------------------------------------------
using Parking.Enums;
using Parking.Structs;
using static System.Console;
using static System.Text.Encoding;
using System.Text.RegularExpressions;

//---------------------------------------------------------------
// CONSTANTES
//---------------------------------------------------------------
const string RegexMenu = @"^\d{1}$";
const string RegexMatricula = @"^\d{4}[B-DF-HJ-NPR-TV-Z]{3}$";
const string RegexNip = @"^\d{3}[A-Za-z]{1}$";
const string RegexNomApeMarModCol = @"^[A-Za-zñÑáéíóúÁÉÍÓÚ\s]{3,}$";
const string RegexEmail = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
const int TamanoInicial = 10;
const int IncrementoTamano = 10;
const int PorcentajeExpansion = 80;
const int PorcentajeReduccion = 50;
const int NumeroFilas = 2;
const int NumeroColumnas = 6;
const string Libre = "🟢";
const string Ocupada = "🔴";

//---------------------------------------------------------------
// INICIO/FIN PROGRAMA
//---------------------------------------------------------------
OutputEncoding = UTF8;
Main();
Thread.Sleep(5000);
//WriteLine("Presiona una tecla para salir...");
//ReadKey();
return;

//---------------------------------------------------------------
// MAIN
//---------------------------------------------------------------
void Main() {
    var profesoresData = new Profesor?[TamanoInicial];
    var parking = new Profesor?[NumeroFilas, NumeroColumnas];
    int numProfesores;
    InicializarDatos(profesoresData, out numProfesores);
    MenuParking("Bienvenido!!!", profesoresData, ref numProfesores, parking);
}

//---------------------------------------------------------------
// INICIALIZACION MATRICES
//---------------------------------------------------------------
void InicializarDatos(Profesor?[] profesor, out int numProfesores) {
    var p1 = new Profesor { Nip = "111A", Nombre = "José Luis", Apellidos = "González", 
        Email = "joseluis@gmail.com", Vehiculo = null };
    var p2 = new Profesor { Nip = "222B", Nombre = "Rafael", Apellidos = "Manjavacas", 
        Email = "rafael@gmail.com", Vehiculo = new Vehiculo { Matricula = "1234BBC", Marca = "Toyota", 
            Modelo = "Celica", Color = "Rojo" } };
    var p3 = new Profesor { Nip = "333C", Nombre = "Pablo", Apellidos = "Berciano", 
        Email = "pablo@gmail.com", Vehiculo = new Vehiculo { Matricula = "4321HHV",  Marca = "Honda", 
            Modelo = "Civic", Color = "Negro" } };
    
    profesor[0] = p1;
    profesor[1] = p2;
    profesor[2] = p3;
    
    numProfesores = 3;
}

//---------------------------------------------------------------
// MENUS Y SUS CASOS
//---------------------------------------------------------------
void MenuParking(string msg, Profesor?[] profesor, ref int numProfesores, Profesor?[,] parking) {
    Clear();
    WriteLine(msg);
    var entradaOk = false;
    do {
        WriteLine("Elija una opción:");
        WriteLine($"{(int)OpcionesMenuParking.EntradaParking}. Entrada al Parking");
        WriteLine($"{(int)OpcionesMenuParking.SalidaParking}. Salida al Parking");
        WriteLine($"{(int)OpcionesMenuParking.Administracion}. Administracion");
        WriteLine($"{(int)OpcionesMenuParking.Exit}. Exit");
        var input = (ReadLine() ?? "").Trim();
        if (Regex.IsMatch(input, RegexMenu)) {
            var opcion = (OpcionesMenuParking)int.Parse(input);
            CasosMenuParking(opcion, ref entradaOk, profesor, ref numProfesores, parking);
        }
        else {
            WriteLine("Opción elegida no válida");
            WriteLine("Ingrese opción de nuevo...");
        }
    } while (!entradaOk);
}

void CasosMenuParking(OpcionesMenuParking opcion, ref bool entradaOk, Profesor?[] profesor, ref int numProfesores, Profesor?[,] parking) {
    switch (opcion) {
        case OpcionesMenuParking.EntradaParking:
            EntradaParking(profesor, parking);
            break;
        case OpcionesMenuParking.SalidaParking:
            SalidaParking(parking);
            break;
        case OpcionesMenuParking.Administracion:
            MenuAdmin(profesor, ref numProfesores, parking);
            break;
        case OpcionesMenuParking.Exit:
            entradaOk = true;
            WriteLine("Hasta pronto!!!");
            break;
        default:
            WriteLine("Opción elegida no válida");
            WriteLine("Ingrese opción de nuevo...");
            break;
    }
}

void MenuAdmin(Profesor?[] profesor, ref int numProfesores, Profesor?[,] parking) {
    Clear();
    var entradaOk = false;
    do {
        WriteLine("Elija una opción:");
        WriteLine($"{(int)OpcionesMenuAdmin.VerParking}. Ver Parking");
        WriteLine($"{(int)OpcionesMenuAdmin.AnadirProfesor}. Añadir nuevo profesor");
        WriteLine($"{(int)OpcionesMenuAdmin.AnadirVehiculo}. Añadir nuevo vehículo");
        WriteLine($"{(int)OpcionesMenuAdmin.ModificarProfesor}. Modificar profesor");
        WriteLine($"{(int)OpcionesMenuAdmin.ModificarVehiculo}. Modificar vehículo");
        WriteLine($"{(int)OpcionesMenuAdmin.EliminarProfesor}. Eliminar profesor");
        WriteLine($"{(int)OpcionesMenuAdmin.EliminarVehiculo}. Eliminar vehículo");
        WriteLine($"{(int)OpcionesMenuAdmin.ListarProfesores}. Listado de profesores");
        WriteLine($"{(int)OpcionesMenuAdmin.ListarVehiculos}. Listado de vehículos");
        WriteLine($"{(int)OpcionesMenuAdmin.Exit}. Salir");
        var input = (ReadLine() ?? "").Trim();
        if (Regex.IsMatch(input, RegexMenu)) {
            var opcion = (OpcionesMenuAdmin)int.Parse(input);
            CasosMenuAdmin(opcion, ref entradaOk, profesor, ref numProfesores, parking);
        }
        else {
            WriteLine("Opción elegida no válida");
            WriteLine("Ingrese opción de nuevo...");
        }
    } while (!entradaOk);
}

void CasosMenuAdmin(OpcionesMenuAdmin opcion, ref bool entradaOk, Profesor?[] profesor, ref int numProfesores, Profesor?[,] parking) {
    switch (opcion) {
        case OpcionesMenuAdmin.VerParking:
            DibujarParking(parking);
            VehiculoParking(parking);
            break;
        case OpcionesMenuAdmin.AnadirProfesor:
            AnadirProfesor(profesor,  ref numProfesores);
            break;
        case OpcionesMenuAdmin.AnadirVehiculo:
            AnadirVehiculo(profesor);
            break;
        case OpcionesMenuAdmin.ModificarProfesor:
            ModificarProfesor(profesor);
            break;
        case OpcionesMenuAdmin.ModificarVehiculo:
            ModificarVehiculo(profesor);
            break;
        case OpcionesMenuAdmin.EliminarProfesor:
            EliminarProfesor(profesor, ref numProfesores);
            break;
        case OpcionesMenuAdmin.EliminarVehiculo:
            EliminarVehiculo(profesor);
            break;
        case OpcionesMenuAdmin.ListarProfesores:
            ListarProfesor(profesor, numProfesores);
            break;
        case OpcionesMenuAdmin.ListarVehiculos:
            ListarVehiculo(profesor, numProfesores);
            break;
        case OpcionesMenuAdmin.Exit:
            entradaOk = true;
            WriteLine("Hasta pronto!!!");
            break;
        default:
            WriteLine("Opción elegida no válida");
            WriteLine("Ingrese opción de nuevo...");
            break;
    }
}

//---------------------------------------------------------------
// GESTION PARKING
//---------------------------------------------------------------
void EntradaParking(Profesor?[] profesor, Profesor?[,] parking) {
    Clear();
    WriteLine("Estado actual del parking:");
    DibujarParking(parking);
    if (HayHueco(parking)) {
        WriteLine("Introduzca la matrícula");
        var matricula = (ReadLine() ?? "").Trim().ToUpper();
        var encontrado = false;
        bool matriculaOk;
        do {
            if (Regex.IsMatch(matricula, RegexMatricula)) {
                matriculaOk = true;
                for (var i = 0; i < profesor.Length && !encontrado; i++) {
                    if (profesor[i].HasValue) {
                        if (profesor[i]!.Value.Vehiculo.HasValue) {
                            if (profesor[i]!.Value.Vehiculo?.Matricula == matricula) {
                                Clear();
                                WriteLine($"Bienvenido {profesor[i]!.Value.Nombre}");
                                for (var j = 0; j < NumeroFilas; j++) {
                                    for (var k = 0; k < NumeroColumnas; k++) {
                                        if (parking[j, k] == null) {
                                            if (j == 0) {
                                                WriteLine($"Ocupe la plaza A{k+1}");
                                            }
                                            else {
                                                WriteLine($"Ocupe la plaza B{k+1}");
                                            }
                                            parking[j, k] = profesor[i];
                                            WriteLine("Abriendo la barrera para usted...");
                                            WriteLine("Pase un buen día");
                                            WriteLine("Estado actual del parking:");
                                            DibujarParking(parking);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                WriteLine($"No se encontró ningun profesor con vehículo con la matricula {matricula}");
                WriteLine("Por favor, pongase en contacto con el Administrador para solicitar su acceso al parking");
            }
            else {
                WriteLine("Matricula no valida");
                matriculaOk = false;
            }
        } while (!matriculaOk);
    }
}

void SalidaParking(Profesor?[,] parking) {
    Clear();
    WriteLine("Estado actual del parking:");
    DibujarParking(parking);
    WriteLine("Introduzca la matrícula");
    var matricula = (ReadLine() ?? "").Trim().ToUpper();
    bool matriculaOk;
    do {
        if (Regex.IsMatch(matricula, RegexMatricula)) {
            matriculaOk = true;
            for (var i = 0; i < NumeroFilas; i++) {
                for (var j = 0; j < NumeroColumnas; j++) {
                    if (parking[i, j].HasValue) {
                        if (parking[i, j]!.Value.Vehiculo.HasValue) {
                            if (parking[i, j]!.Value.Vehiculo?.Matricula == matricula) {
                                Clear();
                                WriteLine($"Hasta pronto {parking[i, j]!.Value.Nombre}");
                                parking[i, j] = null;
                                WriteLine("Abriendo la barrera para usted...");
                                WriteLine("Pase un buen día");
                                WriteLine("Estado actual del parking:");
                                DibujarParking(parking);
                                return;
                            }
                        }
                    }
                }
            }
        }
        else {
            WriteLine("Matricula no valida");
            matriculaOk = false;
        }
    } while(!matriculaOk);
}

void DibujarParking(Profesor?[,] parking) {
    Clear();
    var fila1 = "A";
    var fila2 = "B";
    var numColumna = 0;
    string nuevaColumna;
    do {
        nuevaColumna = fila1 + (numColumna + 1);
        Write($" {nuevaColumna} ");
        numColumna ++;
    } while (numColumna < NumeroColumnas);
    WriteLine();
    for (var i = 0; i < NumeroFilas; i++) {
        for (var j = 0; j < NumeroColumnas; j++) {
            Write(parking[i, j] == null ? $"|{Libre}|" : $"|{Ocupada}|");
        }
        WriteLine();
    }
    numColumna = 0;
    do {
        nuevaColumna = fila2 + (numColumna + 1);
        Write($" {nuevaColumna} ");
        numColumna ++;
    } while (numColumna < NumeroColumnas);
    WriteLine();
}

bool HayHueco(Profesor?[,] parking) {
    for (var i = 0; i < NumeroFilas; i++) {
        for (var j = 0; j < NumeroColumnas; j++) {
            if (parking[i, j] == null) {
                return true;
            }
        }
    }
    return false;
}

void VehiculoParking(Profesor?[,] parking) {
    for (var i = 0; i < NumeroFilas; i++) {
        for (var j = 0; j < NumeroColumnas; j++) {
            if (parking[i, j] != null) {
                WriteLine("-------------------");
                WriteLine(i == 0 ? $"Profesor con vehículo en A{j + 1}:" : $"Profesor con vehículo en B{j + 1}:");
                WriteLine(parking[i, j]!.Value.Nip);
                WriteLine(parking[i, j]!.Value.Nombre);
                WriteLine(parking[i, j]!.Value.Apellidos);
                WriteLine(parking[i, j]!.Value.Email);
                WriteLine("Con vehículo:");
                WriteLine(parking[i, j]!.Value.Vehiculo?.Matricula);
            }
        }
        WriteLine();
    }
}

//---------------------------------------------------------------
// GESTION PROFESORES
//---------------------------------------------------------------
void AnadirProfesor(Profesor?[] profesor, ref int numProfesores) {
    Clear();
    var nip = ValidarDatos("Ingrese el NIP del profesor:", RegexNip).ToUpper();
    var indice = BuscarProf(nip, profesor);
    if (indice == -1) {
        var nombre = ValidarDatos("Ingrese el Nombre del profesor:", RegexNomApeMarModCol);
        var apellido = ValidarDatos("Ingrese el/los Apellidos del profesor:", RegexNomApeMarModCol);
        var email = ValidarDatos("Ingrese el Email del profesor:", RegexEmail);
        if (Confirmacion("El profesor tiene coche?? s/n")) {
            var matricula = ValidarDatos("Ingrese la Matricula del coche:", RegexMatricula).ToUpper();
            var marca = ValidarDatos("Ingrese la Marca del coche:", RegexNomApeMarModCol);
            var modelo = ValidarDatos("Ingrese el Modelo del coche:", RegexNomApeMarModCol);
            var color = ValidarDatos("Ingrese el Color del coche:", RegexNomApeMarModCol);
            var nuevoProfesor = new Profesor { Nip = nip, Nombre = nombre, Apellidos = apellido, 
                Email = email, Vehiculo = new Vehiculo { Matricula = matricula, Marca = marca, 
                    Modelo = modelo, Color = color } };
            Aumentar(ref profesor, numProfesores);
            for (var i = 0; i < profesor.Length; i++) {
                if (!profesor[i].HasValue) {
                    profesor[i] = nuevoProfesor;
                    numProfesores++;
                    Clear();
                    InfoProf(nuevoProfesor);
                    WriteLine($"Profesor añadido exitosamente en el índice {i}.");
                    return;
                }
            }
        }
        else {
            var nuevoProfesor = new Profesor { Nip = nip, Nombre = nombre, Apellidos = apellido,
                Email = email, Vehiculo = null };
            Aumentar(ref profesor, numProfesores);
            for (var i = 0; i < profesor.Length; i++) {
                if (!profesor[i].HasValue) {
                    profesor[i] = nuevoProfesor;
                    numProfesores++;
                    Clear();
                    InfoProf(nuevoProfesor);
                    WriteLine($"Profesor añadido exitosamente en el índice {i}.");
                    return;
                }
            }
        }
    }
    else {
        WriteLine($"El profesor con NIP {nip} ya existe");
    }
}

void ModificarProfesor(Profesor?[] profesor) {
    Clear();
    var nip = ValidarDatos("Ingrese el NIP del profesor:", RegexNip).ToUpper();
    var indice = BuscarProf(nip, profesor);
    if (indice != -1) {
        var profAntiguo = profesor[indice]!.Value;
        WriteLine($"Profesor a modificar: {profAntiguo.Nombre} {profAntiguo.Apellidos}");
        var cambiar = Confirmacion("Desea cambiar el NIP?? s/n");
        var nipNuevo = cambiar ? ValidarDatos("Introduzca nuevo NIP:", RegexNip) : profAntiguo.Nip;
        cambiar = Confirmacion("Desea cambiar el nombre?? s/n");
        var nombreNuevo = cambiar ? ValidarDatos("Introduzca nuevo nombre:", RegexNomApeMarModCol) : profAntiguo.Nombre;
        cambiar = Confirmacion("Desea cambiar el/los apellidos?? s/n");
        var apellidoNuevo = cambiar ? ValidarDatos("Introduzca nuevos apellidos:", RegexNomApeMarModCol) : profAntiguo.Apellidos;
        cambiar = Confirmacion("Desea cambiar el email?? s/n");
        var emailNuevo = cambiar ? ValidarDatos("Introduzca nuevo email:", RegexEmail) :  profAntiguo.Email;
        var profNuevo = new Profesor { Nip = nipNuevo,  Nombre = nombreNuevo,  Apellidos = apellidoNuevo, 
            Email = emailNuevo, Vehiculo = profAntiguo.Vehiculo};
        Clear();
        WriteLine("Datos actualizados:");
        InfoProf(profNuevo);
        if (Confirmacion("Desea mantener los cambios?? s/n")) {
            profesor[indice] = profNuevo;
            WriteLine("Datos actualizados correctamente");
        }
        else {
            WriteLine("Actualización descartada");
        }
    }
    else {
        WriteLine($"El profesor con NIP {nip} no existe");
    }
}

void EliminarProfesor(Profesor?[] profesor, ref int numProfesores) {
    Clear();
    var nip = ValidarDatos("Ingrese el NIP del profesor:", RegexNip).ToUpper();
    var indice = BuscarProf(nip, profesor);
    if (indice != -1) {
        var prof = profesor[indice]!.Value;
        InfoProf(prof);
        if (Confirmacion("Seguro que desea eliminar al profesor?? s/n")) {
            numProfesores--;
            profesor[indice] = null;
            Reducir(ref profesor, numProfesores);
        }
        else {
            WriteLine("Eliminación cancelada");
        }
    }
    else {
        WriteLine("Profesor no encontrado...");
    }
}

void ListarProfesor(Profesor?[] profesor, int numProfesores) {
    Clear();
    var profCompacto = ProfesorCompacto(profesor, numProfesores);
    foreach (var prof in profCompacto) {
        WriteLine("-------------------");
        WriteLine($"NIP: {prof.Nip}");
        WriteLine($"Nombre: {prof.Nombre} {prof.Apellidos}");
        WriteLine($"Email: {prof.Email}");
        if (prof.Vehiculo != null) {
            WriteLine($"Vehiculo con matrícula: {prof.Vehiculo.Value.Matricula}");
        }
        else {
            WriteLine("Sin vehículo");
        }
    }
    WriteLine("-------------------");
    WriteLine();
}

//---------------------------------------------------------------
// GESTION DE VEHICULOS
//---------------------------------------------------------------
void AnadirVehiculo(Profesor?[] profesor) {
    Clear();
    var nip = ValidarDatos("Ingrese el NIP del profesor al que se va a añadir el vehículo:", RegexNip).ToUpper();
    var indice = BuscarProf(nip, profesor);
    if (indice != -1) {
        if (profesor[indice]!.Value.Vehiculo == null) {
            nip = profesor[indice]!.Value.Nip;
            var nombre = profesor[indice]!.Value.Nombre;
            var apellido = profesor[indice]!.Value.Apellidos;
            var email = profesor[indice]!.Value.Email;
            var matricula = ValidarDatos("Ingrese la Matricula del coche:", RegexMatricula).ToUpper();
            var marca = ValidarDatos("Ingrese la Marca del coche:", RegexNomApeMarModCol);
            var modelo = ValidarDatos("Ingrese el Modelo del coche:", RegexNomApeMarModCol);
            var color = ValidarDatos("Ingrese el Color del coche:", RegexNomApeMarModCol);
            var nuevoProfesor = new Profesor { Nip = nip, Nombre = nombre, Apellidos = apellido, 
                Email = email, Vehiculo = new Vehiculo { Matricula = matricula, Marca = marca, 
                    Modelo = modelo, Color = color } };
            profesor[indice] = nuevoProfesor;
            Clear();
            InfoProf(nuevoProfesor);
            WriteLine($"Vehiculo añadido exitosamente a {nombre} {apellido} ");
        }
        else {
            WriteLine($"El profesor con nip {nip} ya tiene un vehículo asociado");
        }
    }
    else {
        WriteLine("Profesor no encontrado...");
    }
}

void ModificarVehiculo(Profesor?[] profesor) {
    Clear();
    var nip = ValidarDatos("Ingrese el NIP del profesor:", RegexNip).ToUpper();
    var indice = BuscarProf(nip, profesor);
    if (indice != -1) {
        var profAntiguo = profesor[indice]!.Value;
        WriteLine($"Profesor a modificar: {profAntiguo.Nombre} {profAntiguo.Apellidos}");
        var cambiar = Confirmacion("Desea cambiar la matrícula?? s/n");
        var matriculaNueva = cambiar ? ValidarDatos("Introduzca nueva matrícula:", RegexMatricula) : profAntiguo.Nip;
        cambiar = Confirmacion("Desea cambiar la marca?? s/n");
        var marcaNueva = cambiar ? ValidarDatos("Introduzca nueva marca:", RegexNomApeMarModCol) : profAntiguo.Nombre;
        cambiar = Confirmacion("Desea cambiar el modelo?? s/n");
        var modeloNuevo = cambiar ? ValidarDatos("Introduzca nuevo modelo:", RegexNomApeMarModCol) : profAntiguo.Apellidos;
        cambiar = Confirmacion("Desea cambiar el color?? s/n");
        var colorNuevo = cambiar ? ValidarDatos("Introduzca nuevo color:", RegexNomApeMarModCol) :  profAntiguo.Email;
        var profNuevo = new Profesor { Nip = profAntiguo.Nip,  Nombre = profAntiguo.Nombre,  Apellidos = profAntiguo.Apellidos, 
            Email = profAntiguo.Email, Vehiculo = new Vehiculo { Matricula = matriculaNueva, Marca = marcaNueva, 
                Modelo = modeloNuevo, Color = colorNuevo } };
        Clear();
        WriteLine("Datos actualizados:");
        InfoProf(profNuevo);
        if (Confirmacion("Desea mantener los cambios?? s/n")) {
            profesor[indice] = profNuevo;
            WriteLine("Datos actualizados correctamente");
        }
        else {
            WriteLine("Actualización descartada");
        }
    }
    else {
        WriteLine($"El profesor con NIP {nip} no existe");
    }
}

void EliminarVehiculo(Profesor?[] profesor) {
    Clear();
    var nip = ValidarDatos("Ingrese el NIP del profesor al que se va a añadir el vehículo:", RegexNip).ToUpper();
    var indice = BuscarProf(nip, profesor);
    if (indice != -1) {
        if (profesor[indice]!.Value.Vehiculo != null) {
            nip = profesor[indice]!.Value.Nip;
            var nombre = profesor[indice]!.Value.Nombre;
            var apellido = profesor[indice]!.Value.Apellidos;
            var email = profesor[indice]!.Value.Email;
            var nuevoProfesor = new Profesor { Nip = nip, Nombre = nombre, Apellidos = apellido, 
                Email = email, Vehiculo = null };
            profesor[indice] = nuevoProfesor;
            WriteLine($"Vehículo de {nombre} {apellido} eliminado exitosamente");
        }
        else {
            WriteLine($"El profesor con nip {nip} no tiene ningún vehículo asociado");
        }
    }
    else {
        WriteLine("Profesor no encontrado...");
    }
}

void ListarVehiculo(Profesor?[] profesor, int numProfesores) {
    Clear();
    var profCompacto = ProfesorCompacto(profesor, numProfesores);
    foreach (var prof in profCompacto) {
        if (prof.Vehiculo != null) {
            WriteLine("-------------------");
            WriteLine($"Nombre del profesor: {prof.Nombre} {prof.Apellidos}");
            WriteLine($"Matrícula: {prof.Vehiculo.Value.Matricula}");
            WriteLine($"Marca: {prof.Vehiculo.Value.Marca}");
            WriteLine($"Modelo: {prof.Vehiculo.Value.Modelo}");
            WriteLine($"Color: {prof.Vehiculo.Value.Color}");
        }
    }
    WriteLine("-------------------");
    WriteLine();
}


//---------------------------------------------------------------
// BUSQUEDA DE PROFESORES
//---------------------------------------------------------------
int BuscarProf(string nip, Profesor?[] profesor) {
    for (var i = 0; i < profesor.Length; i++) {
        if (profesor[i]?.Nip == nip) {
            return i;
        }
    }
    return -1;
}

void InfoProf(Profesor prof) {
    WriteLine("Profesor:");
    WriteLine(prof.Nip);
    WriteLine(prof.Nombre);
    WriteLine(prof.Apellidos);
    WriteLine(prof.Email);
    if (prof.Vehiculo != null) {
        WriteLine("Con vehículo:");
        WriteLine(prof.Vehiculo.Value.Matricula);
        WriteLine(prof.Vehiculo.Value.Marca);
        WriteLine(prof.Vehiculo.Value.Modelo);
        WriteLine(prof.Vehiculo.Value.Color);
    }
    else {
        WriteLine("Sin vehículo asociado");
    }
}

//---------------------------------------------------------------
// REDIMENSIONES DE VECTOR
//---------------------------------------------------------------
void Aumentar(ref Profesor?[] profesor, int numProfesores) {
    var porcentajeUso = numProfesores * 100 / profesor.Length;
    var tamanoActual = profesor.Length;
    var nuevoTamano = tamanoActual + IncrementoTamano;
    if (porcentajeUso >= PorcentajeExpansion) {
        WriteLine("Es necesario aumentar");
        var nuevoVector = new Profesor?[nuevoTamano];
        CopiarVector(profesor, nuevoVector);
        profesor = nuevoVector;
    }
    else {
        WriteLine("No es necesario aumentar");
    }
}

void Reducir(ref Profesor?[] profesor, int numProfesores) {
    var porcentajeUso = numProfesores * 100 / profesor.Length;
    if (profesor.Length > TamanoInicial && porcentajeUso < PorcentajeReduccion) {
        var tamanoActual = profesor.Length;
        var nuevoTamano = tamanoActual - IncrementoTamano;
        if (nuevoTamano < TamanoInicial) {
            nuevoTamano = TamanoInicial;
        }
        WriteLine("Es necesario reducir");
        var nuevoVector = new Profesor?[nuevoTamano];
        CopiarVector(profesor, nuevoVector);
        profesor = nuevoVector;
    }
    else {
        WriteLine("No es necesario reducir");
    }
}

void CopiarVector(Profesor?[] origen, Profesor?[] destino) {
    var indiceDestino = 0;
    for (var i = 0; i < origen.Length; i++) {
        if (origen[i].HasValue) {
            destino[indiceDestino] = origen[i];
            indiceDestino++;
        }
    }
}

Profesor[] ProfesorCompacto(Profesor?[] profesor, int numProfesores) {
    var profCompacto = new Profesor[numProfesores];
    var i = 0;
    for (var j = 0; j < profesor.Length; j++) {
        if (profesor[j] is { } profesorValido) {
            profCompacto[i] = profesorValido;
            i++;
        }
    }
    return profCompacto;
}

//---------------------------------------------------------------
// VALIDACION DE ENTRADA
//---------------------------------------------------------------
string ValidarDatos(string msg, string patron) {
    bool esValido;
    string input;
    do {
        WriteLine(msg);
        input = (ReadLine() ?? "").Trim();
        esValido = true;
        if (!Regex.IsMatch(input, patron)) {
            WriteLine("Datos introducidos no aptos");
            esValido = false;
        }
    } while (!esValido);
    return input;
}

//---------------------------------------------------------------
// CONFIRMACION
//---------------------------------------------------------------
bool Confirmacion(string msg) {
    WriteLine(msg);
    var key = ReadKey(true).KeyChar;
    WriteLine(key);
    if (char.ToUpper(key) == 'S') {
        return true;
    }
    return false;
}