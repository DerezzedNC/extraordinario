using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace InventarioApp
{
    public class Producto
    {
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public int Cantidad { get; set; }

        public Producto() { } 

        public Producto(string nombre, double precio, int cantidad)
        {
            Nombre = nombre;
            Precio = precio;
            Cantidad = cantidad;
        }

        public double TotalVentas()
        {
            return Precio * Cantidad;
        }
    }

    public class Inventario
    {
        public List<Producto> Productos { get; set; }

        public Inventario()
        {
            Productos = new List<Producto>();
        }

        public void CargarDesdeArchivo(string ruta)
        {
            try
            {
                if (File.Exists(ruta))
                {
                    string json = File.ReadAllText(ruta);
                    Productos = JsonSerializer.Deserialize<List<Producto>>(json) ?? new List<Producto>();
                }
                else
                {
                    Productos = new List<Producto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer el archivo JSON: " + ex.Message);
                Productos = new List<Producto>();
            }
        }

        public void GuardarEnArchivo(string ruta)
        {
            try
            {
                string json = JsonSerializer.Serialize(Productos, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ruta, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar el archivo JSON: " + ex.Message);
            }
        }

        public double CalcularTotalVentas()
        {
            return Productos.Sum(p => p.TotalVentas());
        }

        public Producto ObtenerProductoMasCaro()
        {
            return Productos.OrderByDescending(p => p.Precio).FirstOrDefault();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string rutaArchivo = "productos.json";

            Console.WriteLine("Ingrese el nombre del producto vendido:");
            string nombreUsuario = Console.ReadLine();

            Console.WriteLine("Ingrese la cantidad vendida:");
            int cantidadUsuario = int.Parse(Console.ReadLine());

            Console.WriteLine("Ingrese el precio unitario:");
            double precioUsuario = double.Parse(Console.ReadLine());

            Producto productoUsuario = new Producto(nombreUsuario, precioUsuario, cantidadUsuario);

            Inventario inventario = new Inventario();
            inventario.CargarDesdeArchivo(rutaArchivo);

           
            inventario.Productos.Add(productoUsuario);

            Console.WriteLine("\n--- Reporte de Ventas ---");

            double totalVentas = inventario.CalcularTotalVentas();
            Console.WriteLine($"Total de ventas realizadas: ${totalVentas}");

            Producto masCaro = inventario.ObtenerProductoMasCaro();

            if (masCaro != null)
            {
                Console.WriteLine($"Producto más caro: {masCaro.Nombre} (${masCaro.Precio})");
            }
            else
            {
                Console.WriteLine("No se encontraron productos en el inventario.");
            }

           
            inventario.GuardarEnArchivo(rutaArchivo);
        }
    }
}
