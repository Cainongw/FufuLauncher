namespace FufuLauncher.Models;

public class AccountList
{
    public int Version { get; set; } = 1;
    public List<AccountEntry> Accounts { get; set; } = new();
}