namespace Producer.Contracts;

public record MessageRequest(
    string Title,
    string Content,
    string From,
    string To
);
