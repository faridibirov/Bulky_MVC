using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bulky.Models;
using Microsoft.Extensions.Configuration;

public class BankPaymentService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public BankPaymentService(HttpClient httpClient, IConfiguration configuration)
    {
        _configuration = configuration;

        var certPath = _configuration["BankApi:CertPath"];
        var certPassword = _configuration["BankApi:CertPassword"];
        var cert = new X509Certificate2(certPath, certPassword);

        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(cert);

        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            return true; // For testing purposes, accept any certificate
        };

        _httpClient = new HttpClient(handler);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "YourAppName");
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
                    new XElement("Amount", (orderHeader.OrderTotal * 100).ToString("F0")),
                    new XElement("Currency", "944"),
                    new XElement("Description", $"Order {orderHeader.Id}"),
                    new XElement("ApproveURL", $"{returnUrl}Approve"),
                    new XElement("CancelURL", $"{returnUrl}Cancel"),
                    new XElement("DeclineURL", $"{returnUrl}Decline")
                )
            )
        );

        var content = new StringContent(requestXml.ToString(), Encoding.UTF8, "application/xml");

        // Additional headers if required by the bank
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer your_access_token");

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

                var redirectUrl = $"{url}?ORDERID={orderId}&SESSIONID={sessionId}";

                return new PaymentResult
                {
                    IsSuccess = true,
                    RedirectUrl = redirectUrl
                };
            }
        }

        return new PaymentResult { IsSuccess = false, ErrorMessage = "Payment request failed." };
    }
}

public class PaymentResult
{
    public bool IsSuccess { get; set; }
    public string RedirectUrl { get; set; }
    public string ErrorMessage { get; set; }
}
