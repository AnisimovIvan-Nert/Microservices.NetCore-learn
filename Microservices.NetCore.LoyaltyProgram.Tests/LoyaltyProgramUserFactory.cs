using Microservices.NetCore.LoyaltyProgram.Model;

namespace Microservices.NetCore.LoyaltyProgram.Tests;

public static class LoyaltyProgramUserFactory
{
    private const string DefaultName = "defaultName";
    
    public static LoyaltyProgramUser CreateDefault()
    {
        return new LoyaltyProgramUser
        {
            Name = DefaultName,
            Settings = new LoyaltyProgramSettings
            {
                Interests = []
            }
        };
    }

    public static void AssertEqualDefault(LoyaltyProgramUser user)
    {
        Assert.Equal(DefaultName, user.Name);
        Assert.Equal(0, user.LoyaltyPoints);
        Assert.Empty(user.Settings.Interests);
    }
}