namespace WorkerService.DTOs;

public class CotacaoMessageDto
{
    public string Ticker { get; set; } = string.Empty; 
    public decimal PrecoUnitario { get; set; }
    public DateTime DataHora { get; set; }
    public string? MessageId { get; set; }
}
