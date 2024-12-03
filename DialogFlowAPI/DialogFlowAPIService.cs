using Google.Apis.Auth.OAuth2;
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Grpc.Auth;
using Grpc.Core;
using System.Threading.Channels;
using static Google.Rpc.Context.AttributeContext.Types;

namespace DialogFlowAPI
{
    public class DialogFlowAPIService
    {
        private readonly SessionsClient _client;
        const string projectId = "uworld-bot";

        public DialogFlowAPIService()
        {
        }
       
        public async Task<string> GetResultAsync(string sessionId,string query)
        {
            string jsonCredentialsPath = @"D://bot-config.json";

            // Load the credentials from the service account file
 // Load the credentials from the service account file
            GoogleCredential credentials = GoogleCredential.FromFile(jsonCredentialsPath)
                                                            .CreateScoped(SessionsClient.DefaultScopes);
            SessionsClient client = new SessionsClientBuilder
            {
                GoogleCredential = credentials
            }.Build();

            // Convert the credentials to ChannelCredentials
            //ChannelCredentials channelCredentials = credentials.ToChannelCredentials();
           // string endpoint = "dialogflow.googleapis.com"; // Default endpoint

            // Create the Channel using the credentials
           // Grpc.Core.Channel channel = new Grpc.Core.Channel(
             //   endpoint,  // Host: api.dialogflow.com
               // channelCredentials);  // Use the credentials as ChannelCredentials


            // Create the SessionsClient with the credentials
           // SessionsClient _client = SessionsClient.Create();
            SessionName session = SessionName.FromProjectSession(projectId, sessionId);
            TextInput textInput = new TextInput
            {
                Text = query,
                LanguageCode = "en"
            };
            QueryInput queryInput = new QueryInput
            {
                Text = textInput
            };
            DetectIntentRequest detectIntentRequest = new DetectIntentRequest()
            {
                Session = sessionId,
                QueryInput = queryInput,


            };

            var response = await client.DetectIntentAsync(session,queryInput);

            return response.QueryResult.FulfillmentText;

        }

    }
}
