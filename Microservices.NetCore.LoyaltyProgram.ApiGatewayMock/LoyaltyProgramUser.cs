namespace Microservices.NetCore.LoyaltyProgram.ApiGatewayMock;

public class LoyaltyProgramUser
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int LoyaltyPoints { get; set; }
    public required LoyaltyProgramSettings Settings { get; set; }
}