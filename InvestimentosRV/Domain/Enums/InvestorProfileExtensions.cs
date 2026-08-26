namespace Domain.Enums;

public static class InvestorProfileExtensions
{
    public static decimal GetBrokerageRate(this InvestorProfile profile)
    {
        return profile switch
        {
            InvestorProfile.Bold => 0.0010m,
            InvestorProfile.Moderate => 0.0025m,
            InvestorProfile.Conservative => 0.0050m,
            _ => 0.0050m
        };
    }
}
