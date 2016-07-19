using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class StatusStoreClient : IStatusStoreClient
{
    public async Task AddCompletedCommandToBatch(Guid batchId, Guid messageId)
    {
        using (var client = GetClient())
        {
            var completedMessage = new CompletedMessage {BatchId = batchId, MessageId = messageId};
            await client.PostAsJsonAsync("CompleteMessage", completedMessage);
        }
    }

    public async Task<BatchStatus> GetBatchStatus(string batchId)
    {
        using (var client = GetClient())
        {
            var resp = await client.GetAsync($"Batch?id={batchId}");
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsAsync<BatchStatus>();
        }
    }

    private static HttpClient GetClient()
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.BaseAddress = new Uri("http://localhost:8576/api/statusstore/");
        return httpClient;
    }

}