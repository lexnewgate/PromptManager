namespace PromptManager.Models;

public class Card
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Card Clone()
    {
        return new Card
        {
            Id = Id,
            Title = Title,
            Summary = Summary,
            Content = Content,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }
}
