using Obbed.Models.Words;

namespace Obbed.Dto.Words;

public class AddWordDtoInitial
{
    public IList<Category> Categories { get; set; } = null!;
}