using LoanManagementSystem.Entities.Admins;

namespace LoanManagementSystem.TestTools.Admins;

public static class AdminFactory
{
    public static Admin Generate(string adminName = "DummyAdmin")
    {
        return new Admin
        {
            Name = adminName
        };
    }
}