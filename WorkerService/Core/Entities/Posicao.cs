namespace Core.Entities;

public sealed class Posicao
{
    public int IdPosicao { get; private set; }
    public int FkIdUsuario { get; private set; }
    public int FkIdAtivo { get; private set; }
    public int Quantidade { get; private set; }
    public decimal PrecoMedio { get; private set; }
    public decimal PL { get; private set; }
    public Ativo? Ativo { get; private set; }

    private Posicao() { }

    public Posicao(int fkIdUsuario, int fkIdAtivo, int quantidade, decimal precoMedio, decimal pl)
    {
        FkIdUsuario = fkIdUsuario;
        FkIdAtivo = fkIdAtivo;
        Quantidade = quantidade;
        PrecoMedio = precoMedio;
        PL = pl;
    }

    public void AtualizarDadosPosicao(int novaQuantidade, decimal novoPrecoMedio, decimal novoPL)
    {
        Quantidade = novaQuantidade;
        PrecoMedio = novoPrecoMedio;
        PL = novoPL;
    }

    public void AtualizarPLComNovaCotacao(decimal novoPrecoUnitarioAtivo)
    {
        if (this.Quantidade > 0)
        {
            this.PL = (this.Quantidade * novoPrecoUnitarioAtivo) - (this.Quantidade * this.PrecoMedio);
        }
        else
        {
            this.PL = 0;
        }
    }
}
