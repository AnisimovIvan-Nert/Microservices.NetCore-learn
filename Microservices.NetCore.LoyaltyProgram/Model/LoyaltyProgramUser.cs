namespace Microservices.NetCore.LoyaltyProgram.Model;

public class LoyaltyProgramUser
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int LoyaltyPoints { get; set; }
    public required LoyaltyProgramSettings Settings { get; set; }
}