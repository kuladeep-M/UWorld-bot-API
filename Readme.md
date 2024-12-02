Google.Cloud.Dialogflow.V2
Documentation
Technology areas
Cross-product tools
Related sites
Search
/

English
Sign in
.NET
Overview
Guides
Reference
Samples
Contact Us
Start free
Filter

.NET 
Documentation 
Reference
Was this helpful?

Send feedback
Version latest
keyboard_arrow_down
Google.Cloud.Dialogflow.V2

bookmark_border
Google.Cloud.Dialogflow.V2 is a.NET client library for the Google Cloud Dialogflow API.

Note: This documentation is for version 4.23.0 of the library. Some samples may not work with other versions.

Installation
Install the Google.Cloud.Dialogflow.V2 package from NuGet. Add it to your project in the normal way (for example by right-clicking on the project in Visual Studio and choosing "Manage NuGet Packages...").

Authentication
When running on Google Cloud, no action needs to be taken to authenticate.

Otherwise, the simplest way of authenticating your API calls is to set up Application Default Credentials. The credentials will automatically be used to authenticate. See Set up Application Default Credentials for more details.

Getting started
All operations are performed through the following client classes:

AgentsClient
AnswerRecordsClient
ContextsClient
ConversationDatasetsClient
ConversationModelsClient
ConversationProfilesClient
ConversationsClient
DocumentsClient
EncryptionSpecServiceClient
EntityTypesClient
EnvironmentsClient
FulfillmentsClient
GeneratorsClient
IntentsClient
KnowledgeBasesClient
ParticipantsClient
SessionEntityTypesClient
SessionsClient
VersionsClient
Create a client instance by calling the static Create or CreateAsync methods. Alternatively, use the builder class associated with each client class (e.g. AgentsClientBuilder for AgentsClient) as an easy way of specifying custom credentials, settings, or a custom endpoint. Clients are thread-safe, and we recommend using a single instance across your entire application unless you have a particular need to configure multiple client objects separately.

Using the REST (HTTP/1.1) transport
This library defaults to performing RPCs using gRPC using the binary Protocol Buffer wire format. However, it also supports HTTP/1.1 and JSON, for situations where gRPC doesn't work as desired. (This is typically due to an incompatible proxy or other network issue.) To create a client using HTTP/1.1, specify a RestGrpcAdapter reference for the GrpcAdapter property in the client builder. Sample code:


var client = new AgentsClientBuilder
{
    GrpcAdapter = RestGrpcAdapter.Default
}.Build();
For more details, see the transport selection page.

Implementing a web hook
You don't need this package in order to implement a webhook as a Dialogflow fulfillment. You can accept a JSON request dynamically and respond to it with JSON, for example using JObject from Json.NET.

This package allows you to work with a more statically-typed view of the request and response using the Protocol Buffers representation of WebhookRequest and WebhookResponse, however. In order to do this, you must use the JSON parser and formatted provided for Protocol Buffers. You will run into problems if you use Json.NET or similar general-purpose JSON libraries, as they don't know the details of Protocol Buffer JSON representations.

Additionally, it's worth configuring the Protocol Buffer JSON parser to ignore unknown fields. This means your webhook won't break if additional fields are added to WebhookRequest in the future.

Please refer to the fulfillment documentation for more details around authentication, and the schema of requests and responses.

The samples below provide a starting point for ASP.NET Core and ASP.NET. Note that by default, ASP.NET Core 3.x does not allow synchronous IO, and the Google.Protobuf library does not yet support asynchronous parsing from a reader, so you need to asynchronously read the JSON directly as a string first, then parse the string. It's likely that in the future, Google.Protobuf will allow asynchronous parsing.

Web hook template code for ASP.NET Core (asynchronous-only)

public class DialogflowController : ControllerBase
{
    // A Protobuf JSON parser configured to ignore unknown fields. This makes
    // the action robust against new fields being introduced by Dialogflow.
    private static readonly JsonParser jsonParser =
        new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));

    public async Task<ContentResult> DialogAction()
    {
        // Read the request JSON asynchronously, as the Google.Protobuf library
        // doesn't (yet) support asynchronous parsing.
        string requestJson;
        using (TextReader reader = new StreamReader(Request.Body))
        {
            requestJson = await reader.ReadToEndAsync();
        }

        // Parse the body of the request using the Protobuf JSON parser,
        // *not* Json.NET.
        WebhookRequest request = jsonParser.Parse<WebhookRequest>(requestJson);

        // Note: you should authenticate the request here.

        // Populate the response
        WebhookResponse response = new WebhookResponse
        {
            // ...
        };

        // Ask Protobuf to format the JSON to return.
        // Again, we don't want to use Json.NET - it doesn't know how to handle Struct
        // values etc.
        string responseJson = response.ToString();
        return Content(responseJson, "application/json");
    }
}
Web hook template code for ASP.NET Core (synchronous parsing)

public class DialogflowController : ControllerBase
{
    // A Protobuf JSON parser configured to ignore unknown fields. This makes
    // the action robust against new fields being introduced by Dialogflow.
   
}
Web hook template code for ASP.NET (classic) Web API

public class WebApiController : ApiController
{
    // A Protobuf JSON parser configured to ignore unknown fields. This makes
    // the action robust against new fields being introduced by Dialogflow.
    private static readonly JsonParser jsonParser =
        new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));

    [HttpPost]
    public async Task<HttpResponseMessage> Post()
    {
        WebhookRequest request;
        using (var stream = await Request.Content.ReadAsStreamAsync())
        {
            using (var reader = new StreamReader(stream))
            {
                request = jsonParser.Parse<WebhookRequest>(reader);
            }
        }
        WebhookResponse webhookResponse = new WebhookResponse
        {
            // ...
        };
        HttpResponseMessage httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            // Ask Protobuf to format the JSON to return.
            // Again, we don't want to use Json.NET - it doesn't know how to handle Struct
            // values etc.
            Content = new StringContent(webhookResponse.ToString())
            {
                Headers = { ContentType = new MediaTypeHeaderValue("text/json") }
            }
        };

        return httpResponse;
    }
}