namespace Parking.Structs;

public struct Vehiculo {
    public string Matricula;
    public string Marca;
    public string Modelo;
    public string Color;
}

public struct Profesor {
    public string Nip;
    public string Nombre;
    public string Apellidos;
    public string Email;
    public Vehiculo? Vehiculo;
}