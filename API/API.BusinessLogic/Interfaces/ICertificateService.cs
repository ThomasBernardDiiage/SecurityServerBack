using System;
using System.Security.Cryptography.X509Certificates;
using Azure.Security.KeyVault.Certificates;
using Microsoft.Azure.Functions.Worker;
using System.Security.Claims;

namespace API.BusinessLogic.Interfaces;

public interface ICertificateService
{
    KeyVaultCertificateWithPolicy GetCertificate();
    bool VerifyCertificate(string jwtToken);
    bool TryGetTokenFromHeaders(FunctionContext context, out string token);
    IEnumerable<Claim> GetClaims(string token);
}

