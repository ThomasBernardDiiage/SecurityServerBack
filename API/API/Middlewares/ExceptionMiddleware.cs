using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace API.Middlewares;

public class ExceptionMiddleware : IFunctionsWorkerMiddleware
{
	public ExceptionMiddleware()
	{
	}

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch(Exception ex)
        {
            Console.WriteLine("=== ERROR ===");
            Console.WriteLine(ex);
            Console.WriteLine("============");
        }
    }
}

