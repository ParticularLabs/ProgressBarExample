using System;
using System.Net.Http;
using System.Net.Http.Headers;

public class StatusStoreClient : IStatusStoreClient
{
    public void AddCompletedCommandToBatch(Guid batchId, Guid messageId)
    {
        using (var client = GetClient())
        {
            var completedMessage = new CompletedMessage {BatchId = batchId, MessageId = messageId};
            client.PostAsJsonAsync("CompleteMessage", completedMessage).RunSynchronously();
        }
    }

    public BatchStatus GetBatchStatus(string batchId)
    {
        using (var client = GetClient())
        {
            var resp = client.GetAsync($"Batch?id={batchId}").Result;
            resp.EnsureSuccessStatusCode();
            var batchStatus = resp.Content.ReadAsAsync<BatchStatus>().Result;
            return batchStatus;
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