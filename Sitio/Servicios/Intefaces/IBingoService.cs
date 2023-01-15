using Modelos.Models;

namespace Servicios.Intefaces
{
    public interface IBingoService
    {
        List<string[,]> GenerarCartones(int cantidad = 1);
        void GuardarResultado(Datos entity);
    }
}
