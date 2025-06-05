namespace Core.Entities;

public sealed class Ativo
{
    public int IdAtivo { get; private set; }
    public string Codigo { get; private set; } = string.Empty;
    public string Nome { get; private set; } = string.Empty;
    public ICollection<Cotacao>? Cotacoes { get; private set; }
    public ICollection<Posicao>? Posicoes { get; private set; }

    private Ativo() { }

    public Ativo(string codigo, string nome)
    {
        Codigo = codigo;
        Nome = nome;
    }
}
