using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using API.BusinessLogic.Interfaces;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Worker;
using Microsoft.IdentityModel.Tokens;

namespace API.BusinessLogic;

public class CertificateService : ICertificateService
{
    private readonly ClientSecretCredential _clientSecretCredential = new ClientSecretCredential("14bc5219-40ca-4d62-a8e4-7c97c1236349", "83e27da8-f392-4116-8c45-cd2066ad394b", "t1H8Q~P7DgIV26jcgL_kStLjSc6rTFyO-19n3aT0");
    private readonly string _vaultUrl = "https://keyvaultd2p2g4preprod.vault.azure.net/";

    public CertificateService()
	{
	}

    public KeyVaultCertificateWithPolicy GetCertificate()
    {
        var client = new CertificateClient(new Uri(_vaultUrl), _clientSecretCredential);

        var secret = new SecretClient(new Uri(_vaultUrl), _clientSecretCredential);
        KeyVaultCertificateWithPolicy certificate = client.GetCertificate("2024di2p2g4");

        return certificate;
    }


    public bool VerifyCertificate(string jwtToken)
    {
        KeyVaultCertificateWithPolicy certificate = GetCertificate();
        X509Certificate2 cert = new X509Certificate2(certificate.Cer);

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new X509SecurityKey(cert),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        try
        {
            SecurityToken securityToken;
            tokenHandler.ValidateToken(jwtToken, validationParameters, out securityToken);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }




    public bool TryGetTokenFromHeaders(FunctionContext context, out string token)
    {
        token = null;
        // HTTP headers are in the binding context as a JSON object
        // The first checks ensure that we have the JSON string
        if (!context.BindingContext.BindingData.TryGetValue("Headers", out var headersObj))
            return false;

        if (headersObj is not string headersStr)
            return false;

        // Deserialize headers from JSON
        var headers = JsonSerializer.Deserialize<Dictionary<string, string>>(headersStr);
        var normalizedKeyHeaders = headers.ToDictionary(h => h.Key.ToLowerInvariant(), h => h.Value);
        if (!normalizedKeyHeaders.TryGetValue("authorization", out var authHeaderValue))
            return false; // No Authorization header present

        if (!authHeaderValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return false; // Scheme is not Bearer

        token = authHeaderValue.Substring("Bearer ".Length).Trim();
        return true;
    }

    public IEnumerable<Claim> GetClaims(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = jwtHandler.ReadJwtToken(token);

        return jwtSecurityToken.Claims;
    }
}

