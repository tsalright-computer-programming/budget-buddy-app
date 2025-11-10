using Budget_Buddy.Models;

namespace Budget_Buddy.DTOs
{
    public record TransactionCreateDto(string PostedDate, string Description, int AmountCents, Guid CategoryId);

    public record TransactionReadDto(
        Guid Id,
        string PostedDate,
        string Description,
        int AmountCents,
        Guid? CategoryId,
        string? CategoryName,
        CategoryType? CategoryType
    );

    public record TransactionUpdateDto(string PostedDate, string Description, int AmountCents, Guid CategoryId);
}