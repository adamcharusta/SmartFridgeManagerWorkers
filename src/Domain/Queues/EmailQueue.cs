using Domain.Common;

namespace Domain.Queues;

public class Email
{
    public required string Address { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
}

public class EmailQueue : BaseQueue<Email>
{
    public override bool Durable => false;
    public override bool AutoAck => true;
    public override string Queue => "email";
    public override bool Exclusive => false;
    public override bool AutoDelete => false;
    public override Dictionary<string, object>? Arguments => null;
}
