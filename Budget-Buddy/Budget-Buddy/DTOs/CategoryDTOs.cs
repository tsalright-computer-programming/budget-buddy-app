using Budget_Buddy.Models;

namespace Budget_Buddy.DTOs
{
    public record CategoryCreateDto(string Name, CategoryType Type);
    public record CategoryReadDto(Guid Id, string Name, CategoryType Type, bool IsArchived);
    public record CategoryUpdateDto(string Name, CategoryType Type, bool IsArchived);
}
