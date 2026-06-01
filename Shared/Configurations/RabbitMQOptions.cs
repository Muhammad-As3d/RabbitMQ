using System.ComponentModel.DataAnnotations;

namespace Shared.Configurations;

public class RabbitMQOptions
{
    public static string SectionName = "RabbitMQ";

    [Required]
    public string Server { get; set; } = string.Empty;

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string QueueName { get; set; } = string.Empty;
}
