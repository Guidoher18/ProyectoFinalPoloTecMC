using Modelos.Models;
using Repositorio.Interfaces;
using Servicios.Intefaces;

namespace ProtectoFinal.Servicios
{
    public class BingoService : IBingoService
    {
        #region Generar Cartones
        //    Con los conocimientos vistos hasta ahora en clase realizar un programa que haga lo siguiente:
        //    Generar un programa que cree un cartón de bingo aleatorio y lo muestre por pantalla

        //    1) Cartón de 3 filas por 9 columnas
        //        |__| |__| |__|  |  |__| |__| |__|  |  |__| |__| |__|
        //        |__| |__| |__|  |  |__| |__| |__|  |  |__| |__| |__|
        //        |__| |__| |__|  |  |__| |__| |__|  |  |__| |__| |__|

        //    2) El cartón debe tener 15 números y 12 espacios en blanco
        //    3) Cada fila debe tener 5 números
        //    4) Cada columna debe tener 1 o 2 números
        //    5) Ningún número puede repetirse
        //    6) La primer columna contiene los números del 1 al 9, 
        //       la segunda del 10 al 19, la tercera del 20 al 29, 
        //       así sucesivamente hasta la última columna la cual contiene del 80 al 90
        #endregion

        private readonly IBingoRepository _bingoRepository;

        public BingoService(IBingoRepository bingoRepository)
        {
            _bingoRepository = bingoRepository;
        }

        public List<string[,]> GenerarCartones(int cantidad = 1)
        {
            List<string[,]> cartones = new List<string[,]>() { };

            // Filas, Columnas
            bool validacion;
            string[,] carton;

            while(cartones.Count < cantidad) {
                carton = new string[3, 9];
                validacion = false;

                while (!validacion)
            {
                carton = new string[3, 9];
                int cantColConTresNumeros;
                do
                {
                    cantColConTresNumeros = 0;

                    int start = 1;
                    int end = 9;

                    for (var c = 0; c < 9; c++)
                    {
                        switch (c)
                        {
                            case 0:
                                break;
                            case 1:
                                start = 10;
                                end = 19;
                                break;
                            default:
                                start += 10;
                                end += 10;
                                break;
                        }

                        int x = new Random().Next(start, end);
                        int w = new Random().Next(start, end);
                        int z = new Random().Next(start, end);

                        // Obtengo 3 números distintos
                        while (x == w || w == z || x == z)
                        {
                            x = new Random().Next(start, end);
                            w = new Random().Next(start, end);
                            z = new Random().Next(start, end);
                        }

                        List<int> numbers = new List<int>() { x, w, z };
                        numbers.Sort();
                        carton[0, c] = numbers[0].ToString();
                        carton[1, c] = numbers[1].ToString();
                        carton[2, c] = numbers[2].ToString();
                    }

                    // Sólo 5 números por fila
                    for (var h = 0; h < 2; h++)
                    {
                        int aF = new Random().Next(0, 9);
                        int bF = new Random().Next(0, 9);
                        int cF = new Random().Next(0, 9);
                        int dF = new Random().Next(0, 9);

                        while (aF == bF || aF == cF || aF == dF || bF == cF || bF == dF || cF == dF)
                        {
                            aF = new Random().Next(0, 9);
                            bF = new Random().Next(0, 9);
                            cF = new Random().Next(0, 9);
                            dF = new Random().Next(0, 9);
                        }

                        carton[h, aF] = "__";
                        carton[h, bF] = "__";
                        carton[h, cF] = "__";
                        carton[h, dF] = "__";
                    }

                    // Verifico cuantas columnas quedaron con 3 numeros
                    for (var i = 0; i < 9; i++)
                    {
                        if (carton[0, i] != "__" && carton[1, i] != "__" && carton[2, i] != "__")
                            cantColConTresNumeros++;
                    }

                } while (cantColConTresNumeros > 4);

                // Elimino el último número si la columna tiene tres
                for (var i = 0; i < 9; i++)
                {
                    if (carton[0, i] != "__" && carton[1, i] != "__" && carton[2, i] != "__")
                        carton[2, i] = "__";
                }

                // Ajusto la última fila para que tenga sólo 5 números
                int cantVacios = 0;
                for (int i = 0; i < 9; i++)
                {
                    if (carton[2, i] == "__")
                        cantVacios++;
                }

                int faltantes = 4 - cantVacios;

                if (faltantes > 0)
                {
                    while (cantVacios < 4)
                    {
                        int col = new Random().Next(0, 8);
                        if (carton[2, col] != "__")
                        {
                            carton[2, col] = "__";
                            cantVacios++;
                        }
                    }
                }

                // VALIDACIONES
                bool cincoNumeros = true;

                // Cada fila debe contener 5 números
                for (int i = 0; i < 3; i++)
                {
                    int num = 0;
                    for (int j = 0; j < 9; j++)
                    {
                        if (carton[i, j] != "__")
                            num++;
                    }

                    if (num > 5)
                    {
                        cincoNumeros = false;
                        break;
                    }
                }

                // Cada columna debe contener 1 o 2 números
                bool columnasUnoDos = true;

                if (cincoNumeros)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        int vacios = 0;
                        for (int j = 0; j < 3; j++)
                        {
                            if (carton[j, i] == "__")
                                vacios++;
                        }

                        if (vacios > 2)
                        {
                            columnasUnoDos = false;
                            break;
                        }
                    }
                }

                if (cincoNumeros && columnasUnoDos)
                {
                    validacion = true;
                }
            }
                
                cartones.Add(carton);
            }

            return cartones;
        }

        public void GuardarResultado(Datos entity) => _bingoRepository.Add(entity);
    }
}