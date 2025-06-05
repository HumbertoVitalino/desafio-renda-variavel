namespace Core.Entities;

public sealed class Cotacao
{
    public int IdCotacao { get; private set; }
    public int FkIdAtivo { get; private set; }
    public decimal PrecoUnitario { get; private set; }
    public DateTime DataHora { get; private set; }
    public Ativo? Ativo { get; private set; }

    private Cotacao() { }

    public Cotacao(int fkIdAtivo, decimal precoUnitario, DateTime dataHora)
    {
        FkIdAtivo = fkIdAtivo;
        PrecoUnitario = precoUnitario;
        DataHora = dataHora;
    }
}
