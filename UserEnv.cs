/// <summary>
/// Set and get user environment variables.
/// </summary>
public class UserEnv
{
    /// <summary>
    /// Sets a user environment variable.
    /// </summary>
    /// <param name="name">The name of the environment variable.</param>
    /// <param name="value">The value to set for the environment variable.</param>
    public void SetEnvironmentVariable(string name, string value)
    {
        // Use the Environment class to set the user environment variable
        Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.User);
    }

    /// <summary>
    /// Gets the value of a user environment variable.
    /// </summary>
    /// <param name="name">The name of the environment variable.</param>
    /// <returns>The value of the environment variable.</returns>
    public string GetEnvironmentVariable(string name)
    {
        // Use the Environment class to get the value of the user environment variable
        return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);
    }
}
