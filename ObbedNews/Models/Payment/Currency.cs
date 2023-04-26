using System.ComponentModel.DataAnnotations;

namespace ObbedNews.Models.Payment;

public class Currency
{
    [Key] public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Sign { get; set; } = null!;
    public string ShortName { get; set; } = null!;
}