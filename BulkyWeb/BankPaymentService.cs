using Bulky.Models;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;

namespace BulkyWeb;

public class BankPaymentService
{

    private readonly HttpClient _httpClient;
private readonly IConfiguration _configuration;

public BankPaymentService(HttpClient httpClient, IConfiguration configuration)
{
    _httpClient = httpClient;
    _configuration = configuration;

    // Load certificate
    var handler = new HttpClientHandler();
        var certPath = Path.Combine(Directory.GetCurrentDirectory(), _configuration["BankApi:CertPath"]);
        var cert = new X509Certificate2(certPath, "12345");
    handler.ClientCertificates.Add(cert);

        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            // Perform your custom validation logic if necessary
            return true; // For testing purposes, accept any certificate
        };

        _httpClient = new HttpClient(handler);
}

public async Task<PaymentResult> ProcessPayment(OrderHeader orderHeader, string returnUrl)
{
    var requestXml = new XElement("TKKPG",
        new XElement("Request",
            new XElement("Operation", "CreateOrder"),
            new XElement("Language", "EN"),
            new XElement("Order",
                new XElement("OrderType", "Purchase"),
                new XElement("Merchant", _configuration["BankApi:MerchantId"]),
                new XElement("Amount", (orderHeader.OrderTotal * 100).ToString("F0")), // Multiply by 100 as required
                new XElement("Currency", "944"),
                new XElement("Description", $"Order {orderHeader.Id}"),
                new XElement("ApproveURL", $"{returnUrl}/Approve"),
                new XElement("CancelURL", $"{returnUrl}/Cancel"),
                new XElement("DeclineURL", $"{returnUrl}/Decline")
            )
        )
    );

    var content = new StringContent(requestXml.ToString(), Encoding.UTF8, "application/xml");

    var response = await _httpClient.PostAsync(_configuration["BankApi:PaymentEndpoint"], content);

    if (response.IsSuccessStatusCode)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseXml = XDocument.Parse(responseContent);

        var status = responseXml.Root.Element("Response").Element("Status").Value;
        if (status == "00")
        {
            var orderId = responseXml.Root.Element("Response").Element("Order").Element("OrderID").Value;
            var sessionId = responseXml.Root.Element("Response").Element("Order").Element("SessionID").Value;
            var url = responseXml.Root.Element("Response").Element("Order").Element("URL").Value;

            return new PaymentResult
            {
                IsSuccess = true,
                OrderId = orderId,
                SessionId = sessionId,
                Url = url
            };
        }
    }

    return new PaymentResult { IsSuccess = false, ErrorMessage = "Payment request failed." };
}
}


public class PaymentResult
{
    public bool IsSuccess { get; set; }
    public string OrderId { get; set; }
    public string SessionId { get; set; }
    public string Url { get; set; }
    public string ErrorMessage { get; set; }
}