using DotNetEnv;
using System;
using System.IO;




/*
public class EnvConfig
{
    public static void Initialize()
    {
        // Lee la variable de entorno directamente
        var smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER");

        // Verifica si está vacía o nula
        if (string.IsNullOrEmpty(smtpServer))
        {
            throw new Exception("SMTP_SERVER no está configurado en las variables de entorno.");
        }
        else
        {
            Console.WriteLine("SMTP_SERVER: " + smtpServer);
        }
    }

    // Método auxiliar para obtener cualquier variable de entorno
    public static string GetEnvVariable(string variable)
    {
        string envVariable = Environment.GetEnvironmentVariable(variable);

        if (string.IsNullOrEmpty(envVariable))
        {
            throw new Exception(variable + " no está configurado en las variables de entorno.");
        }

        return envVariable;
    }
}
*/

public class EnvConfig
{
    private static readonly string EnvFilePath = @"C:\Users\arjoz\Source\Repos\NTW-TEST-PATTERNS\NTW-TEST-PATTERNS.Arquitecture\.env";
    public static void Initialize()
    {
        Console.WriteLine($"Cargando el archivo .env desde: {EnvFilePath}");
        Env.Load(EnvFilePath);

        string smtpServer = Env.GetString("SMTP_SERVER");
        if (string.IsNullOrEmpty(smtpServer))
        {
            throw new Exception("SMTP_SERVER no está configurado en el archivo .env");
        }
        else
        {
            Console.WriteLine("SMTP_SERVER: " + smtpServer);
        }
    }

    public static string GetEnvVariable(string variable)
    {
        string envVariable = Env.GetString(variable);
        if (string.IsNullOrEmpty(envVariable))
        {
            throw new Exception(variable + " no está configurado en el archivo .env");
        }
        return envVariable;
    }
}
