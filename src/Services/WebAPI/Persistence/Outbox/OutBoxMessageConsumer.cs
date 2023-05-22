namespace Persistence.Outbox;

public class OutBoxMessageConsumer
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = String.Empty;
}