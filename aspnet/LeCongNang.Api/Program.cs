using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("anthropic", c =>
{
    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

var app = builder.Build();

var allowedModel = "claude-sonnet-4-20250514";

app.UseCors(p => p
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true));

app.MapPost("/api/chat", async (HttpContext ctx, IHttpClientFactory httpFactory, IConfiguration cfg) =>
{
    var apiKey = cfg["Anthropic:ApiKey"]
        ?? Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY");

    if (string.IsNullOrWhiteSpace(apiKey))
    {
        ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await ctx.Response.WriteAsJsonAsync(new { error = "ANTHROPIC_API_KEY chưa cấu hình trên server" });
        return;
    }

    using var reader = new StreamReader(ctx.Request.Body);
    var raw = await reader.ReadToEndAsync();
    if (string.IsNullOrWhiteSpace(raw))
    {
        ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
        await ctx.Response.WriteAsJsonAsync(new { error = "Body rỗng" });
        return;
    }

    using var doc = System.Text.Json.JsonDocument.Parse(raw);
    var root = doc.RootElement;
    if (!root.TryGetProperty("model", out var modelEl) || modelEl.GetString() != allowedModel)
    {
        ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
        await ctx.Response.WriteAsJsonAsync(new { error = "Model không hợp lệ" });
        return;
    }

    if (!root.TryGetProperty("messages", out var msgEl) || msgEl.ValueKind != System.Text.Json.JsonValueKind.Array || msgEl.GetArrayLength() == 0)
    {
        ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
        await ctx.Response.WriteAsJsonAsync(new { error = "messages phải là mảng không rỗng" });
        return;
    }

    var client = httpFactory.CreateClient("anthropic");
    using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages");
    req.Headers.TryAddWithoutValidation("x-api-key", apiKey);
    req.Headers.TryAddWithoutValidation("anthropic-version", "2023-06-01");
    req.Content = new StringContent(raw, Encoding.UTF8, "application/json");

    try
    {
        var res = await client.SendAsync(req);
        var body = await res.Content.ReadAsStringAsync();
        ctx.Response.StatusCode = (int)res.StatusCode;
        ctx.Response.ContentType = "application/json; charset=utf-8";
        await ctx.Response.WriteAsync(body);
    }
    catch
    {
        ctx.Response.StatusCode = StatusCodes.Status502BadGateway;
        await ctx.Response.WriteAsJsonAsync(new { error = "Lỗi kết nối tới Anthropic" });
    }
});

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
