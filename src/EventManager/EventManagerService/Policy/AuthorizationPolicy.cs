namespace EventManagerService.Policy;

internal class AuthorizationPolicy
{
    public static AuthorizationPolicy ApiScope()
    {
        return new AuthorizationPolicy("ApiScope", "events");
    }

    public string Name { get; }

    public string Scope { get; }

    private AuthorizationPolicy(string name, string scope)
    {
        Name = name;
        Scope = scope;
    }
}
