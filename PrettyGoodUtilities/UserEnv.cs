namespace PrettyGoodUtilities
{
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
        public static void SetEnvironmentVariable(string name, string value)
        {
            // Use the Environment class to set the user environment variable
            Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.User);
        }
        public static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User).ToString();
        }

    }
}
