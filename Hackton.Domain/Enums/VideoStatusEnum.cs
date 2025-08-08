using System.ComponentModel;

namespace Hackton.Domain.Enums
{
    public enum VideoStatusEnum
    {
        [Description("Na fila")]
        NaFila = 0,
        [Description("Processando")]
        Processando = 1,
        [Description("Concluído")]
        Concluido = 2

    }
}
