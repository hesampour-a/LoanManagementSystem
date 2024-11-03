using LoanManagementSystem.Entities.Admins;

namespace LoanManagementSystem.TestTools.Admins;

public static class AdminFactory
{
    public static Admin Generate()
    {
        return new Admin
        {
            Name = "DummyAdmin",
        };
    }
}