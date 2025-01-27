using DotNetEnv;
using System;
using System.IO;

//test-comment
public class EnvConfig
{
    public static void Initialize()
    {
      
        //not implemented yet
    }

    public static string GetEnvVariable(string variable)
    {
        string envVariable = Environment.GetEnvironmentVariable(variable);

        if (string.IsNullOrEmpty(envVariable))
        {
            throw new Exception(variable + " its not configured in the envionment variables.");
        }
        return envVariable;
    }
}
